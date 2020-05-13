using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO;
/// <summary>
/// 线程管理
/// </summary>
public class ThreadManager : MonoBehaviour {
	
	static MonoBehaviour ins;

	void Awake(){ ins = this; }
	
	public static MonoBehaviour GetIns(){return ins;}

	void OnDestroy(){ ins = null; }
	
	new public static void CancelInvoke(){ ins.CancelInvoke(); }
	
	new public static void CancelInvoke(string methodName){ ins.CancelInvoke(methodName); }
	
	new public static void Invoke(string methodName, float time){ ins.Invoke(methodName, time); }
	
	new public static void InvokeRepeating(string methodName, float time, float repeatRate){ ins.InvokeRepeating(methodName, time, repeatRate); }
	
	new public static bool IsInvoking(){ return ins.IsInvoking(); }
	
	new public static bool IsInvoking(string methodName){ return ins.IsInvoking(methodName); }
	
	new public static void print(object message){ print(message); }
	
	new public static Coroutine StartCoroutine(IEnumerator routine){ return ins.StartCoroutine(routine); }
	
	new public static Coroutine StartCoroutine(string methodName){ return ins.StartCoroutine(methodName); }
	
	new public static Coroutine StartCoroutine(string methodName, object value){ return ins.StartCoroutine(methodName, value); }
	
	new public static Coroutine StartCoroutine_Auto(IEnumerator routine){ return ins.StartCoroutine(routine); }
	
	new public static void StopAllCoroutines(){ ins.StopAllCoroutines(); }
	
	new public static void StopCoroutine(string methodName){ ins.StopCoroutine(methodName); }


}
