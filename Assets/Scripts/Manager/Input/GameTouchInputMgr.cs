//using UnityEngine;
//using NLog;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//public class GameTouchInputMgr : MonoBehaviour
//{
//    private bool BeShowLog = true;

//    #region var

//    // 摇杆
//    [SerializeField] VariableJoystick MoveJoystick;

//    // 技能0
//    [SerializeField] RectTransform SK0_Button;

//    // 技能1
//    [SerializeField] VariableJoystick SK1_Joystick;

//    // 技能2
//    [SerializeField] VariableJoystick SK2_Joystick;

//    // 技能3
//    [SerializeField] VariableJoystick SK3_Joystick;

//    /// <summary>
//    /// 正在使用(已经按下)的那个(方向或位置性)技能
//    /// 同一时间只能有一个技能被按下
//    /// </summary>
//    private VariableJoystick CurSKJoystick;

//    #endregion

//    #region ref comp

//    private CameraMgr _CameraMgr;
//    private CameraMgr CameraMgr
//    {
//        get
//        {
//            if (null == _CameraMgr) _CameraMgr = CameraMgr.GetIns();
//            return _CameraMgr;
//        }
//    }

//    #endregion

//    #region event

//    /// <summary>
//    /// 技能事件
//    /// int：技能id
//    /// Vector3：技能附带信息方向或位置
//    /// </summary>
//    public event Listener<int, Vector3> SkillEvent;

//    /// <summary>
//    /// 运动事件 
//    /// bool： true代表移动fasle停下  
//    /// float： 运动方向，世界坐标下y轴的偏移值
//    /// </summary>
//    public event Listener<bool, float> MoveEvent;

//    #endregion

//    #region life

//    private static GameTouchInputMgr Ins;

//    public static GameTouchInputMgr GetIns()
//    {
//        return Ins;
//    }

//    private void Awake()
//    {        
//        Ins = this;
//    }

//    private void Start()
//    {
//        SK1_Joystick.ID = SysSetting.HK_SK1;
//        SK2_Joystick.ID = SysSetting.HK_SK2;
//        SK3_Joystick.ID = SysSetting.HK_SK3;
//    }

//    private void OnDestroy()
//    {
//        Ins = null;
//    }

//    private void OnEnable()
//    {
//        //移动开始
//        MoveJoystick.OnJoystickDown += ProcessMoveStartEvent;
//        //移动中
//        MoveJoystick.OnJoystickDrag += ProcessMoveEvent;  
//        //移动结束
//        MoveJoystick.OnJoystickUp += ProcessMoveStopEvent;

//        EventTriggerListener.GetListener(SK0_Button.gameObject).onPointerDown += PocessSkill0_Down;
        
//        //技能1事件
//        SK1_Joystick.OnJoystickDown += ProcessSkill1_Down_Event;
//        SK1_Joystick.OnJoystickDrag += ProcessSkill1_Drag_Event;
//        SK1_Joystick.OnJoystickUp += ProcessSkill1_Up_Event;

//        //技能2事件
//        SK2_Joystick.OnJoystickDown += ProcessSkill2_Down_Event;
//        SK2_Joystick.OnJoystickDrag += ProcessSkill2_Drag_Event;
//        SK2_Joystick.OnJoystickUp += ProcessSkill2_Up_Event;

//        //技能3事件
//        SK3_Joystick.OnJoystickDown += ProcessSkill3_Down_Event;
//        SK3_Joystick.OnJoystickDrag += ProcessSkill3_Drag_Event;
//        SK3_Joystick.OnJoystickUp += ProcessSkill3_Up_Event;

//    }

//    private void OnDisable()
//    {
//        //移动开始
//        MoveJoystick.OnJoystickDown -= ProcessMoveEvent;
//        //移动中
//        MoveJoystick.OnJoystickDrag -= ProcessMoveEvent;
//        //移动结束
//        MoveJoystick.OnJoystickUp -= ProcessMoveStopEvent;

//        EventTriggerListener.GetListener(SK0_Button.gameObject).onPointerDown -= PocessSkill0_Down;

//        //技能1事件
//        SK1_Joystick.OnJoystickDown -= ProcessSkill1_Down_Event;
//        SK1_Joystick.OnJoystickDrag -= ProcessSkill1_Drag_Event;
//        SK1_Joystick.OnJoystickUp -= ProcessSkill1_Up_Event;

//        //技能2事件
//        SK2_Joystick.OnJoystickDown -= ProcessSkill2_Down_Event;
//        SK2_Joystick.OnJoystickDrag -= ProcessSkill2_Drag_Event;
//        SK2_Joystick.OnJoystickUp -= ProcessSkill2_Up_Event;

//        //技能3事件
//        SK3_Joystick.OnJoystickDown -= ProcessSkill3_Down_Event;
//        SK3_Joystick.OnJoystickDrag -= ProcessSkill3_Drag_Event;
//        SK3_Joystick.OnJoystickUp -= ProcessSkill3_Up_Event;
//    }

//    #endregion

//    #region move

   
//    void ProcessMoveStartEvent(Vector2 joystickV2)
//    {
//        ProcessMove(joystickV2, true);
//    }

