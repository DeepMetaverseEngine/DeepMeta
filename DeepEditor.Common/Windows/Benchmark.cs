using DeepCore;
using DeepCore.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common
{
    public class Benchmark
    {
        private int bytes_len = 1024;
        private int array_len = 10;
        private Random random = new Random();

        public void SetBytesLength(int len)
        {
            bytes_len = len;
        }
        public void SetArrayLength(int len)
        {
            array_len = len;
        }

        public byte[] RandomBytes()
        {
            var bin = new byte[random.Next(0, bytes_len)];
            for (int i = 0; i < bin.Length; i++)
            {
                bin[i] = (byte)random.Next();
            }
            return bin;
        }

        public object RandomEnum(Type type)
        {
            var array = Enum.GetValues(type);
            return array.GetValue(random.Next(array.Length));
        }

        public Array RandomArray(Type elementType)
        {
            var len = random.Next(0, array_len);
            var array = Array.CreateInstance(elementType, len);
            for (int i = 0; i < len; i++)
            {
                array.SetValue(RandomType(elementType), i);
            }
            return array;
        }

        public IList RandomList(Type type)
        {
            var list = DeepActivator.CreateInstance(type) as IList;
            var etype = typeof(object);
            if (type.IsGenericType)
            {
                etype = type.GenericTypeArguments[0];
            }
            var len = random.Next(0, array_len);
            for (int i = 0; i < len; i++)
            {
                list.Add(RandomType(etype));
            }
            return list;
        }

        public IDictionary RandomMap(Type type)
        {
            var map = DeepActivator.CreateInstance(type) as IDictionary;
            var ktype = typeof(object);
            var vtype = typeof(object);
            if (type.IsGenericType)
            {
                ktype = type.GenericTypeArguments[0];
                vtype = type.GenericTypeArguments[1];
            }
            var len = random.Next(0, array_len);
            for (int i = 0; i < len; i++)
            {
                var k = RandomType(ktype);
                var v = RandomType(vtype);
                map[k] = v;
            }
            return map;
        }

        public object RandomType(Type ft)
        {
            try
            {
                if (ft == typeof(sbyte))
                {
                    return (sbyte)random.Next();
                }
                else if (ft == typeof(int))
                {
                    return (int)random.Next();
                }
                else if (ft == typeof(short))
                {
                    return (short)random.Next();
                }
                else if (ft == typeof(long))
                {
                    return (long)random.Next();
                }
                else if (ft == typeof(byte))
                {
                    return (byte)random.Next();
                }
                else if (ft == typeof(uint))
                {
                    return (uint)random.Next();
                }
                else if (ft == typeof(ushort))
                {
                    return (ushort)random.Next();
                }
                else if (ft == typeof(ulong))
                {
                    return (ulong)random.Next();
                }
                else if (ft == typeof(float))
                {
                    return (float)random.NextDouble();
                }
                else if (ft == typeof(double))
                {
                    return (double)random.NextDouble();
                }
                else if (ft == typeof(string))
                {
                    return random.Next().ToString();
                }
                else if (ft == typeof(byte[]))
                {
                    return RandomBytes();
                }
                else if (ft.IsEnum)
                {
                    return RandomEnum(ft);
                }
                else if (ft.IsArray)
                {
                    return RandomArray(ft.GetElementType());
                }
                else if (ft.GetInterface(typeof(IList).Name) != null)
                {
                    return RandomList(ft);
                }
                else if (ft.GetInterface(typeof(IDictionary).Name) != null)
                {
                    return RandomMap(ft);
                }
                else if (ft.IsClass)
                {
                    var obj = DeepActivator.CreateInstance(ft);
                    RandomFillObject(ft);
                    return obj;
                }
            }
            catch
            {
            }
            return null;
        }

        public void RandomFillObject(object obj)
        {
            var type = obj.GetType();
            foreach (var f in type.GetFields())
            {
                try
                {
                    var fv = RandomType(f.FieldType);
                    f.SetValue(obj, fv);
                }
                catch
                {

                }
            }
        }


    }
}
