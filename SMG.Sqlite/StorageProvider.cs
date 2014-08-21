using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SMG.Sqlite
{
    public sealed class StorageProvider<T> where T : BaseStorage
    {
        private static readonly Lazy<T> storage = new Lazy<T>(() =>
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return (T)asm.CreateInstance(typeof(T).FullName, false,
                BindingFlags.Default | BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic
                , null, null, null, null);
        }, true);

        public static T GetStorage()
        {
            return storage.Value;
        }
    }
}
