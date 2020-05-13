using NLog;
using UnityEngine;
/// <summary>
/// 版本号工具
/// </summary>
public class VersionUtils
{
    /// <summary>
    /// 版本号比较
    /// </summary>
    /// <param name="ver1"></param>
    /// <param name="ver2"></param>
    /// <returns>返回0相等，返回-1 ver1>ver2 返回1 ver1<ver2 </returns>
    public static int CompareVersion(string ver1, string ver2)
    {
        int ver1_0 = 0;

        int ver1_1 = 0;

        int ver1_2 = 0;

        int ver2_0 = 0;

        int ver2_1 = 0;

        int ver2_2 = 0;

        GetIntVersion(ver1, out ver1_0, out ver1_1, out ver1_2);

        GetIntVersion(ver2, out ver2_0, out ver2_1, out ver2_2);
       

        if (ver2_0 > ver1_0)
        {
            return 1;//ver2大
        }
        else if (ver1_0 > ver2_0)
        {
            return -1;//ver1大
        }

        //后面都是ver1_0 == ver2_0的情况

        if (ver2_1 > ver1_1)
        {
            return 1;//ver2大
        }
        else if (ver1_1 > ver2_1)
        {
            return -1;//ver1大
        }

        //后面都是ver1_0 == ver2_0   ver1_1 == ver2_1

        if (ver2_2 > ver1_2)
        {
            return 1;//ver2大
        }
        else if (ver1_2 > ver2_2)
        {
            return -1;//ver1大
        }

        return 0; //相等  

    }

    /// <summary>
    /// 吧字符串version转换成int
    /// </summary>
    /// <param name="version"></param>
    /// <param name="ver0"></param>
    /// <param name="ver1"></param>
    /// <param name="ver2"></param>
    /// <returns>是否返回成功</returns>
    public static bool GetIntVersion(string version, out int ver0, out int ver1, out int ver2)
    {
        var verSplit = version.Split('.');

        ver0 = 0;
        ver1 = 0;
        ver2 = 0;

        if (verSplit.Length != 3)
        {
            LogMgr.E("VersionUtils.CompareVersion ver1:" + ver1 + " split len:" + verSplit.Length);
            return false;
        }

        if (!int.TryParse(verSplit[0], out ver0))
            return false;

        if (!int.TryParse(verSplit[1], out ver1))
            return false;

        if (!int.TryParse(verSplit[2], out ver2))
            return false;

        return true;     

    }
}
