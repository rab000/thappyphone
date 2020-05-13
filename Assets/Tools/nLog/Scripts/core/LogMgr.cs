using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.IO;
using NLog.Server;

namespace NLog
{
    /// <summary>
    /// 用于真机log查看
    /// 主要功能是过滤和远程调试
    /// 同时兼顾在editor端log输出
    /// </summary>
    public class LogMgr : MonoBehaviour
    {
        private static LogMgr _Ins;

        public static LogMgr Ins
        {
            get
            {
                if (_Ins == null)
                {
                    _Ins = UnityEngine.Object.FindObjectOfType<LogMgr>();
                    if (_Ins == null)
                    {
                        GameObject obj = new GameObject(typeof(LogMgr).Name);
                        _Ins = obj.AddComponent<LogMgr>();
                    }

                    if (_Ins != null)
                    {
                        DontDestroyOnLoad(_Ins.gameObject);
                    }
                }
                return _Ins;
            }
        }

        public static List<LogStruct> CurLogList;

        public Action<List<LogStruct>> UpdateLogsEvent;

        private static IoBuffer Buffer = new IoBuffer(10240);

        private static StringBuilder SB = new StringBuilder();

        void Awake()
        {
            Application.logMessageReceived += MsgRec;           
        }

        private void Start()
        {
            if (LogConfig.BeWrite2File && File.Exists(LogConfig.LogFileSavePath))
            {
                File.Delete(LogConfig.LogFileSavePath);
            }

            File.Create(LogConfig.LogFileSavePath).Dispose();
        }

        void OnDestroy()
        {
            Application.logMessageReceived -= MsgRec;
        }

        public static void I(string msg, string tag = LogDefault.TAG_NAME_DEFALUT,LogColorEnum color = LogColorEnum.blue)
        {
            if (!LogConfig.BeShowLog) return;

            LogStruct logStruct = new LogStruct();
            logStruct.Msg = msg;
            logStruct.Tag = tag;
            logStruct.Time = LogUtils.GetCurTime();
            logStruct.LogColor = color;
            logStruct.Type = LogType.Log;           
            LogUtils.SB.Clear();
            
            var stack = new System.Diagnostics.StackTrace(true);
            var frames = stack.GetFrames();
            int length = frames.Length;

            for (int i = 0; i < length; i++)
            {               
                string fileName = frames[i].GetFileName();                
                fileName = System.IO.Path.GetFileName(fileName);
                var method = frames[i].GetMethod();
                Type classType = method.ReflectedType;
                string _nameSpace = classType.Namespace;
                string className = classType.Name;
                string methodName = method.Name;
                int lineNum = frames[i].GetFileLineNumber();

                LogUtils.SB.Append(_nameSpace);
                LogUtils.SB.Append(".");
                LogUtils.SB.Append(className);
                LogUtils.SB.Append(":");
                LogUtils.SB.Append(methodName);
                LogUtils.SB.Append("()");
                LogUtils.SB.Append(" ( at ");
                LogUtils.SB.Append(fileName);
                LogUtils.SB.Append(":");
                LogUtils.SB.Append(lineNum);
                LogUtils.SB.Append("\n");
            }

            logStruct.StackTrace = LogUtils.SB.ToString();

            LogDataMgr.AddLog(logStruct);

            //NINFO 之所以screen console分开处理，因为MsgRec不能接收到tag，color等信息
            //这也是难点，所以要么输出到screen，要么到控制台，两者同时存在也没意义
            if (LogConfig.BeWrite2Screen)
            {
                Ins.RefreshCurLogList();

                if (LogConfig.BeWirte2Remote)
                {
                    Buffer.Clear();

                    Buffer.PutString(GetStringLog(logStruct));

                    //Buffer.PutString("我");


                    byte[] bs = Buffer.ToArray();

                    //for (int i = 0; i < bs.Length; i++)
                    //{
                    //    Debug.Log("客户端发送i:"+i+"--->"+bs[i]);
                    //}

                    LogHttpClient.GetIns().Post((short)125, bs, null);

                }

                if (LogConfig.BeWrite2File)
                {
                    byte[] bs = Buffer.ToArray();
                    FileHelper.WriteBytes2File_Append(LogConfig.LogFileSavePath, bs);
                }

            }
            else
            {               
                Debug.Log(GetStringLog(logStruct));
            }

            

        }

        public static void I(string className, string funcName, string content, bool beShow = true, string tag = LogDefault.TAG_NAME_DEFALUT, LogColorEnum color = LogColorEnum.blue)
        {
            if (!LogConfig.BeShowLog) return;
            if (!beShow) return;

            if (SB.Length > 0) SB.Remove(0, SB.Length);
            SB.Append("[i]-[");
            SB.Append(className);
            SB.Append("->");
            SB.Append(funcName);
            SB.Append("][");
            SB.Append(content);
            SB.Append("]");
            string s = SB.ToString();
            I(s, tag, color);

        }

