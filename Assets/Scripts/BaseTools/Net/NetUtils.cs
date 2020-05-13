
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;

public class NetUtils : MonoBehaviour
{
    //计算md5
    public static string Md5Sum(string strToEncrypt)
    {
        byte[] bs = UTF8Encoding.UTF8.GetBytes(strToEncrypt);
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();

        byte[] hashBytes = md5.ComputeHash(bs);

        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');
    }

    /// <summary>
    /// 网络状态
    /// </summary>
    public enum ENetState
    {
        /// <summary>
        /// 无网络
        /// </summary>
        none,
        /// <summary>
        /// 移动网络
        /// </summary>
        mobile,
        /// <summary>
        /// wifi网络
        /// </summary>
        wifi,
    }
    /// <summary>
    /// 获取当前网络状态
    /// </summary>
    /// <returns>网络状态枚举</returns>
    public static ENetState GetNetState()
    {
        ENetState ens = ENetState.none;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ens = ENetState.none;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            ens = ENetState.mobile;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            ens = ENetState.wifi;
        }
        return ens;
    }

    /// <summary>
    /// 获取本机ipv4
    /// </summary>
    /// <returns></returns>
    public static string GetLocalIPv4()
    {
        try
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress item in IpEntry.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    return item.ToString();
                }
            }
            return "";
        }
        catch { return ""; }
    }

    public static string GetLocalIPv6()
    {
        try
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress item in IpEntry.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    return item.ToString();
                }
            }
            return "";
        }
        catch { return ""; }
    }
}
