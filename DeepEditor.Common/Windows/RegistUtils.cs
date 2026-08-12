using DeepCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common
{
    public static class RegistUtils
    {
        /// <summary>
        /// 从注册表里读取信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetAppRegistry<T>(string key, out T value)
        {
            return TryGetAppRegistry<T>(null, key, out value);
        }
        /// <summary>
        /// 在注册表里写入信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void PutAppRegistry<T>(string key, T value)
        {
            PutAppRegistry<T>(null, key, value);
        }
        /// <summary>
        /// 从注册表里读取信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">应用子目录</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetAppRegistry<T>(string path, string key, out T value)
        {
            RegistryKey masterKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\" + Application.CompanyName + "\\" + Application.ProductName + ((path != null) ? ("\\" + path) : ("")));
            try
            {
                if (masterKey != null)
                {
                    object obj = masterKey.GetValue(key);
                    if (obj != null)
                    {
                        value = (T)Parser.StringToObject(obj.ToString(), typeof(T));
                        return true;
                    }
                }
            }
            finally
            {
                masterKey.Close();
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// 在注册表里写入信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">应用子目录</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void PutAppRegistry<T>(string path, string key, T value)
        {
            RegistryKey masterKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\" + Application.CompanyName + "\\" + Application.ProductName + ((path != null) ? ("\\" + path) : ("")));
            try
            {
                if (masterKey != null)
                {
                    masterKey.SetValue(key, Parser.ObjectToString(value));
                }
            }
            finally
            {
                masterKey.Close();
            }
        }

    }
}
