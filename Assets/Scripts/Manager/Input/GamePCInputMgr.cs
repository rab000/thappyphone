using UnityEngine;
using NLog;

/// <summary>
/// 游戏内pc输入管理
/// 负责监听键盘鼠标输入
/// 转换成移动，释放技能等信息通知给HeroCtrl
/// 这个类可以认为是HeroCtrl功能的一部分即接收输入，解析，
/// 然后由HeroCtrl做后续移动和释放技能的操作
/// 
/// 说明下技能释放过程，
/// 1 瞬发技能，选中技能栏，鼠标点击触发
/// 2 有方向和终点位置的技能，选中技能栏，鼠标选择方向位置后点击触发
/// 
/// 注意这里用的是moba的操作模式，不是zero的操作模式，两者有个明显的区别
/// 就是使用完技能后，当前选中技能栏是保留还是清除，moba的就清除，zero的就保留
/// 这里选用moba的，以后如果要使用zero单独做个控制器需要就挂载
/// </summary>
public class GamePCInputMgr : MonoBehaviour
{
    private bool BeShowLog = false;

    #region event

    //对外发出使用技能事件，int 技能id，Vector3技能附带信息方向或位置
    public event Listener<int, Vector3> SkillEvent;
    //运动事件 bool true代表移动fasle停下  float 运动方向，世界坐标下y轴的偏移值
    public event Listener<bool,float> MoveEvent;

    #endregion

    #region var

    public const byte Layer8_Ground = 8;

    #endregion

    #region ref comp

    private CameraMgr _CameraMgr;
    private CameraMgr CameraMgr
    {
        get
        {
            if (null == _CameraMgr) _CameraMgr = CameraMgr.GetIns();
            return _CameraMgr;
        }
    }

    private Camera _MainCamera;
    private Camera MainCamera
    {
        get
        {
            if (null == _MainCamera) _MainCamera = CameraMgr.GetIns().GetMainCamera();
            return _MainCamera;
        }
    }

    #endregion

    #region life

    private static GamePCInputMgr Ins;

    public static GamePCInputMgr GetIns()
    {
        return Ins;
    }

    private void Awake()
    {
        Ins = this;
    }

    private void OnDestroy()
    {
        Ins = null;
    }

    private void OnEnable()
    {
        PCInputMgr.GetIns().MouseInputEvent += ProcessMouse;       
    }

    private void OnDisable()
    {
        PCInputMgr.GetIns().MouseInputEvent -= ProcessMouse;
    }

    #endregion

    #region mouse

    /// <summary>
    /// 缓存鼠标滑动物体，用于处理OperatableObj的OnFocue和OnLostFocus
    /// </summary>
    private GameObject _MouseOverObj;
    private GameObject MouseOverObj
    {
        get { return _MouseOverObj; }
        set
        {
            if (_MouseOverObj == value) return;

            if (_MouseOverObj != null)
            {
                var operateObj = _MouseOverObj.GetComponent<OperatableMonoObj>();
                if (null != operateObj) operateObj.OnLostFocus();
            }

            _MouseOverObj = value;
            if (null != _MouseOverObj)
            {
                var o = _MouseOverObj.GetComponent<OperatableMonoObj>();
                if (null != o) o.OnFocus();
            }
        }
    }

    /// <summary>
    /// 缓存鼠标click物体，用于处理OperatableObj的OnFocue和OnLostFocus
    /// </summary>
    private GameObject _MouseSelObj;
    private GameObject MouseSelObj
    {
        get { return _MouseSelObj; }
        set
        {
            //二次点选的操作click
            if (_MouseSelObj == value)
            {
                //TODO 二次点选为什么逻辑回事MouseClick，这里要重新规划，关于鼠标二次点选某个目标的逻辑以后做
                //if(null!=_MouseSelObj)OnMouseClick(_MouseSelObj); 
                return;
            }

            //OperatableMonoObj tempObj = null;
            if (null != _MouseSelObj)
            {
                OperatableMonoObj oldSelObj = _MouseSelObj.GetComponent<OperatableMonoObj>();
                if (oldSelObj != null) oldSelObj.OnDeSel();
            }

            _MouseSelObj = value;

            if (null != _MouseSelObj)
            {
                //这里跟上边是不同的_MouseSelObj，注意下
                OperatableMonoObj newSelObj = _MouseSelObj.GetComponent<OperatableMonoObj>();
                if (newSelObj != null) newSelObj.OnSel();
            }
        }
    }