        public static void E(string msg,string tag = LogDefault.TAG_NAME_DEFALUT,LogColorEnum color = LogColorEnum.red)
        {
            if (!LogConfig.BeShowLog) return;
            LogStruct logStruct = new LogStruct();
            logStruct.Msg = msg;
            logStruct.Tag = tag;
            logStruct.Time = LogUtils.GetCurTime();
            logStruct.LogColor = color;
            logStruct.Type = LogType.Error;

            LogUtils.SB.Clear();

            var stack = new System.Diagnostics.StackTrace(true);
            var frames = stack.GetFrames();
            int length = frames.Length;

            for (int i = 0; i < length; i++)
            {
                string fileName = frames[i].GetFileName();
                fileName = System.IO.Path.GetFileName(fileName);
                var method = frames[i].GetMethod();
                Type classType = method.ReflectedType;
                string _nameSpace = classType.Namespace;
                string className = classType.Name;
                string methodName = method.Name;
                int lineNum = frames[i].GetFileLineNumber();

                LogUtils.SB.Append(_nameSpace);
                LogUtils.SB.Append(".");
                LogUtils.SB.Append(className);
                LogUtils.SB.Append(":");
                LogUtils.SB.Append(methodName);
                LogUtils.SB.Append("()");
                LogUtils.SB.Append(" ( at ");
                LogUtils.SB.Append(fileName);
                LogUtils.SB.Append(":");
                LogUtils.SB.Append(lineNum);
                LogUtils.SB.Append("\n");
            }

            logStruct.StackTrace = LogUtils.SB.ToString();

            LogDataMgr.AddLog(logStruct);

            if (LogConfig.BeWrite2Screen)
            {
                Ins.RefreshCurLogList();

                if (LogConfig.BeWirte2Remote)
                {
                    Buffer.Clear();

                    Buffer.PutString(GetStringLog(logStruct));

                    byte[] bs = Buffer.ToArray();

                    LogHttpClient.GetIns().Post((short)125, bs, null);

                }

                if (LogConfig.BeWrite2File)
                {
                    byte[] bs = Buffer.ToArray();
                    FileHelper.WriteBytes2File_Append(LogConfig.LogFileSavePath, bs);
                }
            }
            else
            {
                //MsgRec 会做log文件保存，和向远程传送log，所以使用cosonle输出不用加这两项
                Debug.Log(GetStringLog(logStruct));
            }

        }

        public static void E(string className, string funcName, string content, bool beShow=true, string tag = LogDefault.TAG_NAME_DEFALUT, LogColorEnum color = LogColorEnum.red)
        {
            if (!LogConfig.BeShowLog) return;
            if (!beShow) return;

            if (SB.Length > 0) SB.Remove(0, SB.Length);
            SB.Append("[e]-[");
            SB.Append(className);
            SB.Append("->");
            SB.Append(funcName);
            SB.Append("][");
            SB.Append(content);
            SB.Append("]");
            string s = SB.ToString();
            E(s,tag,color);
        }

        public static string GetStringLog(LogStruct log)
        {
            LogUtils.SB.Clear();

            LogUtils.SB.Append("<color=#");

            LogUtils.SB.Append(ColorUtility.ToHtmlStringRGB(LogUtils.GetColor(log.LogColor)));
            LogUtils.SB.Append(">");

            LogUtils.SB.Append("[");
            LogUtils.SB.Append(log.Tag);
            LogUtils.SB.Append("]");

            LogUtils.SB.Append("[");
            LogUtils.SB.Append(log.Time);
            LogUtils.SB.Append("]");

            LogUtils.SB.Append(":");
            LogUtils.SB.Append(log.Msg);

            LogUtils.SB.Append("</color>");

            return LogUtils.SB.ToString();
        }

        public static void ClearLog()
        {
            LogDataMgr.Clear();
            Ins.RefreshCurLogList();
        }

        public static void RegexFilter(string regex)
        {
            LogConfig.FilterRegex = regex;
            Ins.RefreshCurLogList();
        }

        /// <summary>
        /// 处理unity log
        /// 注意，这里只用于向screen输出unity本身的log信息(就是Debug.Log)
        /// 不想screen输出时这里就不要使用
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="stack"></param>
        /// <param name="type"></param>
        private void MsgRec(string msg, string stack, UnityEngine.LogType type)
        {

            if (!LogConfig.BeShowLog) return;
           
            LogStruct logStruct = new LogStruct();

            logStruct.Msg = msg;

            logStruct.Tag = LogDefault.TAG_NAME_UNITY;

            logStruct.Time = LogUtils.GetCurTime();
            
            logStruct.Type = type;

            LogUtils.SB.Clear();
         
            logStruct.StackTrace = stack;
            
            switch (type)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    logStruct.LogColor = LogColorEnum.red;
                    break;
                case LogType.Warning:
                    logStruct.LogColor = LogColorEnum.yellow;
                    break;
                case LogType.Log:
                    logStruct.LogColor = LogColorEnum.blue;
                    break;
            }
            
            if (LogConfig.BeWirte2Remote)
            {
                Buffer.Clear();

                Buffer.PutString(GetStringLog(logStruct));

                //Buffer.PutString("我");


                byte[] bs = Buffer.ToArray();

                //for (int i = 0; i < bs.Length; i++)
                //{
                //    Debug.Log("客户端发送i:"+i+"--->"+bs[i]);
                //}
                
                LogHttpClient.GetIns().Post((short)125, bs, null);

            }

            if (LogConfig.BeWrite2File)
            {
                byte[] bs = Buffer.ToArray();
                FileHelper.WriteBytes2File_Append(LogConfig.LogFileSavePath, bs);
            }


            LogDataMgr.AddLog(logStruct);

            RefreshCurLogList();
            

        }
     
        public void RefreshCurLogList()
        {
            CurLogList = LogDataMgr.GetFilterLog();

            if (null == CurLogList)
                return;

            //if (CurLogList.Count == 0)
            //    return;
          
            UpdateLogsEvent?.Invoke(CurLogList);

        }


    }
}


