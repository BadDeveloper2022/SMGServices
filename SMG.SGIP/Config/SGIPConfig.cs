using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SMG.SGIP.Config
{
    [XmlRoot("SGIPConfig")]
    public class SGIPConfig
    {
        #region Properties

        /// <summary>
        /// 通信协议
        /// </summary>
        [XmlElement("Agreement")]
        public string Agreement { get; set; }

        /// <summary>
        /// 登录类型
        /// </summary>
        [XmlElement("LoginType")]
        public uint LoginType { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        [XmlElement("ServerIP")]
        public string ServerIP { get; set; }

        /// <summary>
        /// 服务器MT端口
        /// </summary>
        [XmlElement("ServerMTPort")]
        public uint ServerMTPort { get; set; }

        /// <summary>
        /// 服务器MO端口
        /// </summary>
        [XmlElement("ServerMOPort")]
        public uint ServerMOPort { get; set; }

        /// <summary>
        /// SP用户名
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// SP密码
        /// </summary>
        [XmlElement("Password")]
        public string Password { get; set; }

        /// <summary>
        /// SP的接入号码
        /// </summary>
        [XmlElement("SPNumber")]
        public string SPNumber { get; set; }

        /// <summary>
        /// 企业代码，取值范围0-99999 
        /// </summary>
        [XmlElement("CorpID")]
        public string CorpID { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        [XmlElement("NodeID")]
        public string NodeID { get; set; }

        /// <summary>
        /// 付费号码，手机号码前加“86”国别标志
        /// </summary>
        [XmlElement("ChargeNumber")]
        public string ChargeNumber { get; set; }

        /// <summary>
        /// 业务代码，由SP定义
        /// </summary>
        [XmlElement("ServiceType")]
        public string ServiceType { get; set; }

        /// <summary>
        /// 计费类型
        /// </summary>
        [XmlElement("FeeType")]
        public uint FeeType { get; set; }

        /// <summary>
        /// 取值范围0-99999，该条短消息的收费值，单位为分
        /// </summary>
        [XmlElement("FeeValue")]
        public string FeeValue { get; set; }

        /// <summary>
        /// 取值范围0-99999，赠送用户的话费，单位为分
        /// </summary>
        [XmlElement("GivenValue")]
        public string GivenValue { get; set; }

        /// <summary>
        /// 代收费标志，0：应收；1：实收
        /// </summary>
        [XmlElement("AgentFlag")]
        public uint AgentFlag { get; set; }

        /// <summary>
        /// 引起MT消息的原因
        ///    0-MO点播引起的第一条MT消息；
        ///    1-MO点播引起的非第一条MT消息；
        ///    2-非MO点播引起的MT消息；
        ///    3-系统反馈引起的MT消息。
        /// </summary>
        [XmlElement("MorelatetoMTFlag")]
        public uint MorelatetoMTFlag { get; set; }

        /// <summary>
        /// 优先级0-9从低到高，默认为0
        /// </summary>
        [XmlElement("Priority")]
        public uint Priority { get; set; }

        /// <summary>
        /// 状态报告标记
        /// 0-该条消息只有最后出错时要返回状态报告
        /// 1-该条消息无论最后是否成功都要返回状态报告
        /// 2-该条消息不需要返回状态报告
        /// 3-该条消息仅携带包月计费信息，不下发给用户，要返回状态报告
        /// 其它-保留
        /// 缺省设置为0
        /// </summary>
        [XmlElement("ReportFlag")]
        public uint ReportFlag { get; set; }

        /// <summary>
        /// GSM协议类型。
        /// </summary>
        [XmlElement("TP_pid")]
        public uint TP_pid { get; set; }

        /// <summary>
        /// GSM协议类型。仅使用1位，右对齐
        /// </summary>
        [XmlElement("TP_udhi")]
        public uint TP_udhi { get; set; }

        /// <summary>
        /// 短消息的编码格式
        /// 0：纯ASCII字符串
        /// 3：写卡操作
        /// 4：二进制编码
        /// 8：UCS2编码
        /// 15: GBK编码
        /// 其它参见GSM3.38第4节：SMS Data Coding Scheme
        /// </summary>
        [XmlElement("MessageCoding")]
        public uint MessageCoding { get; set; }

        /// <summary>
        /// 信息类型：0-短消息信息 其它：待定
        /// </summary>
        [XmlElement("MessageType")]
        public uint MessageType { get; set; }

        #endregion

        public static SGIPConfig GetInstance(string configPath)
        {
            MemoryStream ms = null;
            SGIPConfig config = null;

            if (!String.IsNullOrEmpty(configPath))
            {
                if (File.Exists(configPath))
                {
                    try
                    {
                        XmlDocument xd = new XmlDocument();
                        xd.Load(configPath);

                        //打开文件读到数据流                                    
                        ms = new MemoryStream();
                        xd.Save(ms);
                        ms.Position = 0;

                        XmlSerializer serializer = new XmlSerializer(typeof(SGIPConfig));
                        config = (SGIPConfig)serializer.Deserialize(ms);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (ms != null)
                        {
                            ms.Close();
                        }
                    } 
                }
            }

            return config;
        } 

    }
}