    /// <summary>
    /// 鼠标事件处理
    /// </summary>
    /// <param name="key">键位</param>
    /// <param name="type">按键方式</param>
    /// <param name="mouseX">Mouse x.</param>
    /// <param name="mouseY">Mouse y.</param>
    private void ProcessMouse(PCInputMgr.MouseInputKeyEnum key, PCInputMgr.InputTypeEnum type, Vector3 mousePos)
    {

        //LogMgr.I ("HeroCtrl","ProcesMouse","key:"+key+" inputType:"+type+ " mousePos:"+mousePos,BeShowLog);
        //重复射线选择物体，这里要考虑过滤掉地表
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        //这里要忽略的时不需要被鼠标掠过的层,暂时时ground
        LayerMask lm = ~(1 << Layer8_Ground);//INFO 这里要做好忽略层，暂时站位的是忽略2层，第二层正好是Igenore Raycast

        RaycastHit hit;
        GameObject _go = null;
        if (Physics.Raycast(ray, out hit, 1000, lm))
        {
            _go = hit.transform.gameObject;
        }

        switch (key)
        {
            case PCInputMgr.MouseInputKeyEnum.moveOver://处理鼠标滑过操作
                OnMouseMoveOver(_go, mousePos);
                break;

            case PCInputMgr.MouseInputKeyEnum.mouse_left://鼠标左键操作
                switch (type)
                {
                    case PCInputMgr.InputTypeEnum.down:
                        OnMouseLeftDown(_go, mousePos);
                        break;
                    case PCInputMgr.InputTypeEnum.up:
                        OnMouseLeftUp(_go, mousePos);
                        break;
                }
                break;

            case PCInputMgr.MouseInputKeyEnum.mouse_right://鼠标右键操作

                switch (type)
                {
                    case PCInputMgr.InputTypeEnum.down:
                        OnMouseRightDown(_go, mousePos);
                        break;
                    case PCInputMgr.InputTypeEnum.up:
                        OnMouseRightUp(_go, mousePos);
                        break;
                }
                break;
        }
    }

    private void OnMouseMoveOver(GameObject go, Vector3 mousePos)
    {

        //TODO 这里地表应该不用赋值,并不是所有物体身上都有OperatableMonoObj，这里依赖于分层和spr继承体系

        //TODO 需要完善分层操作，terrain,装饰物（应该被忽略）,spr

        //TODO 一个问题，商店属于什么分类，防御塔属于什么分类

        //TODO 商店是Spr么，spr这里有root和trm，所以这么看不是Spr，却是obj，可点选

        //TODO 防御塔是什么

        //综合看实际问题是分层(处理类别)，tag分类(继承体系)

        //重点是分类，obj

        MouseOverObj = go;

    }

    private void OnMouseLeftDown(GameObject go, Vector3 mousePos)
    {
        MouseSelObj = go;
               
    }

    private void OnMouseLeftUp(GameObject go, Vector3 mousePos)
    {
        //zero的处理，什么都不做，不会取消选中
        //war的处理,根据矩形计算选中obj，并触发onfocus
    }

    private void OnMouseRightDown(GameObject go, Vector3 mousePos)
    {


    }

    private void OnMouseRightUp(GameObject go, Vector3 mousePos)
    {
        //zero的处理，结束拖屏
        //war的处理，无操作
    }

    #endregion


    

}
