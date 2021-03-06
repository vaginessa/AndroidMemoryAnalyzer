﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AndroidMemoryAnalyzer.HeapAnalyzer;

namespace Demo.Library.MicroMsg
{
    public class MicroMsgHelper
    {
        public static string GetJavaString(ObjectInstanceInfo info)
        {
            if (info == null) return null;
            if (info.ClassName != "java.lang.String") return null;
            var tt1 = ((info as ObjectInstanceInfo).InstanceFields[0] as ReferenceObjectInfo);
            if (tt1.ReferenceTarget == null) return null;
            var str_value = (tt1.ReferenceTarget as PrimitiveArrayInfo).StringData;
            return str_value;
        }

        public static string GetSpannableString(ObjectInstanceInfo info)
        {
            if (info == null) return null;
            var tt1 = ((info as ObjectInstanceInfo).InstanceFields[0] as ReferenceObjectInfo);
            if (tt1.ReferenceTarget == null) return null;
            var str_value = GetJavaString(tt1.ReferenceTarget as ObjectInstanceInfo);
            return str_value;
        }
        public static List<MicroMsgFriend> GetFriends(HeapFileAnalyzer analyser)
        {
            List<MicroMsgFriend> result = new List<MicroMsgFriend>();
            var t = analyser.ObjectInstanceInfos.Where(c => c.ClassName == "com.tencent.mm.storage.w").ToList();
            
            foreach (var it in t)
            {
                var friend = new MicroMsgFriend();
                foreach (var it2 in it.InstanceFields)
                {
                    switch (it2.Name)
                    {
                        case "field_alias":
                            friend.UIN = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        case "field_username":
                            friend.UserName = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        case "field_conRemark":
                            friend.Remark = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        case "field_nickname":
                            friend.NickName = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        case "gqq":
                            friend.Province = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        case "gqr":
                            friend.City = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        case "signature":
                            friend.Signature = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        default:
                            break;
                    }
                }
                result.Add(friend);
            }
            return result;
        }

        public static List<MicroMsgMessage> GetMessages(HeapFileAnalyzer analyser)
        {
            List<MicroMsgMessage> result = new List<MicroMsgMessage>();
            var t = analyser.ObjectInstanceInfos.Where(c => c.ClassName == "com.tencent.mm.ui.conversation.b$d").ToList();

            foreach (var it in t)
            {
                var msg = new MicroMsgMessage();
                foreach (var it2 in it.InstanceFields)
                {
                    switch (it2.Name)
                    {
                        case "nickName":
                            msg.NickName = GetSpannableString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        case "uoB":
                            msg.Message = GetSpannableString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        case "uoA":
                            msg.TimeText = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;
                        
                        default:
                            break;
                    }
                }
                result.Add(msg);
            }
            return result;
        }

        public static string GetAccount(HeapFileAnalyzer analyser)
        {
            var t = analyser.ObjectInstanceInfos.Where(c => c.ClassName == "java.util.HashMap$HashMapEntry").ToList();

            foreach (var it in t)
            {

                string tempKey = "";
                string tempValue = "";
                foreach (var it2 in it.InstanceFields)
                {
                    switch (it2.Name)
                    {
                        case "key":
                            tempKey = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);

                            break;
                        case "value":
                            tempValue = GetJavaString((it2 as ReferenceObjectInfo).ReferenceTarget as ObjectInstanceInfo);
                            break;

                        default:
                            break;
                    }
                }
                if (tempKey == "login_weixin_username")
                {
                    return tempValue;
                }
            }
            return null;
        }
    }
}
