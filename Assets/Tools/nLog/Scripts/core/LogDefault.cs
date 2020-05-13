
namespace NLog
{
    public class LogDefault
    {
        //(真实设备)最大显示条数
        public const int MAX_LEN4_SCREEN = 102400;

        //全部模块，如果tag是all就不进行模块检测
        public const string TAG_NAME_ALL = "all";
        //默认模块名
        public const string TAG_NAME_DEFALUT = "default";
        //unity模块名
        public const string TAG_NAME_UNITY = "unity";

        //level 只显示i
        public const string LEVEL_INFO = "i";
        //level 只显示e
        public const string LEVEL_ERROR = "e";
        //level 都不显示
        public const string LEVEL_NOTHING = "n";
        //level 都显示
        public const string LEVEL_ALL = "a";

        //默认正则nil，如果是nil就不进行任何检测
        public const string REGEX_DEFAULT = "nil";
    }

}

