using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;

using System.Text;
using System.Text.RegularExpressions;

public class StringUtils
{
    public static byte[] StringToByte(string _str)
    {
        return System.Text.Encoding.UTF8.GetBytes(_str);
    }

    /// <summary>
    /// 将传入字符串以GZip算法压缩后，返回Base64编码字符
    /// </summary>
    /// <param name="rawString">需要压缩的字符串</param>
    /// <returns>压缩后的Base64编码的字符串</returns>
    public static string GZipCompressString(string rawString)
    {
        if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
        {
            return "";
        }
        else
        {
            byte[] rawData = StringToByte(rawString);
            byte[] zippedData = GZipCompress(rawData);
            return (string)(Convert.ToBase64String(zippedData));
        }
    }
    /// <summary>
    /// GZip压缩
    /// </summary>
    /// <param name="rawData"></param>
    /// <returns></returns>
    public static byte[] GZipCompress(byte[] rawData)
    {
        MemoryStream ms = new MemoryStream();
        GZipStream gzs = new GZipStream(ms, CompressionMode.Compress, true);
        gzs.Write(rawData, 0, rawData.Length);
        gzs.Close();
        return ms.ToArray();
        //MemoryStream ms = new MemoryStream();
        //GZipOutputStream gzip = new GZipOutputStream(ms);
        //gzip.Write(rawData, 0, rawData.Length);
        //gzip.Close();
        //return ms.ToArray();
    }
    /// <summary>
    /// 将传入的二进制字符串资料以GZip算法解压缩
    /// </summary>
    /// <param name="zippedString">经GZip压缩后的二进制字符串</param>
    /// <returns>原始未压缩字符串</returns>
    public static string GZipDecompressString(string zippedString)
    {
        if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
        {
            return "";
        }
        else
        {
            try
            {
                byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
                return (string)(System.Text.Encoding.UTF8.GetString(GZipDecompress(zippedData)));
            }
            catch
            {
                return "";
            }
        }
    }
    /// <summary>
    /// GZip解压
    /// </summary>
    /// <param name="zippedData"></param>
    /// <returns></returns>
    public static byte[] GZipDecompress(byte[] zippedData)
    {
        MemoryStream ms = new MemoryStream(zippedData);
        GZipStream gZipStream = new GZipStream(ms, CompressionMode.Decompress);
        MemoryStream outBuffer = new MemoryStream();
        byte[] block = new byte[1024];
        while (true)
        {
            int bytesRead = gZipStream.Read(block, 0, block.Length);
            if (bytesRead <= 0)
                break;
            else
                outBuffer.Write(block, 0, bytesRead);
        }
        gZipStream.Close();
        return outBuffer.ToArray();
        //MemoryStream ms = new MemoryStream(zippedData);
        //GZipInputStream gzip = new GZipInputStream(ms);
        //MemoryStream outBuffer = new MemoryStream();
        //int count = 0;
        //byte[] data = new byte[4096];
        //while ((count = gzip.Read(data, 0, data.Length)) != 0)
        //{
        //    outBuffer.Write(data, 0, count);
        //}
        //gzip.Close();
        //return outBuffer.ToArray();
    }


