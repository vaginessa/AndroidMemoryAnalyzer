﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidMemoryAnalyzer.HeapAnalyzer;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AndroidMemoryAnalyzer
{
    static public class BaiduPan
    {
        public static List<ObjectInstanceInfo> getObjectListContainID(HeapFileAnalyzer analyser, int id)
        {
            var temp = analyser.ObjectInstanceInfos.
                    Where(c => c.InstanceFields != null
                        && c.InstanceFields.Exists(i => i.Value.Equals(id)
                        )
                ).ToList();
            return temp;
        }

        public static List<ObjectArrayInfo> getObjectArrayListContainID(HeapFileAnalyzer analyser, int id)
        {
            var temp = analyser.ObjectArrayInfos.
                    Where(c => c.ElementIDs != null
                        && c.ElementIDs.Exists(i => i.Equals(id)
                        )
                ).ToList();
            return temp;
        }

        public static void DoWork()
        {
            string path = @"F:\工作项目\内存提取\test\新真机集合\com.baidu.netdisk.hprof";
            HeapFileAnalyzer x = new HeapFileAnalyzer(path);
            x.DoWork();
            lookForMessage2(x);
            //lookForText(x);

        }
        static void lookForMessage2(HeapFileAnalyzer analyser)
        {
            var t = analyser.PrimitiveArrayInfos.Where(c => c.StringData != null
                && c.StringData.Contains("pdf")).ToList();
            List<ObjectInstanceInfo> tstring = new List<ObjectInstanceInfo>();
            foreach (var it in t)
            {
                var temp = analyser.ObjectInstanceInfos.
                    Where(c => c.InstanceFields != null
                        && c.InstanceFields.Exists(i => i.Value.Equals(it.PrimitiveArrayID)
                        )
                ).FirstOrDefault();
                if (temp != null)
                    tstring.Add(temp);
            }
            
            var tObjectInfo = new List<List<ObjectInstanceInfo>>();
            var tObjectArrayInfo = new List<List<ObjectArrayInfo>>();
            foreach (var it in tstring)
            {
                tObjectInfo.Add(getObjectListContainID(analyser, it.ObjectID));
                tObjectArrayInfo.Add(getObjectArrayListContainID(analyser, it.ObjectID));
            }

            using (FileStream fsWrite = new FileStream(@"result_msg_IMG.txt", FileMode.OpenOrCreate))
            {
                foreach (var it_Main in tObjectInfo)
                {
                    foreach (var it in it_Main)
                    {
                        string line = "———————————————— \r\n";
                        byte[] myByte_line = System.Text.Encoding.Default.GetBytes(line);
                        fsWrite.Write(myByte_line, 0, myByte_line.Length);

                        byte[] myByte_ClassName = System.Text.Encoding.Default.GetBytes(it.ClassName + "\r\n");
                        fsWrite.Write(myByte_ClassName, 0, myByte_ClassName.Length);
                        foreach (var it2 in it.InstanceFields)
                        {
                            string item = null;
                            if (it2 is ReferenceObjectInfo)
                            {
                                var temp = (it2 as ReferenceObjectInfo).ReferenceTarget;
                                if (temp != null)
                                {
                                    if (temp is ObjectInstanceInfo)
                                    {
                                        var cn = (temp as ObjectInstanceInfo).ClassName;
                                        if (cn == "java.lang.String")
                                        {
                                            var tt1 = ((temp as ObjectInstanceInfo).InstanceFields[0] as ReferenceObjectInfo);

                                            if (tt1.ReferenceTarget == null)
                                            {
                                                item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, "(null)");
                                            }
                                            else
                                            {
                                                var str_value = (tt1.ReferenceTarget as PrimitiveArrayInfo).StringData;
                                                item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, str_value);
                                            }
                                        }
                                        else
                                        {
                                            item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, cn);
                                        }
                                    }
                                    else
                                    {
                                        item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, "(others)");
                                    }
                                }
                                else
                                {
                                    item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, "null");
                                }
                            }
                            else
                            {
                                item = String.Format("{0}:{1} \r\n", it2.Name, it2.Value);
                            }

                            byte[] myByte = System.Text.Encoding.Default.GetBytes(item);

                            fsWrite.Write(myByte, 0, myByte.Length);
                        }
                    }
                };

            };

        }
        static void lookForText(HeapFileAnalyzer analyser)
        {
            //com.netease.mobimail.n.c.am 邮件
            //com.netease.mobimail.n.c.k 收发人
            var t = analyser.ObjectInstanceInfos
                .Where(c => c.ClassName != null
                    && c.ClassName.Contains("com.cn21.ecloud.analysis.bean.File"))
                    
                    .ToList();
            using (FileStream fsWrite = new FileStream(@"com.cn21.ecloud.analysis.bean.File.txt", FileMode.OpenOrCreate))
            {
                foreach (var it in t)
                {
                    string line = "———————————————— \r\n";
                    byte[] myByte_line = System.Text.Encoding.UTF8.GetBytes(line);
                    fsWrite.Write(myByte_line, 0, myByte_line.Length);
                    byte[] myByte_line2 = System.Text.Encoding.UTF8.GetBytes(it.ClassName + "\r\n");
                    fsWrite.Write(myByte_line2, 0, myByte_line2.Length);
                    byte[] myByte_line3 = System.Text.Encoding.UTF8.GetBytes(it.ObjectID + "\r\n");
                    fsWrite.Write(myByte_line3, 0, myByte_line3.Length);
                    //byte[] myByte_ClassName = System.Text.Encoding.UTF8.GetBytes(it.ClassName + "\r\n");
                    //fsWrite.Write(myByte_ClassName, 0, myByte_ClassName.Length);
                    foreach (var it2 in it.InstanceFields)
                    {
                        string item = null;
                        if (it2 is ReferenceObjectInfo)
                        {
                            var temp = (it2 as ReferenceObjectInfo).ReferenceTarget;
                            if (temp != null)
                            {
                                if (temp is ObjectInstanceInfo)
                                {
                                    var cn = (temp as ObjectInstanceInfo).ClassName;
                                    if (cn == "java.lang.String")
                                    {
                                        var tt1 = ((temp as ObjectInstanceInfo).InstanceFields[0] as ReferenceObjectInfo);

                                        if (tt1.ReferenceTarget == null)
                                        {
                                            item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, "(null)");
                                        }
                                        else
                                        {
                                            var str_value = (tt1.ReferenceTarget as PrimitiveArrayInfo).StringData;
                                            item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, str_value);
                                        }
                                    }
                                    else if (cn == "android.text.SpannableString")
                                    {
                                        var tt1 = ((temp as ObjectInstanceInfo).InstanceFields[0] as ReferenceObjectInfo);

                                        if (tt1.ReferenceTarget == null)
                                        {
                                            item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, "(null)");
                                        }
                                        else
                                        {
                                            var tt2 = ((tt1.ReferenceTarget as ObjectInstanceInfo).InstanceFields[0] as ReferenceObjectInfo);

                                            if (tt2.ReferenceTarget == null)
                                            {
                                                item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, "(null)");
                                            }
                                            else
                                            {
                                                var str_value = (tt2.ReferenceTarget as PrimitiveArrayInfo).StringData;
                                                item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, str_value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, cn);
                                    }
                                }
                                else
                                {
                                    item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, "(others)");
                                }
                            }
                            else
                            {
                                item = String.Format("{0}:{1} ({2}) \r\n", it2.Name, it2.Value, "null");
                            }
                        }
                        else
                        {
                            item = String.Format("{0}:{1} \r\n", it2.Name, it2.Value);
                        }

                        byte[] myByte = System.Text.Encoding.UTF8.GetBytes(item);

                        fsWrite.Write(myByte, 0, myByte.Length);
                    }
                };

            };
        }
    }
}
