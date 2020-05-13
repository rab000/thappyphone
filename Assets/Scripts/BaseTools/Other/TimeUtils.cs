using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 621355968000000000说明
/// https://bbs.csdn.net/topics/210074382
/// 时区与DateTime
/// https://www.cnblogs.com/wuxiaoqian726/archive/2011/03/19/1988931.html
/// </summary>
public class TimeUtils
{
    /// <summary>
    /// 毫秒时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetMilliSecTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }
    
    /// <summary>
    /// 秒时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    /// <summary>
    /// 毫秒转DateTime
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(long milliseconds)
    {
        DateTime start = new DateTime(1970, 1, 1);
        DateTime newDateTime = start.AddMilliseconds(milliseconds);
        return newDateTime.ToLocalTime();
    }

    /// <summary>
    /// ToTimeStamp(DateTime.Now)结果与GetTimeStamp一致
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToTimeStamp(DateTime dateTime)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
        long timeStamp = (long)(dateTime.ToLocalTime() - startTime).TotalMilliseconds;
        return timeStamp;
    }

    public static long DateTimeToUnixTimestamp(DateTime dateTime)
    {
        if (dateTime.Kind != DateTimeKind.Utc)
        {
            dateTime = dateTime.ToUniversalTime();
        }
        long timestamp = (dateTime.Ticks - 621355968000000000) / 10000000;
        return timestamp;
    }

    public static DateTime UnixTimestampToLocalTime(long timestamp)
    {
        return new DateTime(timestamp * 10000000 + 621355968000000000, DateTimeKind.Utc).ToLocalTime();
    }

    public static DateTime UnixTimestampToUtcTime(long timestamp)
    {
        return new DateTime(timestamp * 10000000 + 621355968000000000, DateTimeKind.Utc);
    }
}