    /// <summary>
    /// 将传入字符串以BZip2算法压缩后，返回Base64编码字符
    /// </summary>
    /// <param name="rawString">需要压缩的字符串</param>
    /// <returns>压缩后的Base64编码的字符串</returns>
    public static string BZip2CompressString(string rawString)
    {
        if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
        {
            return "";
        }
        else
        {
            byte[] rawData = StringToByte(rawString);
            byte[] zippedData = BZip2Compress(rawData);
            return (string)(Convert.ToBase64String(zippedData));
        }
    }
    /// <summary>
    /// BZip2压缩
    /// </summary>
    /// <param name="rawData"></param>
    /// <returns></returns>
    public static byte[] BZip2Compress(byte[] rawData)
    {
        MemoryStream ms = new MemoryStream();
        BZip2OutputStream BZip2 = new BZip2OutputStream(ms);
        BZip2.Write(rawData, 0, rawData.Length);
        BZip2.Close();
        return ms.ToArray();
    }
    /// <summary>
    /// 将传入的二进制字符串资料以BZip2算法解压缩
    /// </summary>
    /// <param name="zippedString">经BZip2压缩后的二进制字符串</param>
    /// <returns>原始未压缩字符串</returns>
    public static string BZip2DecompressString(string zippedString)
    {
        if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
        {
            return "";
        }
        else
        {
            byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
            return (string)(System.Text.Encoding.UTF8.GetString(BZip2Decompress(zippedData)));
        }
    }
    /// <summary>
    /// BZip2解压
    /// </summary>
    /// <param name="zippedData"></param>
    /// <returns></returns>
    public static byte[] BZip2Decompress(byte[] zippedData)
    {
        MemoryStream ms = new MemoryStream(zippedData);
        BZip2InputStream BZip2 = new BZip2InputStream(ms);
        MemoryStream outBuffer = new MemoryStream();
        int count = 0;
        byte[] data = new byte[4096];
        while ((count = BZip2.Read(data, 0, data.Length)) != 0)
        {
            outBuffer.Write(data, 0, count);
        }
        BZip2.Close();
        return outBuffer.ToArray();
    }


    /// <summary>
    /// 将传入字符串以Zip算法压缩后，返回Base64编码字符
    /// </summary>
    /// <param name="rawString">需要压缩的字符串</param>
    /// <returns>压缩后的Base64编码的字符串</returns>
    public static string ZipCompressString(string rawString)
    {
        if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
        {
            return "";
        }
        else
        {
            byte[] rawData = StringToByte(rawString);
            byte[] zippedData = ZipCompress(rawData);
            return (string)(Convert.ToBase64String(zippedData));
        }
    }
    /// <summary>
    /// Zip压缩
    /// </summary>
    /// <param name="rawData"></param>
    /// <returns></returns>
    public static byte[] ZipCompress(byte[] rawData)
    {
        MemoryStream ms = new MemoryStream();
        ZipOutputStream zip = new ZipOutputStream(ms);
        ZipEntry _zipEntry = new ZipEntry("CLO");
        zip.PutNextEntry(_zipEntry);
        zip.Write(rawData, 0, rawData.Length);
        zip.CloseEntry();
        zip.Close();
        return ms.ToArray();
    }
    /// <summary>
    /// 将传入的二进制字符串资料以Zip算法解压缩
    /// </summary>
    /// <param name="zippedString">经Zip压缩后的二进制字符串</param>
    /// <returns>原始未压缩字符串</returns>
    public static string ZipDecompressString(string zippedString)
    {
        if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
        {
            return "";
        }
        else
        {
            byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
            return (string)(System.Text.Encoding.UTF8.GetString(ZipDecompress(zippedData)));
        }
    }
    /// <summary>
    /// ZIP解压
    /// </summary>
    /// <param name="zippedData"></param>
    /// <returns></returns>
    public static byte[] ZipDecompress(byte[] zippedData)
    {
        MemoryStream ms = new MemoryStream(zippedData);
        ZipInputStream zip = new ZipInputStream(ms);
        MemoryStream outBuffer = new MemoryStream();
        zip.GetNextEntry();
        //ZipEntry _zipEntry;
        //while ((_zipEntry = zip.GetNextEntry()) != null)
        //{
        //    //string str = _zipEntry.Name;
        //}
        int count = 0;
        byte[] data = new byte[4096];
        while ((count = zip.Read(data, 0, data.Length)) != 0)
        {
            outBuffer.Write(data, 0, count);
        }
        //}
        zip.Close();
        return outBuffer.ToArray();
    }


