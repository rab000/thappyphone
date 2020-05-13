using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ZLog : MonoBehaviour
{
	private bool bPause = false;
	private bool bShowError = true;
	private bool bShowLog = true;
	private bool bShowWarning = false;
	public static bool enable = true;
	public static bool enableConsole = true;
	public static bool enableGUI = false;
	private static int height = 150;
	private static ZLog instance;
	private List<LogInfo> logList = new List<LogInfo>();
	public static int maxLogCount = 50;
	private float scroll = 10f;
	private const float scroll_factor = 10f;
	private static int stackHeight = ((int) (height * 0.3f));
	private static int width = 200;

	private void AddLog(string str, string stack, LogType type)
	{
		if (!this.bPause)
		{
			LogInfo item = new LogInfo {
				type = type,
				str = type.ToString() + ":" + str,
				stack = stack
			};
			this.logList.Add(item);
			if (this.logList.Count > maxLogCount)
			{
				int count = this.logList.Count - maxLogCount;
				this.logList.RemoveRange(0, count);
			}
			this.scroll = this.logList.Count - 0x10;
			if (this.scroll < 0f)
			{
				this.scroll = 0f;
			}
		}
	}

	private void Awake()
	{
		if (instance != null)
		{
			throw new Exception("单例");
		}
		instance = this;
		Application.RegisterLogCallback(new Application.LogCallback(this.AddLog));
	}

	private void DrawWindow(int id)
	{
		GUI.Label(new Rect(2f, 0f, 200f, 20f), this.scroll + "/" + this.logList.Count);
		int num = 210;
		if (GUI.Button(new Rect((float) num, 0f, 40f, 20f), "clear"))
		{
			this.logList.Clear();
		}
		num += 50;
		if (GUI.Button(new Rect((float) num, 0f, 40f, 20f), "pause"))
		{
			this.bPause = !this.bPause;
		}
		num += 50;
		if (GUI.Button(new Rect((float) num, 0f, 40f, 20f), "save"))
		{
			this.SaveLog();
		}
		num += 50;
		this.bShowLog = GUI.Toggle(new Rect((float) num, 0f, 60f, 20f), this.bShowLog, "log");
		num += 70;
		this.bShowWarning = GUI.Toggle(new Rect((float) num, 0f, 60f, 20f), this.bShowWarning, "warnning");
		num += 70;
		this.bShowError = GUI.Toggle(new Rect((float) num, 0f, 60f, 20f), this.bShowError, "error");
		this.scroll = GUI.VerticalSlider(new Rect((float) (width - 20), 0f, 20f, (float) (height - stackHeight)), this.scroll, 0f, (float) (this.logList.Count - 1));
		int scroll = (int) this.scroll;
		int num3 = 30;
		int num4 = ((width / 20) - (stackHeight / 20)) - 2;
		for (int i = scroll; i < Mathf.Min(scroll + num4, this.logList.Count); i++)
		{
			if (this.logList[i].type == LogType.Log)
			{
				if (this.bShowLog)
				{
					GUI.Label(new Rect(2f, (float) (((i - scroll) * 20) + num3), (float) (width - 4), 20f), this.logList[i].str);
				}
			}
			else if (this.logList[i].type == LogType.Error)
			{
				if (this.bShowError)
				{
					GUI.Label(new Rect(2f, (float) (((i - scroll) * 20) + num3), (float) (width - 4), 20f), this.logList[i].str);
				}
			}
			else if (this.logList[i].type == LogType.Warning)
			{
				if (this.bShowWarning)
				{
					GUI.Label(new Rect(2f, (float) ((i - scroll) * 20), (float) (width - 4), 20f), this.logList[i].str);
				}
			}
			else
			{
				GUI.Label(new Rect(2f, (float) (((i - scroll) * 20) + num3), (float) (width - 4), 20f), this.logList[i].str);
			}
		}
		if ((scroll >= 0) && (scroll < this.logList.Count))
		{
			GUI.TextArea(new Rect(0f, (float) (height - stackHeight), (float) width, (float) stackHeight), this.logList[scroll].stack);
		}
	}

	public static void Init(GameObject __gameObject, int __width, int __height)
	{
		if (__gameObject.GetComponent<ZLog>() == null)
		{
			__gameObject.AddComponent<ZLog>();
			width = __width;
			height = __height;
			stackHeight = (int) (height * 0.3f);
		}
	}

	public static void Log(object message)
	{
		if (enable)
		{
			if (enableConsole)
			{
				Debug.Log(message);
			}
			else if (instance != null)
			{
				instance.AddLog(message.ToString(), "查看栈区信息请将enableConsole设置为true", LogType.Log);
			}
		}
	}

	public static void LogError(object message)
	{
		if (enable)
		{
			if (enableConsole)
			{
				Debug.LogError(message);
			}
			else if (instance != null)
			{
				instance.AddLog(message.ToString(), "查看栈区信息请将enableConsole设置为true", LogType.Error);
			}
		}
	}

	public static void LogWarning(object message)
	{
		if (enable)
		{
			if (enableConsole)
			{
				Debug.LogWarning(message);
			}
			else if (instance != null)
			{
				instance.AddLog(message.ToString(), "查看栈区信息请将enableConsole设置为true", LogType.Warning);
			}
		}
	}

	private void OnGUI()
	{
		if (enable && enableGUI)
		{
			GUI.Window(0, new Rect(10f, 0f, (float) width, (float) height), new GUI.WindowFunction(this.DrawWindow), "");
		}
	}

	private void SaveLog()
	{
	}

	private void Update()
	{
		if (enableGUI)
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (!(axis == 0f))
			{
				this.scroll -= axis * 10f;
				this.scroll = Mathf.Clamp(this.scroll, 0f, (float) (this.logList.Count - 1));
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct LogInfo
	{
		public LogType type;
		public string str;
		public string stack;
	}
}
