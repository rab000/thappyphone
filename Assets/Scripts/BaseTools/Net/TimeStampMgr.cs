using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

public class TimeStampMgr : Singleton<TimeStampMgr>
{
    /// <summary>
    /// 是否已同步服务器时间
    /// </summary>
    private bool m_IsServerTimeHasSync = false;

    /// <summary>
    /// 服务器时间戳(单位毫秒)
    /// </summary>
    private long m_ServerTimestamp;

    /// <summary>
    /// 当前与服务器时间偏移量(单位毫秒)
    /// </summary>
    private long m_ServerTimeOffset;

    /// <summary>
    /// 本地时间戳(单位毫秒)
    /// </summary>
    private long m_LocalTimestamp;

    /// <summary>
    /// 本地时间偏移量(单位毫秒)
    /// </summary>
    private long m_LocalTimeOffset;

    /// <summary>
    /// 当前时间戳(如果已同步服务器时间返回服务器时间戳，否则返回本地时间戳 单位秒)
    /// </summary>
    public long Now
    {
        get { return GetServerTimestamp(); }
    }

    public TimeStampMgr()
    {
        m_LocalTimestamp = TimeUtils.DateTimeToUnixTimestamp(DateTime.Now) * 1000L;
        m_LocalTimeOffset = Convert.ToInt64(Time.realtimeSinceStartup * 1000f);
    }

    /// <summary>
    /// 初始化服务器时间
    /// </summary>
    /// <param name="serverTimestamp">服务器时间戳(单位秒)</param>
    public void SetServerTime(long serverTimestamp)
    {
        m_ServerTimestamp = serverTimestamp * 1000L;
        m_ServerTimeOffset = Convert.ToInt64(Time.realtimeSinceStartup * 1000f);
        m_IsServerTimeHasSync = true;
    }

    /// <summary>
    /// 获取服务器时间戳(单位毫秒)
    /// </summary>
    /// <returns>服务器时间戳</returns>
    public long GetServerTimestamp()
    {
        //若未同步服务器时间，返回本地时间戳
        if (!m_IsServerTimeHasSync)
        {
            return GetLocalTimestamp();
        }

        long currentTimestamp = m_ServerTimestamp + Convert.ToInt64(Time.realtimeSinceStartup * 1000f) - m_ServerTimeOffset;
        return currentTimestamp;
    }

    public DateTime GetServerTime()
    {
        long timestamp = GetServerTimestamp();
        return TimeUtils.UnixTimestampToLocalTime(timestamp / 1000L);
    }

    /// <summary>
    /// 获取本队时间戳(单位秒)
    /// </summary>
    /// <returns>本地时间戳</returns>
    public long GetLocalTimestamp()
    {
        long currentTimestamp = m_LocalTimestamp + Convert.ToInt64(Time.realtimeSinceStartup * 1000f) - m_LocalTimeOffset;
        return currentTimestamp;
    }
}

