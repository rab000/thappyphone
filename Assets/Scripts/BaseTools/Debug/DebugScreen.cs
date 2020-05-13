using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[System.Reflection.Obfuscation(Exclude = true)]
public class DebugScreen : MonoBehaviour
{

	Text uiLabel;
    // Use this for initialization
    void Start()
    {
		uiLabel = transform.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTick();
    }

    void OnGUI()
    {
        
    }

    private void DrawFps()
    {
        Color color;
        if (mLastFps > 50)
        {
            color = new Color(0, 1, 0);
        }
        else if (mLastFps > 40)
        {
            color = new Color(1, 1, 0);
        }
        else
        {
            color = new Color(1.0f, 0, 0);
        }

        uiLabel.color = color;
        uiLabel.text = "fps: " + mLastFps;

    }

    private long mFrameCount = 0;
    private long mLastFrameTime = 0;
    static long mLastFps = 0;
    private void UpdateTick()
    {
        if (true)
        {
            mFrameCount++;
            long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
            if (mLastFrameTime == 0)
            {
                mLastFrameTime = TickToMilliSec(System.DateTime.Now.Ticks);
            }

            if ((nCurTime - mLastFrameTime) >= 1000)
            {
                long fps = (long)(mFrameCount * 1.0f / ((nCurTime - mLastFrameTime) / 1000.0f));

                mLastFps = fps;

                mFrameCount = 0;

                mLastFrameTime = nCurTime;
            }

            DrawFps();
        }
        
    }
    public static long TickToMilliSec(long tick)
    {
        return tick / (10 * 1000);
    }
}