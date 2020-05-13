using UnityEngine;
using System.Collections;
using NLog;

/// <summary>
/// Nafio 组件事件
/// </summary>
public class EventDispather
{
	//public delegate void EventDelegate (string eventName,System.Object sender);

	public event EventDelegate eventListener;

	public void dispatchEvent (byte eventID, byte[] data=null){

		if (eventListener != null) {

			eventListener (eventID, data);
		} else {
            LogMgr.E(string.Format("eventListener {0} is null",eventID));
		}
	}	
}

public delegate void EventDelegate (byte eventID,byte[] sender);

