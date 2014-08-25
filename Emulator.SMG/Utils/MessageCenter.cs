using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.Sqlite;
using SMG.Sqlite.Models;
using SMG.Sqlite.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Emulator.SMG.Utils
{
    public class MessageCenter
    {

        private static readonly Lazy<MessageCenter> instance = new Lazy<MessageCenter>(() => new MessageCenter(), true);

        public static MessageCenter GetInstance()
        {
            return instance.Value;
        }

        private MessageCenter()
        {
            submitLocker = new ReaderWriterLockSlim();
            submitQueue = new Queue<Deliver>();
            deliverLocker = new ReaderWriterLockSlim();
            deliverQueue = new Queue<Deliver>();
        }


        #region 转发手机相关

        ReaderWriterLockSlim submitLocker;
        Queue<Deliver> submitQueue;
        System.Timers.Timer submitTimer;
        SMSCServerHandler smscHandler;

        public void RegisterSMSC(SMSCServerHandler handler)
        {
            this.smscHandler = handler;
            if (submitTimer != null)
            {
                submitTimer.Stop();
            }
            submitTimer = new System.Timers.Timer(1000);
            submitTimer.Elapsed += (sender, e) =>
            {
                submitLocker.EnterWriteLock();

                if (submitQueue.Count > 0)
                {
                    var deliver = submitQueue.Dequeue();
                    var client = handler.GetMTClient(deliver.UserNumber);

                    //若手机客户端在线则转发，不在线则丢弃等待下次上线再转发
                    if (client != null)
                    {
                        client.Socket.Send(deliver.GetBytes());
                    }
                }

                submitLocker.ExitWriteLock();
            };

            submitTimer.Start();
        }

        public void UnRegisterSMSC()
        {
            if (smscHandler != null)
            {
                try
                {
                    submitTimer.Stop();
                    submitQueue.Clear();
                    smscHandler = null;
                }
                catch
                {                  
                    throw;
                }
            }
        }

        public void Commit(Submit submit)
        {
            try
            {
                submitLocker.EnterWriteLock();

                var deliver = new Deliver
                {
                    SPNumber = submit.SPNumber,
                    UserNumber = submit.UserNumber,
                    MessageCoding = MessageCodes.GBK,
                    MessageContent = submit.MessageContent,
                    TP_pid = 0,
                    TP_udhi = 0
                };
                //添加到转发消息队列
                submitQueue.Enqueue(deliver);
                //映射序列号
                smscHandler.MapSequeue(deliver.SequenceNumberString, submit.SequenceNumberString);

                //添加发送报告添加到数据库
                var mReport = new MReport
                {
                    TargetSubmitSequenceNumber = submit.SequenceNumber,
                    SubmitSequenceNumber = submit.SequenceNumberString,
                    UserNumber = deliver.UserNumber,
                    SPNumber = deliver.SPNumber,
                    ReportType = (int)ReportTypes.PerSubmit,
                    ErrorCode = 0,
                    State = (int)ReportStatus.Wait,
                    Status = 0,
                    Created = DateTime.Now
                };
                StorageProvider<ReportStorage>.GetStorage().Insert(mReport);
            }
            catch
            {
                throw;
            }
            finally
            {
                submitLocker.ExitWriteLock();
            }
        }

        #endregion

        #region 转发SP相关

        ReaderWriterLockSlim deliverLocker;
        Queue<Deliver> deliverQueue;
        System.Timers.Timer deliverTimer;
        SPServerHandler spHandler;

        public void RegisterSP(SPServerHandler handler)
        {
            this.spHandler = handler;

            if (deliverTimer != null)
            {
                deliverTimer.Stop();
            }
            deliverTimer = new System.Timers.Timer(1000);
            deliverTimer.Elapsed += (sender, e) =>
            {
                deliverLocker.EnterWriteLock();

                if (submitQueue.Count > 0)
                {
                    var deliver = deliverQueue.Dequeue();
                    var client = handler.GetSPClient(deliver.SPNumber);

                    //若SP客户端在线则转发，不在线则丢弃等待下次上线再转发
                    if (client != null)
                    {
                        client.Socket.Send(deliver.GetBytes());
                    }
                }

                deliverLocker.ExitWriteLock();
            };
            deliverTimer.Start();
        }

        public void UnRegisterSP()
        {
            if (spHandler != null)
            {
                try
                {
                    deliverTimer.Stop();
                    deliverQueue.Clear();
                    spHandler = null;
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Commit(Deliver deliver)
        {
            submitLocker.EnterWriteLock();
            //添加到消息队列
            deliverQueue.Enqueue(deliver);
            submitLocker.ExitWriteLock();
        }

        public void Commit(MReport mReport)
        {
            var client = spHandler.GetSPClient(mReport.SPNumber);
            //若SP客户端在线则转发，不在线则丢弃等待下次上线再转发
            if (client != null)
            {
                var r = new Report
                {
                    SubmitSequenceNumber = mReport.TargetSubmitSequenceNumber,
                    ReportType = (uint)mReport.ReportType,
                    State = (uint)mReport.State,
                    ErrorCode = (uint)mReport.ErrorCode,
                    UserNumber = mReport.UserNumber
                };
                client.Socket.Send(r.GetBytes());
                //映射序列号
                spHandler.MapSequeue(r.SequenceNumberString, mReport.SubmitSequenceNumber);
            }
        }

        #endregion

    }
}
