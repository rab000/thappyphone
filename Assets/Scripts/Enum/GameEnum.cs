using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class GameEnum
{
    public static StringBuilder SB1 = new StringBuilder();

    public static StringBuilder SB2 = new StringBuilder();

    public enum PlatformEnum
    {
        win,
        mac,
        android,
        ios,
        linux,
    }

#if UNITY_STANDALONE_WIN
    public static PlatformEnum CurPlatform = PlatformEnum.win;
#elif UNITY_IOS
    public static PlatformEnum CurPlatform = PlatformEnum.ios;
#elif UNITY_ANDROID
    public static PlatformEnum CurPlatform = PlatformEnum.android;
#elif UNITY_STANDALONE_OSX
    public static PlatformEnum CurPlatform = PlatformEnum.mac;
#elif UNITY_STANDALONE_LINUX
    public static PlatformEnum CurPlatform = PlatformEnum.linux;
#endif


    public static string StreamPath
    {
        get
        {
#if UNITY_ANDROID
            return Application.streamingAssetsPath;
#elif UNITY_IOS
		//注意ios，mac上需要加file:///
		return "file:///" + Application.streamingAssetsPath;		
#elif UNITY_EDITOR_OSX
        return "file:///"+Application.streamingAssetsPath;		
#elif UNITY_EDITOR_WIN
		//win下是否有file:///都能加载
		return "file:///"+Application.streamingAssetsPath;
#endif
        }
    }

    public static string PersistPath
    {
        get
        {
#if UNITY_ANDROID
            return Application.persistentDataPath;
#elif UNITY_IOS
		//注意ios，mac上需要加file:///
		return "file///"+Application.persistentDataPath;		
#elif UNITY_EDITOR_OSX
        return "file:///"+Application.persistentDataPath;	
#elif UNITY_EDITOR_WIN
            //win下是否有file:///都能加载
            return "file:///" + Application.persistentDataPath;
#endif
        }
    }


}
