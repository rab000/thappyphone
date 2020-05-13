using UnityEngine;
using NLog;
using System.Collections;
/// <summary>
/// Camera位置管理器
/// 用来处理相机跟随，旋转，移动
/// </summary>
public class CameraMgr : MonoBehaviour {

    private bool BeShowLog = true;

	#region config var
	
	/// <summary>
	/// 按住右键移动时旋转相机速度
	/// </summary>
	float xSpeed = 5f;
	float ySpeed = 5f;

	/// <summary>
	/// y轴最大最小滚动（其实就是在屏幕上上下滚动的极限）
	/// </summary>
	float yMinLimit = 3f;
	float yMaxLimit = 80f;

	/// <summary>
	/// 摄像机距离缩放最大最小值
	/// </summary>
	float zoomMin = 5f;
	float zoomMax = 10f;

	/// <summary>
	/// 鼠标中轮滚动速度
	/// </summary>
	float scrollSpeed = 10f;
     
    /// <summary>
    /// 摄像机跟随目标
    /// </summary>
    public Transform cameraTargetTrm;

	/// <summary>
	/// 摄像机自身Transform
	/// </summary>
	Transform cameraTrm;

	/// <summary>
	/// 摄像机组件
	/// </summary>
	public Camera mainCamera;

    #endregion

    #region temp var

    /// <summary>
	/// 相机距离物体距离
	/// </summary>
	float distance;

    /// <summary>
    /// 摄像机距离插值(随distance变换而变换，目的为了保证平滑插值，缩放时动画连续)
    /// </summary>
    float distanceLerp;

    /// <summary>
    /// 摄像机位置
    /// </summary>
    Vector3 camPosition;
    /// <summary>
    /// 设为false屏蔽缩放
    /// </summary>
    bool CanScroll = true;

    /// <summary>
    /// 设为false屏蔽旋转
    /// </summary>
    bool CanRotate = true;

    /// <summary>
	/// 这两个值实际是针对鼠标操作屏幕的x，y所以在屏幕上鼠标上下移动改变y值，但在3d中实际要沿着x轴转动，这里要注意
	/// </summary>
	float x;//x方向旋转角度
    float y;//y方向旋转角度

    /// <summary>
    /// 3种情况需要更新camera位置，跟随目标移动，有相机旋转输入，有相机缩放输入
    /// </summary>
    private bool bNeedUpdateCameraPos = false;

    /// <summary>
    /// 记录上一次目标位置，当目标位置有变化，就更新相机位置
    /// </summary>
    private Vector3 targetPos = Vector3.zero;

    #endregion

    #region life

    static CameraMgr ins;

    public static CameraMgr GetIns() { return ins; }

    void Awake()
    {
        ins = this;
    }

    void Start()
    {
        SetCamera(transform);
    }

    public void tLateUpdate()
    {
        if (null == cameraTrm) return;

        if (null == cameraTargetTrm) return;

        if (cameraTargetTrm.position != targetPos)
        {
            bNeedUpdateCameraPos = true;
            targetPos = cameraTargetTrm.position;
        }

        if (bNeedUpdateCameraPos)
        {
            UpdateCameraPos();
            bNeedUpdateCameraPos = false;
        }
    }

    void OnDestroy()
    {
        ins = null;
    }

    #endregion

    #region public

    /// <summary>
    /// 对外提供基于当前相机视角的8方向
    /// </summary>
    /// <value>The DI r U.</value>
    public float CAM_VIEW_DIR_UP{get{ return cameraTrm.localEulerAngles.y;}}
	public float CAM_VIEW_DIR_DOWN{get{ return MathsTool.Limit2_360Degree(cameraTrm.localEulerAngles.y+180);}}
	public float CAM_VIEW_DIR_LEFT{get{ return MathsTool.Limit2_360Degree(cameraTrm.localEulerAngles.y-90);}}
	public float CAM_VIEW_DIR_RIGHT{get{ return MathsTool.Limit2_360Degree(cameraTrm.localEulerAngles.y+90);}}
	public float CAM_VIEW_DIR_LEFT_UP{get{ return MathsTool.Limit2_360Degree(cameraTrm.localEulerAngles.y-45);}}
	public float CAM_VIEW_DIR_LEFT_DOWN{get{ return MathsTool.Limit2_360Degree(cameraTrm.localEulerAngles.y-135);}}
	public float CAM_VIEW_DIR_RIGHT_UP{get{ return MathsTool.Limit2_360Degree(cameraTrm.localEulerAngles.y+45);}}
	public float CAM_VIEW_DIR_RIGHT_DOWN{get{ return MathsTool.Limit2_360Degree(cameraTrm.localEulerAngles.y+135);}}
	
