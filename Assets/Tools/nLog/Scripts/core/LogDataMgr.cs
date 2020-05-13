using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
namespace NLog
{
    /// <summary>
    /// 存储所有log数据
    /// 
    /// 功能说明
    /// 数据越界处理
    /// </summary>
    public class LogDataMgr
    {
        private static List<LogStruct> LogList = new List<LogStruct>();

        private static List<LogStruct> InfoList = new List<LogStruct>();

        private static List<LogStruct> ErrorList = new List<LogStruct>();
            
        private static Dictionary<string, List<LogStruct>> TagLogDic = new Dictionary<string, List<LogStruct>>();

        private static readonly Dictionary<string, List<LogStruct>> TagInfoLogDic = new Dictionary<string, List<LogStruct>>();

        private static readonly Dictionary<string, List<LogStruct>> TagErrorLogDic = new Dictionary<string, List<LogStruct>>();

        public static void AddLog(LogStruct log)
        {           
            SafeAdd(ref LogList, log);

            if (log.Type == LogType.Log)
            {
                SafeAdd(ref InfoList,log);
            }
            else if (log.Type == LogType.Error)
            {               
                SafeAdd(ref ErrorList,log);               
            }

        }

        public static void SafeAdd(ref List<LogStruct> list, LogStruct log)
        {
            int count = list.Count;

            list.Add(log);

            //if (count >= LogDefault.MAX_LEN4_SCREEN)
            //{
            //    int diff = count - LogDefault.MAX_LEN4_SCREEN;

            //    list.RemoveRange(0, diff);
            //}
            
        }

        public static void SafeAdd(ref Dictionary<string, List<LogStruct>> dic, string tag, LogStruct log)
        {
            if (!dic.ContainsKey(tag))
            {
                dic.Add(tag, new List<LogStruct>());
            }

            if (null == dic[tag])
            {
                dic[tag] = new List<LogStruct>();
            }

            var list = dic[tag];

            list.Add(log);

        }

        public static void Clear()
        {
            LogList.Clear();
            InfoList.Clear();
            ErrorList.Clear();
            TagLogDic.Clear();
            TagInfoLogDic.Clear();
            TagErrorLogDic.Clear();
        }

        static List<LogStruct> tempList4Filter = new List<LogStruct>();
        static List<LogStruct> tempList4Filter2 = new List<LogStruct>();
        public static List<LogStruct> GetFilterLog()
        {
            tempList4Filter.Clear();
            tempList4Filter2.Clear();
            bool hasTag = true;

            if (LogConfig.Tag.Equals(LogDefault.TAG_NAME_ALL))
            {
                hasTag = false;
            }

            //log level，log tag过滤
            switch (LogConfig.Level)
            {
                case LogDefault.LEVEL_ALL:
                    if (hasTag)
                    {
                        if(TagLogDic[LogConfig.Tag].Count>0) tempList4Filter.AddRange(TagLogDic[LogConfig.Tag]);
                    }
                    else
                    {
                        if(LogList.Count>0) tempList4Filter.AddRange(LogList);
                    }
                    break;
                case LogDefault.LEVEL_INFO:
                    if (hasTag)
                    {                        
                        if(TagInfoLogDic[LogConfig.Tag].Count>0) tempList4Filter.AddRange(TagInfoLogDic[LogConfig.Tag]);
                    }
                    else
                    {
                        if(InfoList.Count>0)tempList4Filter.AddRange(InfoList);
                    }
                    break;
                case LogDefault.LEVEL_ERROR:
                    if (hasTag)
                    {
                        if(TagErrorLogDic[LogConfig.Tag].Count>0) tempList4Filter.AddRange(TagErrorLogDic[LogConfig.Tag]);                       
                    }
                    else
                    {
                        //Debug.Log("count------:"+ ErrorList.Count);
                        if (ErrorList.Count > 0)
                        {
                            tempList4Filter.AddRange(ErrorList);
                        }
                                             
                    }
                    break;
                case LogDefault.LEVEL_NOTHING:
                    tempList4Filter.Clear();
                    break;
            }

            

            if (!LogConfig.FilterRegex.Equals(LogDefault.REGEX_DEFAULT))
            {
                //log 正则过滤
                Regex regex = new Regex(LogConfig.FilterRegex);

                for (int i = 0; i < tempList4Filter.Count; i++)
                {
                    LogStruct logStruct = tempList4Filter[i];
                    if (regex.IsMatch(tempList4Filter[i].Msg))
                    {
                        //Debug.Log("删除:" + tempList4Filter[i].Msg);
                        tempList4Filter2.Add(logStruct);
                    }
                }

                tempList4Filter2.Reverse();
                return tempList4Filter2;

            }
            else
            {
                //无正则过滤
                tempList4Filter.Reverse();
                return tempList4Filter;
            }
                       
            
        }
    }
}