    #region 可逆加密:Simple(3)、Base64(8)、DES(12)
    public static string SimpleEncode(string str)
    {
        string htext = "";

        for (int i = 0; i < str.Length; i++)
        {
            htext = htext + (char)(str[i] + 10 - 1 * 2);
        }
        return htext;
    }
    public static string SimpleDecode(string str)
    {
        string dtext = "";

        for (int i = 0; i < str.Length; i++)
        {
            dtext = dtext + (char)(str[i] - 10 + 1 * 2);
        }
        return dtext;
    }

    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="input">需要加密的字符串</param>
    /// <returns></returns>
    public static string Base64Encrypt(string input)
    {
        return Base64Encrypt(input, new UTF8Encoding());
    }
    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="input">需要加密的字符串</param>
    /// <param name="encode">字符编码</param>
    /// <returns></returns>
    public static string Base64Encrypt(string input, Encoding encode)
    {
        return Convert.ToBase64String(encode.GetBytes(input));
    }
    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="input">需要解密的字符串</param>
    /// <returns></returns>
    public static string Base64Decrypt(string input)
    {
        return Base64Decrypt(input, new UTF8Encoding());
    }
    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="input">需要解密的字符串</param>
    /// <param name="encode">字符的编码</param>
    /// <returns></returns>
    public static string Base64Decrypt(string input, Encoding encode)
    {
        return encode.GetString(Convert.FromBase64String(input));
    }

    const string KEY_64 = "Unity3D.";
    const string IV_64 = "Unity3D.";
    /// <summary>
    /// DES加密
    /// </summary>
    /// <param name="data">加密数据</param>
    /// <param name="key">8位字符的密钥字符串</param>
    /// <param name="iv">8位字符的初始化向量字符串</param>
    /// <returns></returns>
    public static string DESEncode(string data, string key = KEY_64, string iv = IV_64)
    {
        byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
        byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream();
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

        StreamWriter sw = new StreamWriter(cst);
        sw.Write(data);
        sw.Flush();
        cst.FlushFinalBlock();
        sw.Flush();
        return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
    }
    /// <summary>
    /// DES解密
    /// </summary>
    /// <param name="data">解密数据</param>
    /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param>
    /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
    /// <returns></returns>
    public static string DESDecode(string data, string key = KEY_64, string iv = IV_64)
    {
        byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
        byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

        byte[] byEnc;
        try
        {
            byEnc = Convert.FromBase64String(data);
        }
        catch
        {
            return null;
        }

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream(byEnc);
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cst);
        return sr.ReadToEnd();
    }
    #endregion

    #region 不可逆加密:MD5
    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="input">需要加密的字符串</param>
    /// <returns></returns>
    public static string MD5Encrypt(string input)
    {
        return MD5Encrypt(input, new UTF8Encoding());
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="input">需要加密的字符串</param>
    /// <param name="encode">字符的编码</param>
    /// <returns></returns>
    public static string MD5Encrypt(string input, Encoding encode)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] t = md5.ComputeHash(encode.GetBytes(input));
        StringBuilder sb = new StringBuilder(32);
        for (int i = 0; i < t.Length; i++)
            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
        return sb.ToString();
    }
    #endregion

    #region 格式检测

    //邮件检测
    public static bool CheckEmailAddress(string emailAddress)
    {
        Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
        if (!r.IsMatch(emailAddress))
        {

            return false;
        }

        return true;
    }

    //密码格式检测
    public static string CheckPassword(string pass)
    {
        Regex pas6 = new Regex("^.{6,}$");
        if (!pas6.IsMatch(pass))
        {
            return "Password at least 6 characters";
        }
        Regex pas16 = new Regex("^.{6,16}$");
        if (!pas16.IsMatch(pass))
        {
            return "Password at most 16 characters";
        }

        // 同时包含数字字母  可包含其他字符
        Regex pas = new Regex("^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z#@!~%^&*]{6,16}$");

        if (!pas.IsMatch(pass))
        {
            return "Password must contain letters, numbers";
        }
        return "ok";//正确返回
    }
    #endregion

}
