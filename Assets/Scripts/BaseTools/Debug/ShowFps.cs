using UnityEngine;

namespace ntools
{
    internal class ShowFps : MonoBehaviour
    {
        private long _mFrameCount = 0;
        private long _mLastFrameTime = 0;
        private long _mLastFps = 0;
        
        public void Update()
        {
            updateTick();
        }
        
        public void OnGUI()
        {
            drawFps();
        }

        private void drawFps()
        {
            GUIStyle bb = new GUIStyle();
            bb.normal.background = null;
            bb.fontSize = 26;
            if (_mLastFps > 50)
            {
                bb.normal.textColor = new Color(0, 1, 0);
            }
            else if (_mLastFps > 40)
            {
                bb.normal.textColor = new Color(1, 1, 0);
            }
            else
            {
                bb.normal.textColor = new Color(1, 0, 0);
            }
            GUI.Label(new Rect(0, Screen.height - 40, 80, 40), "fps: " + _mLastFps, bb);
        }

        private void updateTick()
        {
            _mFrameCount++;
            long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
            if (_mLastFrameTime == 0)
            {
                _mLastFrameTime = TickToMilliSec(System.DateTime.Now.Ticks);
            }

            if ((nCurTime - _mLastFrameTime) >= 1000)
            {
                long fps = (long)(_mFrameCount * 1.0f / ((nCurTime - _mLastFrameTime) / 1000.0f));

                _mLastFps = fps;

                _mFrameCount = 0;

                _mLastFrameTime = nCurTime;
            }
        }
        private static long TickToMilliSec(long tick)
        {
            return tick / (10 * 1000);
        }
    }
}

