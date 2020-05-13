//using UnityEngine;
//using System.Collections;


//public class HUDFPS : MonoBehaviour
//{

//    // Attach this to a GUIText to make a frames/second indicator.
//    //
//    // It calculates frames/second over each updateInterval,
//    // so the display does not keep changing wildly.
//    //
//    // It is also fairly accurate at very low FPS counts (<10).
//    // We do this not by simply counting frames per interval, but
//    // by accumulating FPS for each frame. This way we end up with
//    // correct overall FPS even if the interval renders something like
//    // 5.5 frames.

//    public float updateInterval = 0.5F;

//    private float accum = 0; // FPS accumulated over the interval
//    private int frames = 0; // Frames drawn over the interval
//    private float timeleft; // Left time for current interval
	
////	private float fpsSum;			//一段时间内fps总和
////	private int computationTime = 5 * 60;//间隔多长时间计算一次FPS总和
////	private int fpsCount;
//    public bool ShowMemory = false;
//    void Start()
//    {
//        if (!GetComponent<GUIText>())
//        {
//            //TDebug.Log("UtilityFramesPerSecond needs a GUIText component!");
//            enabled = false;
//            return;
//        }
//        timeleft = updateInterval;
//    }
//	public static HUDFPS GetInst()
//	{
//		return inst;
//	}
//	static HUDFPS inst;
//    GUIStyle ss;
//    GUIStyle ss1;
//    string memoryStr = "";
//	void Awake()
//	{
//		if(inst!=null)
//		{
//			//TDebug.LogError("HUDFPS重复加载了");
//			return;
//		}
//        ss = new GUIStyle();
//        ss.fontSize = 30;
//        ss.normal.textColor = new Color(255,255,255);

//        ss1 = new GUIStyle();
//        ss1.fontSize = 18;
//        ss1.normal.textColor = new Color(255, 255, 255); 
//		inst = this;
////		fpsSum = 0;
////		fpsCount = 0;
//	}
//	public bool bShowUI = false;
//	void OnGUI()
//	{
//		if(bShowUI)
//		{
//            GUI.Label(new Rect((Screen.width - 200), 2, 150, 20), GetComponent<GUIText>().text);
//		}
//        //GUI.Label(new Rect((200), 2, 150, 20), currCount + "/50" , ss);
//        if(ShowMemory)
//        {
//            //下面是游戏内存状况显示
//            GUI.Label(new Rect(0, 0, 300, 300), memoryStr.ToString(), ss1);
//        }

//	}
//	//TODO fps给个初始值要不会导致TGameState中GetFpsSumTimes方法里sumFpsNumber += HUDFPS.GetInst().fps 这句报空
//	public float fps = 0;
//    public static int currCount = 0;
//    void Update()
//    {
//		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F))
//		{
//			bShowUI = !bShowUI ;                                    //显示GUI
//		}
//        timeleft -= Time.deltaTime;
//        accum += Time.timeScale / Time.deltaTime;
//        ++frames;

//        // Interval ended - update GUI text and start new interval
//        if (timeleft <= 0.0)
//        {
//            // display two fractional digits (f2 format)
//            fps = accum / frames;
//            string format =((int)(fps)).ToString();// , fps);
//			format+=" FPS";
//#if UNITY_IPHONE&&!UNITY_EDITOR

//            guiText.text = ((int)CatchCrash.getMemory()*100)/100 + "M " + ((int)CatchCrash.getUseMemory()*100)/100 + "M " + format;
//#else
//            //guiText.text = System.GC.GetTotalMemory(false)/1024/1024 + "M " + Profiler.usedHeapSize/1024/1024 + "M " + format;
//            GetComponent<GUIText>().text = string.Format("{0}M {1}M {2}", UnityEngine.Profiling.Profiler.GetMonoUsedSize() / 1024 / 1024, UnityEngine.Profiling.Profiler.usedHeapSize / 1024 / 1024, format);
//#endif
//            //guiText.text = Profiler.usedHeapSize / 1024 / 1024 + "M " + format;
//            if (fps < 30)
//                GetComponent<GUIText>().material.color = Color.yellow;
//            else
//                if (fps < 10)
//                    GetComponent<GUIText>().material.color = Color.red;
//                else
//                    GetComponent<GUIText>().material.color = Color.green;
//            //  DebugConsole.Log(format,level);
//            timeleft = updateInterval;
//            accum = 0.0F;
//            frames = 0;
//        }
//        if (ShowMemory)
//        {
//            //下面是游戏内存状况计算
//            UpdateMemory();
//        }
//    }

