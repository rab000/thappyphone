using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于存放游戏配置
/// 配置一般从外部文本读取
/// </summary>
public class GameConfig
{

    private static string ConfigTxtURL = "";

    /// <summary>
    /// 输入方式，影响输入方式
    /// 键盘鼠标或者触控操作
    /// </summary>
    public enum InputCtrlTypeEnum
    {
        mobile,
        pc
    }

    /// <summary>
    /// 输入方式，影响输入方式
    /// 键盘鼠标或者触控操作
    /// </summary>
    public static InputCtrlTypeEnum InputCtrlType = InputCtrlTypeEnum.mobile;

    //导入并解析配置文件
    public static void LoadConfig()
    {

#if UNITY_ANDROID
		ConfigTxtURL = Application.streamingAssetsPath+"/config.txt";
#elif UNITY_IOS
		ConfigTxtURL = "file:///" + Application.streamingAssetsPath+"/config.txt";		
#elif UNITY_EDITOR_OSX
        ConfigTxtURL = "file:///"+Application.streamingAssetsPath+"/config.txt";
#elif UNITY_EDITOR_WIN
        ConfigTxtURL = "file:///" + Application.streamingAssetsPath + "/config.txt";       
#endif

        FileHelper.GetIns().ReadStringFromApkFile(ConfigTxtURL,

        (string content)=> 
        {
            Dictionary<string, string> configDic = TxtConfig.Load(content);

            Process(configDic);
        },

        (string error)=> 
        {
            Debug.LogError("GameConfig.LoadConfig 读取配置文件失败！");
        }
        
        );

    }

    //解析配置文件
    private static void Process(Dictionary<string, string> configDic)
    {
        string InputCtrlType_string = configDic["InputCtrlType"];

        switch (InputCtrlType_string)
        {
            case "mobile":
                InputCtrlType = InputCtrlTypeEnum.mobile;
                break;
            case "pc":
                InputCtrlType = InputCtrlTypeEnum.pc;
                break;
        }
    }

}
