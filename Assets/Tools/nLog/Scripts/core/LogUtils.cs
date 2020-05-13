using System;
using System.Text;
using UnityEngine;
namespace NLog
{
    public static class LogUtils
    {

        public static StringBuilder SB = new StringBuilder();

        public static StringBuilder SB2 = new StringBuilder();

        /// <summary>
        /// 用于log的时间
        /// </summary>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public static string GetCurTime(string timeFormat = "HH:mm:ss")
        {
            return DateTime.Now.ToString(timeFormat);
        }

        /// <summary>
        /// log时间显示格式
        /// </summary>
        /// <returns></returns>
        public static string GetLogFormatTime()
        {
            StringBuilder sb = new StringBuilder();
            DateTime time = DateTime.Now;
            sb.Append(time.Year);
            sb.Append("/");
            sb.Append(time.Month);
            sb.Append("/");
            sb.Append(time.Day);
            sb.Append("-");
            sb.Append(time.Hour);
            sb.Append(":");
            sb.Append(time.Minute);
            sb.Append(":");
            sb.Append(time.Second);
            sb.Append(".");
            sb.Append(time.Millisecond);
            sb.Append("-");
            sb.Append(Time.frameCount % 999);
            sb.Append(":");
            return sb.ToString();
        }

        /// <summary>
        /// 绝对路径转换到Assets内部的相对路径
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static string ChangeToRelativePath(string absolutePath)
        {
            string s = "Assets" + absolutePath.Substring(Application.dataPath.Length);           
            return s; 
        }

        public static Color GetColor(LogColorEnum color)
        {
            Color c = new Color();

            switch (color)
            {
                case LogColorEnum.white:
                    c = Color.white;
                    break;
                case LogColorEnum.red:
                    c = Color.red;
                    break;
                case LogColorEnum.blue:
                    c = Color.blue;
                    break;
                case LogColorEnum.green:
                    c = Color.green;
                    break;
                case LogColorEnum.yellow:
                    c = Color.yellow;
                    break;
                case LogColorEnum.magenta:
                    c = Color.magenta;
                    break;
                case LogColorEnum.cyan:
                    c = Color.cyan;
                    break;
            }

            return c;
        }

        public static void Test()
        {
            string s = string.Format("<i><color='{0}'>{1}</color></i>", "#e114e9", "ss");

            int a = 0;

            string s1 = $"test--->{a}";
        }
        


    }
}

