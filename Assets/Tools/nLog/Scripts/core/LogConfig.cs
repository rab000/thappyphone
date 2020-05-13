using UnityEngine;

namespace NLog
{
    public static class LogConfig
    {
        //输出log的总开关
        public static bool BeShowLog = true;

        //是否输出到屏幕,输出到屏幕就不输出到控制台，两者互斥
        //本地false，移动端true,本地如果用true，则屏幕上会出两条log
        public static bool BeWrite2Screen = false;

        //是否输出到文件，
        //程序可能会崩溃时，保存到文件能保留事故现场，正常调试不需要开这个，这个可以用来调试不易复现的问题
        public static bool BeWrite2File = false;

        //是否开启远程传输
        public static bool BeWirte2Remote = false;

        //当前log等级
        public static string Level = LogDefault.LEVEL_ALL;

        //当前过滤模块
        public static string Tag = LogDefault.TAG_NAME_ALL;

        //用于过滤的regex
        public static string FilterRegex = LogDefault.REGEX_DEFAULT;

        public static string LogFileSavePath = Application.persistentDataPath + "/log.txt";
    }

}

