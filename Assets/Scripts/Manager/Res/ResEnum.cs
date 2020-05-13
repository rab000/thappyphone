using UnityEngine;
using System.Text;
using NLog;
/// <summary>
/// 资源管理常量
/// </summary>
public class ResEnum{

    /// <summary>
    /// 资源加载位置
    /// </summary>
    public enum ResLoadURL
    {
        resources,
        streamingAssets,
        persistent,
    }

	public static string ResVer = "1.0.0";

	#region 资源读取存储路径

	//资源(配表)路径
	public static string StreamingAssetPath;

	//非www加载路径
	public static string PersistentPath;

	//www加载专用
	public static string PersistentPath4WWW;

	//资源的类型决定资源的位置
	public static string URL_RES = "/res";//资源根路径

	public static string SCENE ="/scn";//场景配表专用路径
	public static string MAP ="/map";//共有
	public static string ROLE ="/role";//角色资源路径
	public static string UI ="/ui";//ui资源路径
	public static string EFFECT ="/effect";//特效资源路径
	public static string SOUND ="/sound";//声音资源路径

	public static string TABLE = "/table";//配表根路径
	public static string SYSTEM ="/system";//系统配表路径

	public static void InitResConfig()
	{
#if UNITY_ANDROID
		StreamingAssetPath = Application.streamingAssetsPath;
		PersistentPath = Application.persistentDataPath;
		PersistentPath4WWW = Application.persistentDataPath;//未测试

#elif UNITY_IOS
		//注意ios，mac上需要加file:///
		StreamingAssetPath = "file:///" + Application.streamingAssetsPath;
		PersistentPath = Application.persistentDataPath;
		PersistentPath4WWW = "file///"+Application.persistentDataPath;

#elif UNITY_EDITOR_OSX
        StreamingAssetPath = "file:///"+Application.streamingAssetsPath;
		PersistentPath = Application.persistentDataPath;
		PersistentPath4WWW = "file:///"+Application.persistentDataPath;

#elif UNITY_EDITOR_WIN
		//win下是否有file:///都能加载
		StreamingAssetPath = "file:///"+Application.streamingAssetsPath;
		PersistentPath = Application.persistentDataPath;
		PersistentPath4WWW = "file:///"+Application.persistentDataPath;

#endif

        LogMgr.I("读取资源版本Ver:"+ResVer);
	}
	#endregion

	#region 原始配表资源resID

	//NINFO 目前所有ResID都是相对路径，相对的是根目录(StreamingAssets)
	public static string TABLE_SKILL_RESID = TABLE + "/game/skill.gdata";

	#endregion


	//xxx nres 这里要检查路径是否正确,这里对错还要取决于neditor中的临时数据表
	/// <summary>
	/// 通过resid的相对地址路径获取绝对地址路径
	/// 修改int resID到string型的临时函数
	/// </summary>
	/// <returns>The URL by res I.</returns>
	/// <param name="resid">Resid.</param>
	public static string GetUrlByResID(string resid)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(StreamingAssetPath);
		sb.Append("/");
		sb.Append(resid);
		return sb.ToString ();
	}
	
	#region 资源载入状态常量
	public enum ResLoadingStepEnum{
		Wait = 0,//等待加载
		Loading = 1,//从apk，cache，romate到内存的过程
		//Jiexi,//从www->obj,bytes,text的过程,加载完毕就直接解析了，不需要单独的状态
		Loadover = 2,//加载完毕
		Interrapt = 3,//被打断
		Error = 4,//加载失败		
		Null = 7,//空状态
	}
	#endregion

	#region 资源释放类型
	public enum ResUnloadTypeEnum{
		RefCount=0,//引用为0时立即释放(依赖资源)
		Delay=1,//延迟释放
		Never=2,//永不释放(手动释放，用于处理需要反复加载的资源)
	}
    #endregion

    #region 资源打包类型
    //这个类型可以直接从xml中判断出来
    public enum RES_PACK_TYPE
    {
        Single,
        Mult,
        Depend
    }
    #endregion


    #region 资源载入优先级
    //数值越大优先级越高
    public const byte Load_Priority_Normarl = 0;//非依赖资源
	//public const byte Load_Priority_Dependent = Load_Priority_Normarl+1;//依赖资源
	#endregion




}
