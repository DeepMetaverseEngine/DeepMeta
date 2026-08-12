using DeepCore;
using DeepCore.IO;
using DeepCore.Xml;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Reflection.Emit;
using System.Windows.Forms;
using static DeepEditor.Common.G2D.DataGrid.G2DTypeDescriptor;

namespace DeepEditor.Common.Windows
{
    //-----------------------------------------------------------------------------------------------------------------------------
    public class CopyPaste : Disposable
    {
        const long DEFAULT_CAPACITY = 1024 * 1024 * 10;
        //-------------------------------------------------------------------------------------------------------------
        private readonly string type;
        private readonly MemoryMappedFile mmf;
        public CopyPaste(Type type, long capacity = DEFAULT_CAPACITY) : this(type.FullName, capacity) { }
        public CopyPaste(string type, long capacity = DEFAULT_CAPACITY)
        {
            this.type = type;
            this.mmf = MemoryMappedFile.CreateOrOpen(type, capacity, MemoryMappedFileAccess.ReadWrite);
            Application.ApplicationExit += Application_ApplicationExit;
        }
        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            this.Dispose();
        }
        protected override void Disposing()
        {
            this.mmf.Dispose();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool Copy(object data)
        {
            return Copy($"{data}", data);
        }
        public bool Copy(string name, object data)
        {
            try
            {
                //进程间同步                    
                using (var stream = mmf.CreateViewStream()) //创建文件内存视图流 基于流的操作
                {
                    if (name != null)
                    {
                        var bin = CUtils.UTF8.GetBytes(name);
                        LittleEdian.PutS32(stream, bin.Length);
                        IOUtil.WriteToEnd(stream, bin);
                    }
                    else
                    {
                        LittleEdian.PutS32(stream, 0);
                    }
                    if (data != null)
                    {
                        var xml = XmlUtil.ObjectToXmlString(data);
                        var bin = CUtils.UTF8.GetBytes(xml);
                        LittleEdian.PutS32(stream, bin.Length);
                        IOUtil.WriteToEnd(stream, bin);
                    }
                    else
                    {
                        LittleEdian.PutS32(stream, 0);
                    }
                    stream.Flush();
                    return true;
                }
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool TryPaste(Type decleardType, out string name, out object data)
        {
            try
            {
                //进程间同步
                using (var stream = mmf.CreateViewStream()) //创建文件内存视图流 基于流的操作
                {
                    var len = LittleEdian.GetS32(stream);
                    if (len > 0)
                    {
                        var bin = new byte[len];
                        IOUtil.ReadToEnd(stream, bin);
                        name = CUtils.UTF8.GetString(bin);
                    }
                    else
                    {
                        name = null;
                    }
                    len = LittleEdian.GetS32(stream);
                    if (len > 0)
                    {
                        var bin = new byte[len];
                        IOUtil.ReadToEnd(stream, bin);
                        var xml = CUtils.UTF8.GetString(bin);
                        data = XmlUtil.XmlTextToObject(xml, decleardType);
                        return true;
                    }
                }
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            name = null;
            data = null;
            return false;
        }
        public bool TryPaste(Type decleardType, out object data)
        {
            if (TryPaste(decleardType, out var name, out data))
            {
                return true;
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool TryPaste(out string name, out object data)
        {
            return TryPaste(null, out name, out data);
        }
        public bool TryPaste(out object data)
        {
            if (TryPaste(null, out var name, out data))
            {
                return true;
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool TryPaste<T>(out T data)
        {
            if (TryPaste<T>(out var name, out data))
            {
                return true;
            }
            return false;
        }
        public bool TryPaste<T>(out string name, out T data)
        {
            if (TryPaste(typeof(T), out name, out var vdata))
            {
                data = (T)vdata;
                return true;
            }
            data = default(T);
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool HasData
        {
            get
            {
                try
                {
                    //进程间同步
                    using (var stream = mmf.CreateViewStream()) //创建文件内存视图流 基于流的操作
                    {
                        var len = LittleEdian.GetS32(stream);
                        if (len > 0)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception err)
                {
                    err.PrintStackTrace();
                }
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool Clear()
        {
            try
            {
                //进程间同步                    
                using (var stream = mmf.CreateViewStream()) //创建文件内存视图流 基于流的操作
                {
                    LittleEdian.PutS32(stream, 0);
                    stream.Flush();
                }
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            return false;
        }

        //-------------------------------------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------------------------------------------------------------
    public static class CopyHistory
    {
        private static HashMap<Type, List<Entry>> copying_history = new HashMap<Type, List<Entry>>();
        private static List<Entry> copying_history_primitive = new List<Entry>();
        private static int max_history_count = 22;
        private static CopyPaste s_copying = new CopyPaste(typeof(CopyHistory).FullName);

        public static int CopyHistoryLimit
        {
            get { return max_history_count; }
            set { if (value >= 20) { max_history_count = value; } }
        }
        public static List<Entry> GetHistoryList(Type decleard_type)
        {
            if (decleard_type.IsPrimitive && decleard_type.IsValueType && (!decleard_type.Equals(typeof(bool))))
            {
                return copying_history_primitive;
            }
            List<Entry> ret = copying_history.Get(decleard_type);
            return ret;
        }
        public static bool AddHistory(Type decleardFieldType, object owner, Entry copying_value, out List<Entry> out_list)
        {
            //var copying_value = new Entry(owner, decleardFieldType, value, label, clipText);
            var list = GetHistoryList(decleardFieldType);
            if (list != null)
            {
                foreach (var old in list)
                {
                    if (old.data.Equals(copying_value.data) || old.EqualsXmlText(copying_value.xml_text))
                    {
                        old.Renew(owner);
                        list.Sort();
                        out_list = list;
                        return false;
                    }
                }
            }
            else
            {
                list = new List<Entry>();
                copying_history.Put(decleardFieldType, list);
            }
            try
            {
                Win32.SetClipboard(copying_value.cvt_text);
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            list.Add(copying_value);
            list.Sort();
            if (list.Count > max_history_count)
            {
                list.RemoveRange(max_history_count, list.Count - max_history_count);
            }
            out_list = list;
            return true;
        }

        public static bool PushCopy(Type decleardFieldType, object value, out List<Entry> out_list)
        {
            var copying_value = new Entry(null, decleardFieldType, value, value.GetType().Name, value.ToString());
            return AddHistory(decleardFieldType, null, copying_value, out out_list);
        }
        public static bool Copy(object owner, GridItem item, out List<Entry> out_list)
        {
            if (item != null && item.PropertyDescriptor is G2DOwnerPropertyDescriptor g2dpp)
            {
                if (item.Value != null)
                {
                    var clipText = item.Value.ToString();
                    if (item.PropertyDescriptor.Converter != null)
                    {
                        clipText = item.PropertyDescriptor.Converter.ConvertToString(item.Value);
                    }
                    var copying_value = new Entry(owner, g2dpp.DecleardFieldType, item.Value, item.Label, clipText);
                    s_copying.Copy(copying_value);
                    return AddHistory(g2dpp.DecleardFieldType, owner, copying_value, out out_list);
                }
            }
            out_list = null;
            return false;
        }

        public static Entry Paste(Type decleard_type)
        {
            if (s_copying.TryPaste<Entry>(out var copying_value))
            {
                return copying_value;
            }
            List<Entry> list = GetHistoryList(decleard_type);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }
        public static int GetHistoryListCount(Type decleard_type)
        {
            List<Entry> ret = GetHistoryList(decleard_type);
            if (ret != null)
            {
                return ret.Count;
            }
            return 0;
        }
        //----------------------------------------------------------------------------------
        public class Entry : IComparable<Entry>
        {
            public object data;
            public string cvt_text;
            public string xml_text;
            public string lbl_suffix;
            public DateTime time;
            public string lbl_text;
            public string lst_text;
            public Entry() { }
            public Entry(object owner, Type expect_type, object value, string label, string cvt_text)
            {
                this.time = DateTime.Now;
                this.data = value;
                this.xml_text = XmlUtil.ToString(new XmlSerializer(true).ObjectToXml(value, "data"));
                this.cvt_text = value.ToString();
                this.lst_text = string.Format("[{0}] {1}", expect_type.Name, cvt_text);
                this.lbl_suffix = string.Format("{0} = {1}", label, lst_text);
                this.lbl_text = string.Format("{0}:{1}", owner, lbl_suffix);
            }
            public void Renew(object owner)
            {
                this.time = DateTime.Now;
                this.lbl_text = string.Format("{0}:{1}", owner, lbl_suffix);
                try
                {
                    Win32.SetClipboard(cvt_text);
                }
                catch (Exception err)
                {
                    err.PrintStackTrace();
                }
            }
            public bool CanConvertTo(Type decleard_type)
            {
                if (TryConvertTo(data, decleard_type))
                {
                    return true;
                }
                return false;
            }
            public int CompareTo(Entry other)
            {
                return -this.time.CompareTo(other.time);
            }
            public object CloneData(Type decleard_type)
            {
                object value = new XmlSerializer(true).XmlToObject(data.GetType(), XmlUtil.FromString(xml_text));
                object target;
                if (TryConvertTo(value, decleard_type, out target))
                {
                    return target;
                }
                return value;
            }
            public bool EqualsXmlText(string x)
            {
                return this.xml_text.Equals(x);
            }
            private static bool TryConvertTo(object src, Type targetType)
            {
                object target;
                return TryConvertTo(src, targetType, out target);
            }
            private static bool TryConvertTo(object src, Type targetType, out object target)
            {
                if (targetType.IsInstanceOfType(src))
                {
                    target = src;
                    return true;
                }
                if (targetType.IsPrimitive && src.GetType().IsPrimitive)
                {
                    try
                    {
                        target = Convert.ChangeType(src, targetType);
                        if (targetType.IsInstanceOfType(target))
                        {
                            return true;
                        }
                    }
                    catch (Exception err)
                    {
                        err.PrintStackTrace();
                    }
                }
                target = null;
                return false;
            }

        }
        //----------------------------------------------------------------------------------

    }

}
