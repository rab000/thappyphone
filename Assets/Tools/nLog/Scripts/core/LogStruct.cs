namespace NLog
{
    public struct LogStruct
    {
        // HH:mm:ss        
        public string Time;

        public UnityEngine.LogType Type;

        public string Msg;

        public string Tag;

        public string StackTrace;

        public LogColorEnum LogColor;

    }

    public enum LogColorEnum
    {
        red,
        white,
        green,
        blue,
        yellow,
        magenta,//洋红
        cyan//青
    }

}