//    void ProcessMoveEvent(Vector2 joystickV2)
//    {
//        ProcessMove(joystickV2, true);
//    }

//    void ProcessMoveStopEvent(Vector2 joystickV2)
//    {
//        ProcessMove(joystickV2, false);
//    }

//    void ProcessMove(Vector2 joystickV2,bool bMove)
//    {
       
//        Vector3 touchV = Joystick2WorldDir(joystickV2);

//        //世界坐标z轴方向 沿着y轴 旋转到 摇杆在世界坐标的真实位置的角度，这个角度就是最终角色朝向
//        //注意这里顺时针旋转得到角度是正
//        float angleTouch2worldZ = Vector3.SignedAngle(Vector3.forward, touchV, Vector3.up);

//        MoveEvent?.Invoke(bMove, angleTouch2worldZ);
//    }

//    #endregion

//    #region skill

//    /// <summary>
//	/// 用于临时保存当前选择技能的操作类型
//	/// </summary>
//	private byte CurSelSkillOperateType = FightEnum.SKOperateType_Dir;

//    void PocessSkill0_Down(GameObject go)
//    {
//        //NTODO 以后这里加冷却判断？
//        SkillEvent?.Invoke(1, Vector3.zero);
//    }

//    void ProcessSkill0_Up(GameObject go)
//    {
        
//    }

//    void ProcessSkill1_Down_Event(Vector2 joystickV2)
//    {
//        ProcessSkillDown(SysSetting.HK_SK1);
//    }

//    void ProcessSkill1_Drag_Event(Vector2 joystickV2)
//    {
//        ProcessSkillDrag(SysSetting.HK_SK1);
//    }

//    void ProcessSkill1_Up_Event(Vector2 joystickV2)
//    {
//        ProcessSkillUp(SysSetting.HK_SK1);
//    }

//    void ProcessSkill2_Down_Event(Vector2 joystickV2)
//    {
//        ProcessSkillDown(SysSetting.HK_SK2);
//    }

//    void ProcessSkill2_Drag_Event(Vector2 joystickV2)
//    {
//        ProcessSkillDrag(SysSetting.HK_SK2);
//    }

//    void ProcessSkill2_Up_Event(Vector2 joystickV2)
//    {
//        ProcessSkillUp(SysSetting.HK_SK2);
//    }

//    void ProcessSkill3_Down_Event(Vector2 joystickV2)
//    {
//        ProcessSkillDown(SysSetting.HK_SK3);
//    }

//    void ProcessSkill3_Drag_Event(Vector2 joystickV2)
//    {
//        ProcessSkillDrag(SysSetting.HK_SK3);
//    }

//    void ProcessSkill3_Up_Event(Vector2 joystickV2)
//    {
//        ProcessSkillUp(SysSetting.HK_SK3);
//    }

//    /// <summary>
//    /// 处理技能按下
//    /// </summary>
//    /// <param name="SkillHotkey"> 热键，快捷键，某个技能栏</param>
//    void ProcessSkillDown(byte SkillHotkey)
//    {
//        //这里要处理是不是瞬发技能        
//        if (null != CurSKJoystick)
//        {
//            if (CurSKJoystick.ID == SkillHotkey)
//            {
//                LogMgr.I("GameTouchInputMgr", "ProcessSkillDown", "CurSKJoystick.ID:"+ CurSKJoystick.ID+ " SkillHotkey:"+ SkillHotkey + "两者相同，一个技能被多次按下，不进行进一步处理摇杆按下事件", BeShowLog);
//                return;
//            }

//            CurSKJoystick.Cancel();
//            CurSKJoystick = null;
//        }

//        //根据当前热键(快捷键或技能栏)获取当前技能id
//        int skillID = SysSetting.Hotkey2SkillDic[SkillHotkey];

//        if (skillID == 0)
//        {
//            LogMgr.I("GameTouchInputMgr", "ProcessSkillDown", "skillID=0 当前技能栏为null技能，不进行进一步处理摇杆按下事件", BeShowLog);
//            return;
//        }

//        //获取技能操作类型，瞬发型，方向型，位置型
//        CurSelSkillOperateType = GameDataMgr.GetSkTypeByID(skillID).OperateType;

//        LogMgr.I("GameTouchInputMgr", "ProcessSkillDown", "skillID:" + skillID + " CurSelSkillOperateType:" + CurSelSkillOperateType + " SkillHotkey:" + SkillHotkey, BeShowLog);

//        CurSKJoystick = GetCurSKJoysticBySkillHotKey(SkillHotkey);

//    }

//    void ProcessSkillDrag(byte SkillHotkey)
//    {
//        if (null == CurSKJoystick) return;

//        if (CurSKJoystick.ID != SkillHotkey) { return; }