    public Vector3 GetCameraForward()
    {
        return transform.forward;
    }

    public float GetCameraEulerY()
    {
        return MathsTool.Limit2_360Degree(transform.localEulerAngles.y);
    }

    private void SetCamera(Transform cameraTrm)
    {
        this.cameraTrm = cameraTrm;

        mainCamera = cameraTrm.gameObject.GetComponent<Camera>();

        if (null == mainCamera)
        {
            LogMgr.E("CameraManager", "SetCamera", "设置相机时找不到Camera组件", BeShowLog);
            return;
        }
    }

    public Camera GetMainCamera() { return mainCamera; }

    public void LockScroll(bool b) { CanScroll = !b; }

    public void LockRotate(bool b) { CanRotate = !b; }

    public void SetTarget(Transform targetTrm)
    {
        cameraTargetTrm = targetTrm;
        CalInitPos();
    }

    /// <summary>
    /// 绑定输入事件
    /// 相机旋转，缩放事件需要配合输入管理器(PCInputMgr,TouchMgr)
    /// 进入游戏场景时就绑定输入事件
    /// </summary>
    public void BindInputEvent()
    {
        switch (GameConfig.InputCtrlType)
        {
            case GameConfig.InputCtrlTypeEnum.mobile:
                TouchMgr.GetIns().TouchMoveEvent += ProcessMoveEvent;
                TouchMgr.GetIns().TouchScaleEvent += ProcessScaleEvent;
                break;
            case GameConfig.InputCtrlTypeEnum.pc:
                PCInputMgr.GetIns().MouseMoveEvent += ProcessMoveEvent;
                PCInputMgr.GetIns().MouseScrollWheelEvent += ProcessScaleEvent;
                break;
        }
    }

    public void UnBindInputEvent()
    {
        switch (GameConfig.InputCtrlType)
        {
            case GameConfig.InputCtrlTypeEnum.mobile:
                TouchMgr.GetIns().TouchMoveEvent -= ProcessMoveEvent;
                TouchMgr.GetIns().TouchScaleEvent -= ProcessScaleEvent;
                break;
            case GameConfig.InputCtrlTypeEnum.pc:
                PCInputMgr.GetIns().MouseMoveEvent -= ProcessMoveEvent;
                PCInputMgr.GetIns().MouseScrollWheelEvent -= ProcessScaleEvent;
                break;
        }
    }

    #endregion

    #region func

    private void ProcessScaleEvent(float scale)
    {

        if (!CanScroll) return;

        if (null == cameraTargetTrm) return;

        distanceLerp = Mathf.Lerp(distanceLerp, distance, Time.deltaTime * 5);
       
        if (scale != 0)
        {
           
            //对于触屏，调整距离时禁止旋转，避免camera抖动
            //pc这个操作可有可无，PCInputMgr不会输出scale=0的情况
            LockRotate(true);
            distance = Vector3.Distance(transform.position, cameraTargetTrm.position);
            distance = ScrollLimit(distance - scale * scrollSpeed, zoomMin, zoomMax);
        }
        else
        {
            LockRotate(false);
        }

        bNeedUpdateCameraPos = true;
    }

    private void ProcessMoveEvent(Vector2 moveV2)
    {
        if (!CanRotate) return;

        if (null == cameraTargetTrm) return;

        y -= moveV2.y * ySpeed;
        x += moveV2.x * xSpeed;
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        bNeedUpdateCameraPos = true;

    }
   
    // 计算摄像机初始位置
    void CalInitPos()
    {
        Vector3 angles = cameraTrm.eulerAngles;
        x = angles.y;
        y = angles.x;
        distance = zoomMin;
        distanceLerp = distance;

        UpdateCameraPos();

    }
    
    void UpdateCameraPos()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 calPos = new Vector3(0, 0, -distanceLerp);
        camPosition = rotation * calPos + cameraTargetTrm.position;
        transform.rotation = rotation;
        transform.position = camPosition;
    }

    float ScrollLimit(float dist, float min, float max)
    {
		if (dist < min)
			dist= min;

		if (dist > max)
			dist= max; 

		return dist;
	}

	float ClampAngle(float angle,float min,float max)
    {
		if(angle < -360)
			angle += 360;
		if(angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle,min,max);
	}

    #endregion

}

