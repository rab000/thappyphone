
using UnityEngine;
using System.Collections;

/// <summary>
/// 自由视角相机
/// 主要调试查看场景用
/// 
/// 右键转向
/// 中键拉远拉近
/// wasd水平移动
/// qe上下移动
/// </summary>
public class FreeCamera : MonoBehaviour{
	//旋转变量;
	private float m_deltX = 0f;
	private float m_deltY = 0f;
	//缩放变量;
	private float m_distance = 10f;
	private float m_mSpeed = 5f;
	//移动变量;
	private Vector3 m_mouseMovePos = Vector3.zero;

	private Vector3 OriginalPos;

	private Camera _Camera;

	void Start (){
		_Camera = GetComponent<Camera>();
		OriginalPos = _Camera.transform.position;
	}

	void Update (){
		//鼠标右键点下控制相机旋转;
		if (Input.GetMouseButton (1)) {
			m_deltX += Input.GetAxis ("Mouse X") * m_mSpeed;
			m_deltY -= Input.GetAxis ("Mouse Y") * m_mSpeed;
			m_deltX = ClampAngle (m_deltX, -360, 360);
			m_deltY = ClampAngle (m_deltY, -70, 70);
			_Camera.transform.rotation = Quaternion.Euler (m_deltY, m_deltX, 0);
		}

		//鼠标中键点下场景缩放;
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			//自由缩放方式;
			m_distance = Input.GetAxis ("Mouse ScrollWheel") * 10f;
			_Camera.transform.localPosition = _Camera.transform.position + _Camera.transform.forward * m_distance;
		}
			
		//相机复位远点;
		if (Input.GetKey (KeyCode.Space)) {
			m_distance = 10.0f;
			_Camera.transform.position = OriginalPos;
		}

		if (Input.GetKey (KeyCode.W)) {
			transform.Translate(Vector3.forward*0.1f);
		}

		if (Input.GetKey (KeyCode.A)) {
			transform.Translate(Vector3.left*0.1f);
		}

		if (Input.GetKey (KeyCode.S)) {
			transform.Translate(Vector3.back*0.1f);
		}

		if (Input.GetKey (KeyCode.D)) {
			transform.Translate(Vector3.right*0.1f);
		}

		if (Input.GetKey (KeyCode.Q)) {
			transform.Translate(Vector3.down*0.1f);
		}

		if (Input.GetKey (KeyCode.E)) {
			transform.Translate(Vector3.up*0.1f);
		}
	}

	//规划角度;
	float ClampAngle (float angle, float minAngle, float maxAgnle)
	{
		if (angle <= -360)
			angle += 360;
		if (angle >= 360)
			angle -= 360;

		return Mathf.Clamp (angle, minAngle, maxAgnle);
	}
}