//        switch (CurSelSkillOperateType)
//        {
//            case FightEnum.SKOperateType_Dir:
//                Vector3 dir = new Vector3(CurSKJoystick.Direction.normalized.x, 0, CurSKJoystick.Direction.normalized.y);
//                break;
//            case FightEnum.SKOperateType_TargetPos:
//                int skillID = SysSetting.Hotkey2SkillDic[SkillHotkey];
//                SkillType skillType = GameDataMgr.GetSkTypeByID(skillID);
//                Vector2 v2 = CurSKJoystick.Direction * skillType.SkillPartTypes[0].MotionMaxDistance;
//                Vector3 pos = new Vector3(v2.x, 0, v2.y);
//                break;
//            case FightEnum.SKOperateType_Instant:
//                return;
//        }

//        //TODO 更新方向图或者位置图标位置
//    }

//    void ProcessSkillUp(byte SkillHotkey)
//    {
//        if (null == CurSKJoystick)
//        {
//            LogMgr.I("GameTouchInputMgr", "ProcessSkillUp", "CurSKJoystick==null 不继续处理摇杆抬起事件 skillHotKey:"+ SkillHotkey,BeShowLog);
//            return;
//        } 

//        if (CurSKJoystick.ID != SkillHotkey)
//        {
//            LogMgr.I("GameTouchInputMgr", "ProcessSkillUp", "CurSKJoystick.ID:"+ CurSKJoystick.ID + " SkillHotkey:"+ SkillHotkey+ " 两者不同，不继续处理摇杆抬起事件", BeShowLog);
//            return;
//        }

        
//        int skillID = SysSetting.Hotkey2SkillDic[SkillHotkey];
//        if (skillID == 0)
//        {
//            LogMgr.I("GameTouchInputMgr", "ProcessSkillUp", "skillID==0 不继续处理摇杆抬起事件 SkillHotkey:" + SkillHotkey, BeShowLog);
//            return;
//        }

//        LogMgr.I("GameTouchInputMgr", "ProcessSkillUp", "skillID:"+skillID+ " CurSelSkillOperateType:"+ CurSelSkillOperateType +" SkillHotkey:"+ SkillHotkey, BeShowLog);

//        switch (CurSelSkillOperateType)
//        {
//            case FightEnum.SKOperateType_Dir:
//                //计算技能(世界坐标)方向
//                Vector3 dir = Joystick2WorldDir(CurSKJoystick.Direction).normalized;                
//                SkillEvent?.Invoke(skillID, dir);
//                break;

//            case FightEnum.SKOperateType_TargetPos:

//                //获取技能part1最大距离
//                SkillType skillType = GameDataMgr.GetSkTypeByID(skillID);
//                float maxDis = skillType.SkillPartTypes[0].MotionMaxDistance;

//                //光标距离角色距离
//                float dis = CurSKJoystick.Direction.magnitude * maxDis;
//                //技能(世界坐标)方向
//                Vector3 dir0 = Joystick2WorldDir(CurSKJoystick.Direction).normalized;

//                //DebugDraw.DrawVector(HeroCtrl.GetIns().MainPlayer.transform.position, dir0, dis, 3f, Color.green, 100);

//                Vector3 pos = HeroCtrl.GetIns().MainPlayer.transform.position + dir0 * dis;

//                LogMgr.I("GameTouchInputMgr", "ProcessSkillUp", "目地型技能抬手 touch位置:"+ CurSKJoystick.Direction+" 目标位置:"+ pos+" 目标最大移动距离:"+ maxDis, BeShowLog);
//                SkillEvent?.Invoke(skillID, pos);

//                break;

//            case FightEnum.SKOperateType_Instant:
//                SkillEvent?.Invoke(skillID, Vector3.zero);
//                break;
//        }

//        CurSKJoystick = null;
//    }

//    /// <summary>
//    /// 输入摇杆xy，返回当前相机角度下，摇杆实际指向的世界坐标向量
//    /// 得到的结果是一个方向向量
//    /// 具体计算图看doc/touchInputMoveDir
//    /// </summary>
//    /// <param name="joystickV2"></param>
//    /// <returns></returns>
//    private Vector3 Joystick2WorldDir(Vector2 joystickV2)
//    {
//        //获取相机正(z)方向
//        Vector3 camZ = CameraMgr.GetCameraForward();
//        camZ.y = 0;

//        //获取世界坐标z轴方向到相机正(z)方向的旋转
//        Quaternion q = Quaternion.FromToRotation(Vector3.forward, camZ);

//        //构建摇杆相对世界坐标的向量
//        Vector3 touchV = new Vector3(0, 0, 0);
//        touchV.x = joystickV2.x;
//        touchV.y = 0f;
//        touchV.z = joystickV2.y;

//        //计算得到摇杆相对相机的向量，从而得到摇杆在世界坐标的真实位置
//        touchV = q * touchV;

//        return touchV;
//    }

//    VariableJoystick GetCurSKJoysticBySkillHotKey(byte SkillHotkey)
//    {
//        VariableJoystick _CurSKJoystick = null;
//        switch (SkillHotkey)
//        {
//            case SysSetting.HK_SK1:
//                _CurSKJoystick = SK1_Joystick;
//                break;
//            case SysSetting.HK_SK2:
//                _CurSKJoystick = SK2_Joystick;
//                break;
//            case SysSetting.HK_SK3:
//                _CurSKJoystick = SK3_Joystick;
//                break;
//        }
//        return _CurSKJoystick;
//    }

//    #endregion



//}
