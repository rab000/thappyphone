using UnityEngine;
/// <summary>
/// 统一处理全局所有键盘鼠标操作
/// 对外部观察者发出对应键盘鼠标事件
/// 
/// 管理鼠标icon
/// 
/// 为什么要有这个类
/// 如果没有，任何功能想监听keycode =a需要再循环中监听
/// 有了这个，就可以注册观察者，然后等待事件发生
/// </summary>
public class PCInputMgr : SingletonBehaviour<PCInputMgr>
{
    /// <summary>
    /// 鼠标事件输入类型
    /// </summary>
    public enum MouseInputKeyEnum
    {
        mouse_left,
        mouse_right,
        mouse_mid,
        moveOver
    };

    /// <summary>
    /// 按键方式
    /// </summary>
    public enum InputTypeEnum
    {
        down,   //鼠标，键盘按键按下
        hold,   //按住未抬起
        up,     //鼠标，键盘按键抬起
    }

    //鼠标点击事件
    public event Listener<MouseInputKeyEnum, InputTypeEnum,Vector3> MouseInputEvent;

    //中轮滚动事件
    public event Listener<float> MouseScrollWheelEvent;

    //鼠标移动事件
    public event Listener<Vector2> MouseMoveEvent;

    //键盘点击事件
    public event Listener<byte, InputTypeEnum> KBInputEvent;

    protected override void Awake()
    {      
        base.Awake();
        InitCursor();
    }

    public void tUpdate()
    {
        ProcessMouse();

        ProcessKB();

    }

    private bool bMouseRightDown = false;

    //中轮滚动中
    private bool bScorlling = false;

    private void ProcessMouse()
    {
        if (null != MouseInputEvent)
        {
            //鼠标滑动事件
            MouseInputEvent.Invoke(MouseInputKeyEnum.moveOver, InputTypeEnum.hold, Input.mousePosition);

            //左键点击处理
            if (Input.GetMouseButtonDown(0))
            {
                MouseInputEvent.Invoke(MouseInputKeyEnum.mouse_left, InputTypeEnum.down, Input.mousePosition);
            }
            //左键抬起处理
            if (Input.GetMouseButtonUp(0))
            {
                MouseInputEvent.Invoke(MouseInputKeyEnum.mouse_left, InputTypeEnum.up, Input.mousePosition);
            }

            //右键点下处理
            if (Input.GetMouseButtonDown(1))
            {
                MouseInputEvent.Invoke(MouseInputKeyEnum.mouse_right, InputTypeEnum.down, Input.mousePosition);
            }

            //右键抬起处理
            if (Input.GetMouseButtonUp(1))
            {
                MouseInputEvent.Invoke(MouseInputKeyEnum.mouse_right, InputTypeEnum.up, Input.mousePosition);
            }

            //中轮点下
            if (Input.GetMouseButtonDown(2))
            {
                MouseInputEvent.Invoke(MouseInputKeyEnum.mouse_mid, InputTypeEnum.down, Input.mousePosition);
            }

            //中轮抬起
            if (Input.GetMouseButtonUp(2))
            {
                MouseInputEvent.Invoke(MouseInputKeyEnum.mouse_mid, InputTypeEnum.up, Input.mousePosition);
            }
        }
      
        //中轮滚动
        if (null != MouseScrollWheelEvent)
        {
            float mouseScrool = Input.GetAxis("Mouse ScrollWheel");
            //中论滚动
            if (mouseScrool != 0)
            {
                bScorlling = true;
                
                MouseScrollWheelEvent.Invoke(mouseScrool);
            }
            else
            {
                //从滚动中变为停止时发出一次事件，其他无滚动时不再发出事件
                if (bScorlling)
                {                  
                    bScorlling = false;
                    MouseScrollWheelEvent.Invoke(0);
                }

            }
        }

        //鼠标拖动,只有右键按下时才发出拖动事件
        if (null != MouseMoveEvent)
        {
            if (Input.GetMouseButtonDown(1))
            {
                bMouseRightDown = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                bMouseRightDown = false;
            }

            if (bMouseRightDown)
            {
                float offx = Input.GetAxis("Mouse X");
                float offy = Input.GetAxis("Mouse Y");
                Vector2 v2 = new Vector2(offx, offy);
                MouseMoveEvent(v2);
            }
            
        }

    }

    private void ProcessKB()
    {

    }

    #region mouse icon
    private Texture2D Cursor_Normal;
    private Texture2D Cursor_Skill;
    private Texture2D Cursor_Npc;

    public void InitCursor()
    {
        Cursor_Normal = Resources.Load<Texture2D>("Cursor/normal");
        Cursor_Skill = Resources.Load<Texture2D>("Cursor/skill");
        Cursor_Npc = Resources.Load<Texture2D>("Cursor/npc");
        ChangeCursor(CursorEnum.normal);
    }

    public enum CursorEnum
    {
        normal,//默认状态，这个状态一般是没有用的
        skill,//攻击，当技能被点选时
        npc,//npc商店
    }

    public void ChangeCursor(CursorEnum cursorType)
    {
        switch (cursorType)
        {
            case CursorEnum.normal:
                Cursor.SetCursor(Cursor_Normal, Vector2.zero, CursorMode.Auto);
                break;
            case CursorEnum.npc:
                Cursor.SetCursor(Cursor_Npc, Vector2.zero, CursorMode.Auto);
                break;
            case CursorEnum.skill:
                Cursor.SetCursor(Cursor_Skill, Vector2.zero, CursorMode.Auto);
                break;
        }

    }
    #endregion
}

