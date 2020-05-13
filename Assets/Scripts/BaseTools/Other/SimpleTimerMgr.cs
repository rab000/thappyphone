using UnityEngine;
using System.Collections;
/// <summary>
/// 简易计时器
/// 要确保任务执行过程中behaviour不会被销毁掉，否则协程就挂了
/// 这个计时器没有暂停功能，那么跨场景状态时就可能会null指针
/// </summary>
public class SimpleTimerMgr{
	
    private static MonoBehaviour behaviour;

	public static Coroutine Schedule(MonoBehaviour _behaviour, float delay, Listener task)
    {
        behaviour = _behaviour;
        Coroutine coroutine = behaviour.StartCoroutine(DoTask(task, delay));
        return coroutine;
    }

	private static IEnumerator DoTask(Listener task, float delay)
    {
        yield return new WaitForSeconds(delay);
        task();
    }

}
