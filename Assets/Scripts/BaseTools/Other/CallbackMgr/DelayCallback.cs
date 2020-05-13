using System.Collections;

/// <summary>
/// 延迟一帧执行事件
/// 为go.addComp
/// comp.dosomthing准备
/// 保证comp执行完start才执行后续的dosomething
/// </summary>
public class DelayCallback : AbsCallbackAction
{
    public Listener DelayCb;

    public override void Start()
    {
        base.Start();

        ThreadManager.GetIns().StartCoroutine("DelayAction");
    }

    IEnumerator DelayAction()
    {
        yield return 0;

        DelayCb?.Invoke();
    }


}
