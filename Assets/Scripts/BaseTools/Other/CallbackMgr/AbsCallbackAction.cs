using System;

public class AbsCallbackAction : ICallbackAction {

    public Listener OnStart;

    public Listener OnEnd;

	public virtual void Start () 
	{
        OnStart?.Invoke();
    }

	public virtual void End () 
	{
        OnEnd?.Invoke();
    }

}
