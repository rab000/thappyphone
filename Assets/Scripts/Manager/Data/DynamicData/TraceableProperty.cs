/// <summary>
/// 用于跟踪调用频繁的运行时变量
/// 变量发生变化时会抛出事件给观察者
/// </summary>
/// <typeparam name="T"></typeparam>
public class TraceableProperty<T>
{
    public delegate void ValueChangedHandler(T oldValue, T newValue);

    public ValueChangedHandler OnValueChanged;

    public string ID;

    private T _value;
    public T Value
    {
        get
        {
            //+log
            return _value;
        }
        set
        {
            if (!Equals(_value, value))
            {
                //+log
                T old = _value;
                _value = value;
                ValueChanged(old, _value);
            }
        }
    }

    private void ValueChanged(T oldValue, T newValue)
    {        
        OnValueChanged?.Invoke(oldValue, newValue);       
    }


}

