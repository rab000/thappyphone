using System.Text;
using UnityEngine;
/// <summary>
/// 常用类型转换工具
/// </summary>
public class TypeConvertUtils
{

    public static string Bytes2String(byte[] bs)
    {
        return System.Text.Encoding.UTF8.GetString(bs);
    }

    public static int String2Int32(string s)
    {
        int _i = 0;

        bool b = int.TryParse(s, out _i);
        if (b)
        {
            return _i;
        } 
        else
        {
            Debug.LogError("TypeConvertUtils.String2Int32 字符串转int失败 string="+s);
            return -99999;
        }
        
        //其他字符串转int方法
        //1  (int)string
        //2  Convert.ToInt32(string)
    }

}