//    void UpdateMemory()
//    {
//        memoryStr = "";
//        object[] array = Resources.FindObjectsOfTypeAll(typeof(Texture));
//        double num13 = 0.0;
//        object[] array2 = array;
//        for (int i = 0; i < array2.Length; i++)
//        {
//            Texture texture = (Texture)array2[i];
//            int runtimeMemorySize = UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(texture);
//            num13 += (double)runtimeMemorySize;
//            //if (runtimeMemorySize / 1024 / 1024 >= 1)
//            //{
//            //    Debug.Log(string.Concat(new object[]
//            //        {
//            //            "Texture object ",
//            //            texture.name,
//            //            " using: ",
//            //            runtimeMemorySize / 1024 / 1024,
//            //            "M"
//            //        }));
//            //}
//        }
//        memoryStr += "TotalTextureSize:" + num13 / 1024.0 / 1024.0 + "M\n";
//        //Debug.Log("TotalTextureSize:" + num13 / 1024.0 / 1024.0);
//        object[] array3 = Resources.FindObjectsOfTypeAll<Mesh>();
//        double num14 = 0.0;
//        object[] array4 = array3;
//        for (int j = 0; j < array4.Length; j++)
//        {
//            Mesh o = (Mesh)array4[j];
//            num14 += (double)UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(o);
//        }
//        //Debug.Log("totalMeshSize:" + num14 / 1024.0 / 1024.0);
//        memoryStr += "totalMeshSize:" + num14 / 1024.0 / 1024.0 + "M\n";
//        object[] array5 = Resources.FindObjectsOfTypeAll<AudioClip>();
//        double num15 = 0.0;
//        object[] array6 = array5;
//        for (int k = 0; k < array6.Length; k++)
//        {
//            AudioClip o2 = (AudioClip)array6[k];
//            num15 += (double)UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(o2);
//        }
//        //Debug.Log("totalAudioSize:" + num15 / 1024.0 / 1024.0);
//        memoryStr += "totalAudioSize:" + num15 / 1024.0 / 1024.0 + "M\n";

//        object[] array7 = Resources.FindObjectsOfTypeAll<Animation>();
//        double num16 = 0.0;
//        object[] array8 = array7;
//        for (int l = 0; l < array8.Length; l++)
//        {
//            Animation o3 = (Animation)array8[l];
//            num16 += (double)UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(o3);
//        }
//        //Debug.Log("totalAnimationsSize:" + num16 / 1024.0 / 1024.0);
//        memoryStr += "totalAnimationsSize:" + num16 / 1024.0 / 1024.0 + "M\n";

//        object[] array9 = Resources.FindObjectsOfTypeAll<Material>();
//        double num17 = 0.0;
//        object[] array10 = array9;
//        for (int m = 0; m < array10.Length; m++)
//        {
//            Material o4 = (Material)array10[m];
//            num17 += (double)UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(o4);
//        }
//        //Debug.Log("totalMaterialSize:" + num17 / 1024.0 / 1024.0);
//        memoryStr += "totalMaterialSize:" + num17 / 1024.0 / 1024.0 + "M\n";

//        //GC.Collect();

//        memoryStr += "Mono used size" + UnityEngine.Profiling.Profiler.GetMonoUsedSize() / 1024f / 1024f + "M\n";
//        //UnityEngine.Debug.Log("Mono used size" + Profiler.GetMonoUsedSize() / 1024f / 1024f + "M");
//        memoryStr += "Allocated Mono heap size" + UnityEngine.Profiling.Profiler.GetMonoHeapSize() / 1024f / 1024f + "M\n";
//        //UnityEngine.Debug.Log("Allocated Mono heap size" + Profiler.GetMonoHeapSize() / 1024f / 1024f + "M");
//    }

//}