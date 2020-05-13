using UnityEngine;

public static class ScreenOrientationHelper
{

    public enum NScreenOrientation
    {
        LandscapeLeft,
        LandscapeRight,
        Portrait,
        PortraitUpsideDown,
        AutoRotation,
        LandspaceAutoRotation,//横屏两方向切换
        PortraitAutoRotation//竖屏两方向切换
    }

    static void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    public static void SetOrientation(NScreenOrientation type) 
    {
        switch (type) 
        {
            case NScreenOrientation.LandscapeLeft:
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
            case NScreenOrientation.LandscapeRight:
                Screen.orientation = ScreenOrientation.LandscapeRight;
                break;
            case NScreenOrientation.Portrait:
                Screen.orientation = ScreenOrientation.Portrait;
                break;
            case NScreenOrientation.PortraitUpsideDown:
                Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                break;
            case NScreenOrientation.AutoRotation:
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                break;
            case NScreenOrientation.LandspaceAutoRotation:
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                break;
            case NScreenOrientation.PortraitAutoRotation:
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                break;
        }  

    }



}
