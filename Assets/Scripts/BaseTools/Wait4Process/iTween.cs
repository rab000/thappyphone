﻿//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using UnityEngine;

//public class iTween : MonoBehaviour
//{
//	public string _name;
//	private ApplyTween apply;
//	private AudioSource audioSource;
//	private static GameObject cameraFade;
//	private Color[,] colors;
//	public float delay;
//	private float delayStarted;
//	private EasingFunction ease;
//	public EaseType easeType;
//	private float[] floats;
//	public string id;
//	private bool isLocal;
//	public bool isPaused;
//	public bool isRunning;
//	private bool kinematic;
//	private float lastRealTime;
//	private bool loop;
//	public LoopType loopType;
//	public string method;
//	private NamedValueColor namedcolorvalue;
//	private CRSpline path;
//	private float percentage;
//	private bool physics;
//	private Vector3 postUpdate;
//	private Vector3 preUpdate;
//	private Rect[] rects;
//	private bool reverse;
//	private float runningTime;
//	private Space space;
//	public float time;
//	private Hashtable tweenArguments;
//	public static ArrayList tweens = new ArrayList();
//	public string type;
//	private bool useRealTime;
//	private Vector2[] vector2s;
//	private Vector3[] vector3s;
//	private bool wasPaused;

//	private void ApplyAudioToTargets()
//	{
//		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
//		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
//		this.audioSource.volume = this.vector2s[2].x;
//		this.audioSource.pitch = this.vector2s[2].y;
//		if (this.percentage == 1f)
//		{
//			this.audioSource.volume = this.vector2s[1].x;
//			this.audioSource.pitch = this.vector2s[1].y;
//		}
//	}

//	private void ApplyColorTargets()
//	{
//		this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
//		this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
//		this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
//		this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
//		this.tweenArguments["onupdateparams"] = (this.colors[0, 2]);
//		if (this.percentage == 1f)
//		{
//			this.tweenArguments["onupdateparams"] = (this.colors[0, 1]);
//		}
//	}

//	private void ApplyColorToTargets()
//	{
//		int num;
//		for (num = 0; num < this.colors.GetLength(0); num++)
//		{
//			this.colors[num, 2].r = this.ease(this.colors[num, 0].r, this.colors[num, 1].r, this.percentage);
//			this.colors[num, 2].g = this.ease(this.colors[num, 0].g, this.colors[num, 1].g, this.percentage);
//			this.colors[num, 2].b = this.ease(this.colors[num, 0].b, this.colors[num, 1].b, this.percentage);
//			this.colors[num, 2].a = this.ease(this.colors[num, 0].a, this.colors[num, 1].a, this.percentage);
//		}
//		if (base.GetComponent(typeof(GUITexture)) != null)
//		{
//			base.GetComponent<GUITexture>().color = (this.colors[0, 2]);
//		}
//		else if (base.GetComponent(typeof(GUIText)) != null)
//		{
//			base.GetComponent<GUIText>().material.color = (this.colors[0, 2]);
//		}
//		else if (base.GetComponent<Renderer>() != null)
//		{
//			for (num = 0; num < this.colors.GetLength(0); num++)
//			{
//				base.GetComponent<Renderer>().materials[num].SetColor(this.namedcolorvalue.ToString(), (this.colors[num, 2]));
//			}
//		}
//		else if (base.GetComponent<Light>() != null)
//		{
//			base.GetComponent<Light>().color = (this.colors[0, 2]);
//		}
//		if (this.percentage == 1f)
//		{
//			if (base.GetComponent(typeof(GUITexture)) != null)
//			{
//				base.GetComponent<GUITexture>().color = (this.colors[0, 1]);
//			}
//			else if (base.GetComponent(typeof(GUIText)) != null)
//			{
//				base.GetComponent<GUIText>().material.color = (this.colors[0, 1]);
//			}
//			else if (base.GetComponent<Renderer>() != null)
//			{
//				for (num = 0; num < this.colors.GetLength(0); num++)
//				{
//					base.GetComponent<Renderer>().materials[num].SetColor(this.namedcolorvalue.ToString(), (this.colors[num, 1]));
//				}
//			}
//			else if (base.GetComponent<Light>() != null)
//			{
//				base.GetComponent<Light>().color = (this.colors[0, 1]);
//			}
//		}
//	}

//	private void ApplyFloatTargets()
//	{
//		this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
//		this.tweenArguments["onupdateparams"] = this.floats[2];
//		if (this.percentage == 1f)
//		{
//			this.tweenArguments["onupdateparams"] = this.floats[1];
//		}
//	}

//	private void ApplyLookToTargets()
//	{
//		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
//		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
//		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
//		if (this.isLocal)
//		{
//			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
//		}
//		else
//		{
//			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
//		}
//	}

//	private void ApplyMoveByTargets()
//	{
//		this.preUpdate = base.transform.position;
//		Vector3 eulerAngles = new Vector3();
//		if (this.tweenArguments.Contains("looktarget"))
//		{
//			eulerAngles = base.transform.eulerAngles;
//			base.transform.eulerAngles = this.vector3s[4];
//		}
//		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
//		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
//		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
//		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
//		this.vector3s[3] = this.vector3s[2];
//		if (this.tweenArguments.Contains("looktarget"))
//		{
//			base.transform.eulerAngles = eulerAngles;
//		}
//		this.postUpdate = base.transform.position;
//		if (this.physics)
//		{
//			base.transform.position = this.preUpdate;
//			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
//		}
//	}

//	private void ApplyMoveToPathTargets()
//	{
//		this.preUpdate = base.transform.position;
//		float num = this.ease(0f, 1f, this.percentage);
//		if (this.isLocal)
//		{
//			base.transform.localPosition = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
//		}
//		else
//		{
//			base.transform.position = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
//		}
//		if (this.tweenArguments.Contains("orienttopath") && ((bool) this.tweenArguments["orienttopath"]))
//		{
//			float lookAhead;
//			if (this.tweenArguments.Contains("lookahead"))
//			{
//				lookAhead = (float) this.tweenArguments["lookahead"];
//			}
//			else
//			{
//				lookAhead = Defaults.lookAhead;
//			}
//			float num3 = this.ease(0f, 1f, Mathf.Min((float) 1f, (float) (this.percentage + lookAhead)));
//			this.tweenArguments["looktarget"] = this.path.Interp(Mathf.Clamp(num3, 0f, 1f));
//		}
//		this.postUpdate = base.transform.position;
//		if (this.physics)
//		{
//			base.transform.position = this.preUpdate;
//			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
//		}
//	}

//	private void ApplyMoveToTargets()
//	{
//		this.preUpdate = base.transform.position;
//		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
//		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
//		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
//		if (this.isLocal)
//		{
//			base.transform.localPosition = this.vector3s[2];
//		}
//		else
//		{
//			base.transform.position = this.vector3s[2];
//		}
//		if (this.percentage == 1f)
//		{
//			if (this.isLocal)
//			{
//				base.transform.localPosition = this.vector3s[1];
//			}
//			else
//			{
//				base.transform.position = this.vector3s[1];
//			}
//		}
//		this.postUpdate = base.transform.position;
//		if (this.physics)
//		{
//			base.transform.position = this.preUpdate;
//			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
//		}
//	}

//	private void ApplyPunchPositionTargets()
//	{
//		this.preUpdate = base.transform.position;
//		Vector3 eulerAngles = new Vector3();
//		if (this.tweenArguments.Contains("looktarget"))
//		{
//			eulerAngles = base.transform.eulerAngles;
//			base.transform.eulerAngles = this.vector3s[4];
//		}
//		if (this.vector3s[1].x > 0f)
//		{
//			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
//		}
//		else if (this.vector3s[1].x < 0f)
//		{
//			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
//		}
//		if (this.vector3s[1].y > 0f)
//		{
//			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
//		}
//		else if (this.vector3s[1].y < 0f)
//		{
//			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
//		}
//		if (this.vector3s[1].z > 0f)
//		{
//			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
//		}
//		else if (this.vector3s[1].z < 0f)
//		{
//			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
//		}
//		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
//		this.vector3s[3] = this.vector3s[2];
//		if (this.tweenArguments.Contains("looktarget"))
//		{
//			base.transform.eulerAngles = eulerAngles;
//		}
//		this.postUpdate = base.transform.position;
//		if (this.physics)
//		{
//			base.transform.position = this.preUpdate;
//			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
//		}
//	}

//	private void ApplyPunchRotationTargets()
//	{
//		this.preUpdate = base.transform.eulerAngles;
//		if (this.vector3s[1].x > 0f)
//		{
//			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
//		}
//		else if (this.vector3s[1].x < 0f)
//		{
//			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
//		}
//		if (this.vector3s[1].y > 0f)
//		{
//			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
//		}
//		else if (this.vector3s[1].y < 0f)
//		{
//			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
//		}
//		if (this.vector3s[1].z > 0f)
//		{
//			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
//		}
//		else if (this.vector3s[1].z < 0f)
//		{
//			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
//		}
//		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
//		this.vector3s[3] = this.vector3s[2];
//		this.postUpdate = base.transform.eulerAngles;
//		if (this.physics)
//		{
//			base.transform.eulerAngles = this.preUpdate;
//			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
//		}
//	}

//	private void ApplyPunchScaleTargets()
//	{
//		if (this.vector3s[1].x > 0f)
//		{
//			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
//		}
//		else if (this.vector3s[1].x < 0f)
//		{
//			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
//		}
//		if (this.vector3s[1].y > 0f)
//		{
//			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
//		}
//		else if (this.vector3s[1].y < 0f)
//		{
//			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
//		}
//		if (this.vector3s[1].z > 0f)
//		{
//			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
//		}
//		else if (this.vector3s[1].z < 0f)
//		{
//			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
//		}
//		base.transform.localScale = this.vector3s[0] + this.vector3s[2];
//	}

//	private void ApplyRectTargets()
//	{
//		this.rects[2].x = this.ease(this.rects[0].x, this.rects[1].x, this.percentage);
//		this.rects[2].y = this.ease(this.rects[0].y, this.rects[1].y, this.percentage);
//		this.rects[2].width = this.ease(this.rects[0].width, this.rects[1].width, this.percentage);
//		this.rects[2].height = this.ease(this.rects[0].height, this.rects[1].height, this.percentage);
//		this.tweenArguments["onupdateparams"] = this.rects[2];
//		if (this.percentage == 1f)
//		{
//			this.tweenArguments["onupdateparams"] = this.rects[1];
//		}
//	}

//	private void ApplyRotateAddTargets()
//	{
//		this.preUpdate = base.transform.eulerAngles;
//		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
//		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
//		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
//		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
//		this.vector3s[3] = this.vector3s[2];
//		this.postUpdate = base.transform.eulerAngles;
//		if (this.physics)
//		{
//			base.transform.eulerAngles = this.preUpdate;
//			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
//		}
//	}

//	private void ApplyRotateToTargets()
//	{
//		this.preUpdate = base.transform.eulerAngles;
//		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
//		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
//		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
//		if (this.isLocal)
//		{
//			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
//		}
//		else
//		{
//			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
//		}
//		if (this.percentage == 1f)
//		{
//			if (this.isLocal)
//			{
//				base.transform.localRotation = Quaternion.Euler(this.vector3s[1]);
//			}
//			else
//			{
//				base.transform.rotation = Quaternion.Euler(this.vector3s[1]);
//			}
//		}
//		this.postUpdate = base.transform.eulerAngles;
//		if (this.physics)
//		{
//			base.transform.eulerAngles = this.preUpdate;
//			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
//		}
//	}

//	private void ApplyScaleToTargets()
//	{
//		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
//		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
//		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
//		base.transform.localScale = this.vector3s[2];
//		if (this.percentage == 1f)
//		{
//			base.transform.localScale = this.vector3s[1];
//		}
//	}

//	private void ApplyShakePositionTargets()
//	{
//		if (this.isLocal)
//		{
//			this.preUpdate = base.transform.localPosition;
//		}
//		else
//		{
//			this.preUpdate = base.transform.position;
//		}
//		Vector3 eulerAngles = new Vector3();
//		if (this.tweenArguments.Contains("looktarget"))
//		{
//			eulerAngles = base.transform.eulerAngles;
//			base.transform.eulerAngles = this.vector3s[3];
//		}
//		if (this.percentage == 0f)
//		{
//			base.transform.Translate(this.vector3s[1], this.space);
//		}
//		if (this.isLocal)
//		{
//			base.transform.localPosition = this.vector3s[0];
//		}
//		else
//		{
//			base.transform.position = this.vector3s[0];
//		}
//		float num = 1f - this.percentage;
//		this.vector3s[2].x = UnityEngine.Random.Range((float) (-this.vector3s[1].x * num), (float) (this.vector3s[1].x * num));
//		this.vector3s[2].y = UnityEngine.Random.Range((float) (-this.vector3s[1].y * num), (float) (this.vector3s[1].y * num));
//		this.vector3s[2].z = UnityEngine.Random.Range((float) (-this.vector3s[1].z * num), (float) (this.vector3s[1].z * num));
//		if (this.isLocal)
//		{
//			Transform transform = base.transform;
//			transform.localPosition += this.vector3s[2];
//		}
//		else
//		{
//			Transform transform2 = base.transform;
//			transform2.position += this.vector3s[2];
//		}
//		if (this.tweenArguments.Contains("looktarget"))
//		{
//			base.transform.eulerAngles = eulerAngles;
//		}
//		this.postUpdate = base.transform.position;
//		if (this.physics)
//		{
//			base.transform.position = this.preUpdate;
//			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
//		}
//	}

//	private void ApplyShakeRotationTargets()
//	{
//		this.preUpdate = base.transform.eulerAngles;
//		if (this.percentage == 0f)
//		{
//			base.transform.Rotate(this.vector3s[1], this.space);
//		}
//		base.transform.eulerAngles = this.vector3s[0];
//		float num = 1f - this.percentage;
//		this.vector3s[2].x = UnityEngine.Random.Range((float) (-this.vector3s[1].x * num), (float) (this.vector3s[1].x * num));
//		this.vector3s[2].y = UnityEngine.Random.Range((float) (-this.vector3s[1].y * num), (float) (this.vector3s[1].y * num));
//		this.vector3s[2].z = UnityEngine.Random.Range((float) (-this.vector3s[1].z * num), (float) (this.vector3s[1].z * num));
//		base.transform.Rotate(this.vector3s[2], this.space);
//		this.postUpdate = base.transform.eulerAngles;
//		if (this.physics)
//		{
//			base.transform.eulerAngles = this.preUpdate;
//			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
//		}
//	}

//	private void ApplyShakeScaleTargets()
//	{
//		if (this.percentage == 0f)
//		{
//			base.transform.localScale = this.vector3s[1];
//		}
//		base.transform.localScale = this.vector3s[0];
//		float num = 1f - this.percentage;
//		this.vector3s[2].x = UnityEngine.Random.Range((float) (-this.vector3s[1].x * num), (float) (this.vector3s[1].x * num));
//		this.vector3s[2].y = UnityEngine.Random.Range((float) (-this.vector3s[1].y * num), (float) (this.vector3s[1].y * num));
//		this.vector3s[2].z = UnityEngine.Random.Range((float) (-this.vector3s[1].z * num), (float) (this.vector3s[1].z * num));
//		Transform transform = base.transform;
//		transform.localScale += this.vector3s[2];
//	}

//	private void ApplyStabTargets()
//	{
//	}

//	private void ApplyVector2Targets()
//	{
//		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
//		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
//		this.tweenArguments["onupdateparams"] = this.vector2s[2];
//		if (this.percentage == 1f)
//		{
//			this.tweenArguments["onupdateparams"] = this.vector2s[1];
//		}
//	}

//	private void ApplyVector3Targets()
//	{
//		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
//		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
//		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
//		this.tweenArguments["onupdateparams"] = this.vector3s[2];
//		if (this.percentage == 1f)
//		{
//			this.tweenArguments["onupdateparams"] = this.vector3s[1];
//		}
//	}

//	public static void AudioFrom(GameObject target, Hashtable args)
//	{
//		Vector2 vector;
//		Vector2 vector2;
//		AudioSource component;
//		args = CleanArgs(args);
//		if (args.Contains("audiosource"))
//		{
//			component = (AudioSource) args["audiosource"];
//		}
//		else if (target.GetComponent(typeof(AudioSource)) != null)
//		{
//			component = target.GetComponent<AudioSource>();
//		}
//		else
//		{
//			////Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
//			return;
//		}
//		vector.x = vector2.x = component.volume;
//		vector.y = vector2.y = component.pitch;
//		if (args.Contains("volume"))
//		{
//			vector2.x = (float) args["volume"];
//		}
//		if (args.Contains("pitch"))
//		{
//			vector2.y = (float) args["pitch"];
//		}
//		component.volume = vector2.x;
//		component.pitch = vector2.y;
//		args["volume"] = vector.x;
//		args["pitch"] = vector.y;
//		if (!args.Contains("easetype"))
//		{
//			args.Add("easetype", EaseType.linear);
//		}
//		args["type"] = "audio";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
//	{
//		AudioFrom(target, Hash(new object[] { "volume", volume, "pitch", pitch, "time", time }));
//	}

//	public static void AudioTo(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		if (!args.Contains("easetype"))
//		{
//			args.Add("easetype", EaseType.linear);
//		}
//		args["type"] = "audio";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void AudioTo(GameObject target, float volume, float pitch, float time)
//	{
//		AudioTo(target, Hash(new object[] { "volume", volume, "pitch", pitch, "time", time }));
//	}

//	public static void AudioUpdate(GameObject target, Hashtable args)
//	{
//		AudioSource component;
//		float updateTime;
//		CleanArgs(args);
//		Vector2[] vectorArray = new Vector2[4];
//		if (args.Contains("time"))
//		{
//			updateTime = (float) args["time"];
//			updateTime *= Defaults.updateTimePercentage;
//		}
//		else
//		{
//			updateTime = Defaults.updateTime;
//		}
//		if (args.Contains("audiosource"))
//		{
//			component = (AudioSource) args["audiosource"];
//		}
//		else if (target.GetComponent(typeof(AudioSource)) != null)
//		{
//			component = target.GetComponent<AudioSource>();
//		}
//		else
//		{
//			////Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
//			return;
//		}
//		vectorArray[0] = vectorArray[1] = new Vector2(component.volume, component.pitch);
//		if (args.Contains("volume"))
//		{
//			vectorArray[1].x = (float) args["volume"];
//		}
//		if (args.Contains("pitch"))
//		{
//			vectorArray[1].y = (float) args["pitch"];
//		}
//		vectorArray[3].x = Mathf.SmoothDampAngle(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
//		vectorArray[3].y = Mathf.SmoothDampAngle(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
//		component.volume = vectorArray[3].x;
//		component.pitch = vectorArray[3].y;
//	}

//	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
//	{
//		AudioUpdate(target, Hash(new object[] { "volume", volume, "pitch", pitch, "time", time }));
//	}

//	private void Awake()
//	{
//		this.RetrieveArgs();
//		this.lastRealTime = Time.realtimeSinceStartup;
//	}

//	private void CallBack(string callbackType)
//	{
//		if (this.tweenArguments.Contains(callbackType) && !this.tweenArguments.Contains("ischild"))
//		{
//			GameObject gameObject;
//			if (this.tweenArguments.Contains(callbackType + "target"))
//			{
//				gameObject = (GameObject) this.tweenArguments[callbackType + "target"];
//			}
//			else
//			{
//				gameObject = base.gameObject;
//			}
//			if (this.tweenArguments[callbackType].GetType() == typeof(string))
//			{
//				gameObject.SendMessage((string) this.tweenArguments[callbackType], this.tweenArguments[callbackType + "params"], SendMessageOptions.DontRequireReceiver);
//			}
//			else
//			{
//				//Debug.LogError("iTween Error: Callback method references must be passed as a String!");
//				UnityEngine.Object.Destroy(this);
//			}
//		}
//	}

//	public static GameObject CameraFadeAdd()
//	{
//		if (cameraFade != null)
//		{
//			return null;
//		}
//		cameraFade = new GameObject("iTween Camera Fade");
//		cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float) Defaults.cameraFadeDepth);
//		cameraFade.AddComponent<GUITexture>();
//		cameraFade.GetComponent<GUITexture>().texture = CameraTexture(Color.black);
//		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
//		return cameraFade;
//	}

//	public static GameObject CameraFadeAdd(Texture2D texture)
//	{
//		if (cameraFade != null)
//		{
//			return null;
//		}
//		cameraFade = new GameObject("iTween Camera Fade");
//		cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float) Defaults.cameraFadeDepth);
//		cameraFade.AddComponent<GUITexture>();
//		cameraFade.GetComponent<GUITexture>().texture = texture;
//		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
//		return cameraFade;
//	}

//	public static GameObject CameraFadeAdd(Texture2D texture, int depth)
//	{
//		if (cameraFade != null)
//		{
//			return null;
//		}
//		cameraFade = new GameObject("iTween Camera Fade");
//		cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float) depth);
//		cameraFade.AddComponent<GUITexture>();
//		cameraFade.GetComponent<GUITexture>().texture = texture;
//		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
//		return cameraFade;
//	}

//	public static void CameraFadeDepth(int depth)
//	{
//		if (cameraFade != null)
//		{
//			cameraFade.transform.position = new Vector3(cameraFade.transform.position.x, cameraFade.transform.position.y, (float) depth);
//		}
//	}

//	public static void CameraFadeDestroy()
//	{
//		if (cameraFade != null)
//		{
//			UnityEngine.Object.Destroy(cameraFade);
//		}
//	}

//	public static void CameraFadeFrom(Hashtable args)
//	{
//		if (cameraFade != null)
//		{
//			ColorFrom(cameraFade, args);
//		}
//		else
//		{
//			//Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
//		}
//	}

//	public static void CameraFadeFrom(float amount, float time)
//	{
//		if (cameraFade != null)
//		{
//			CameraFadeFrom(Hash(new object[] { "amount", amount, "time", time }));
//		}
//		else
//		{
//			//Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
//		}
//	}

//	public static void CameraFadeSwap(Texture2D texture)
//	{
//		if (cameraFade != null)
//		{
//			cameraFade.GetComponent<GUITexture>().texture = texture;
//		}
//	}

//	public static void CameraFadeTo(Hashtable args)
//	{
//		if (cameraFade != null)
//		{
//			ColorTo(cameraFade, args);
//		}
//		else
//		{
//			//Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
//		}
//	}

//	public static void CameraFadeTo(float amount, float time)
//	{
//		if (cameraFade != null)
//		{
//			CameraFadeTo(Hash(new object[] { "amount", amount, "time", time }));
//		}
//		else
//		{
//			//Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
//		}
//	}

//	public static Texture2D CameraTexture(Color color)
//	{
//		Texture2D textured = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
//		Color[] colors = new Color[Screen.width * Screen.height];
//		for (int i = 0; i < colors.Length; i++)
//		{
//			colors[i] = color;
//		}
//		textured.SetPixels(colors);
//		textured.Apply();
//		return textured;
//	}

//	private static Hashtable CleanArgs(Hashtable args)
//	{
//		Hashtable hashtable = new Hashtable(args.Count);
//		Hashtable hashtable2 = new Hashtable(args.Count);
//		foreach (DictionaryEntry entry in args)
//		{
//			hashtable.Add(entry.Key, entry.Value);
//		}
//		foreach (DictionaryEntry entry in hashtable)
//		{
//			float num2;
//			if (entry.Value.GetType() == typeof(int))
//			{
//				int num = (int) entry.Value;
//				num2 = num;
//				args[entry.Key] = num2;
//			}
//			if (entry.Value.GetType() == typeof(double))
//			{
//				double num3 = (double) entry.Value;
//				num2 = (float) num3;
//				args[entry.Key] = num2;
//			}
//		}
//		foreach (DictionaryEntry entry in args)
//		{
//			string key = entry.Key.ToString().ToLower();
//			hashtable2.Add(key, entry.Value);
//		}
//		args = hashtable2;
//		return args;
//	}

//	private float clerp(float start, float end, float value)
//	{
//		float num = 0f;
//		float num2 = 360f;
//		float num3 = Mathf.Abs((float) ((num2 - num) / 2f));
//		float num5 = 0f;
//		if ((end - start) < -num3)
//		{
//			num5 = ((num2 - start) + end) * value;
//			return (start + num5);
//		}
//		if ((end - start) > num3)
//		{
//			num5 = -((num2 - end) + start) * value;
//			return (start + num5);
//		}
//		return (start + ((end - start) * value));
//	}

//	public static void ColorFrom(GameObject target, Hashtable args)
//	{
//		Color color = new Color();
//		Color color2 = new Color();
//		args = CleanArgs(args);
//		if (!args.Contains("includechildren") || ((bool) args["includechildren"]))
//		{
//			foreach (Transform transform in target.transform)
//			{
//				Hashtable hashtable = (Hashtable) args.Clone();
//				hashtable["ischild"] = true;
//				ColorFrom(transform.gameObject, hashtable);
//			}
//		}
//		if (!args.Contains("easetype"))
//		{
//			args.Add("easetype", EaseType.linear);
//		}
//		if (target.GetComponent(typeof(GUITexture)) != null)
//		{
//			color2 = color = target.GetComponent<GUITexture>().color;
//		}
//		else if (target.GetComponent(typeof(GUIText)) != null)
//		{
//			color2 = color = target.GetComponent<GUIText>().material.color;
//		}
//		else if (target.GetComponent<Renderer>() != null)
//		{
//			color2 = color = target.GetComponent<Renderer>().material.color;
//		}
//		else if (target.GetComponent<Light>() != null)
//		{
//			color2 = color = target.GetComponent<Light>().color;
//		}
//		if (args.Contains("color"))
//		{
//			color = (Color) args["color"];
//		}
//		else
//		{
//			if (args.Contains("r"))
//			{
//				color.r = (float) args["r"];
//			}
//			if (args.Contains("g"))
//			{
//				color.g = (float) args["g"];
//			}
//			if (args.Contains("b"))
//			{
//				color.b = (float) args["b"];
//			}
//			if (args.Contains("a"))
//			{
//				color.a = (float) args["a"];
//			}
//		}
//		if (args.Contains("amount"))
//		{
//			color.a = (float) args["amount"];
//			args.Remove("amount");
//		}
//		else if (args.Contains("alpha"))
//		{
//			color.a = (float) args["alpha"];
//			args.Remove("alpha");
//		}
//		if (target.GetComponent(typeof(GUITexture)) != null)
//		{
//			target.GetComponent<GUITexture>().color = color;
//		}
//		else if (target.GetComponent(typeof(GUIText)) != null)
//		{
//			target.GetComponent<GUIText>().material.color = color;
//		}
//		else if (target.GetComponent<Renderer>() != null)
//		{
//			target.GetComponent<Renderer>().material.color = color;
//		}
//		else if (target.GetComponent<Light>() != null)
//		{
//			target.GetComponent<Light>().color = color;
//		}
//		args["color"] = color2;
//		args["type"] = "color";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void ColorFrom(GameObject target, Color color, float time)
//	{
//		ColorFrom(target, Hash(new object[] { "color", color, "time", time }));
//	}

//	public static void ColorTo(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		if (!args.Contains("includechildren") || ((bool) args["includechildren"]))
//		{
//			foreach (Transform transform in target.transform)
//			{
//				Hashtable hashtable = (Hashtable) args.Clone();
//				hashtable["ischild"] = true;
//				ColorTo(transform.gameObject, hashtable);
//			}
//		}
//		if (!args.Contains("easetype"))
//		{
//			args.Add("easetype", EaseType.linear);
//		}
//		args["type"] = "color";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void ColorTo(GameObject target, Color color, float time)
//	{
//		ColorTo(target, Hash(new object[] { "color", color, "time", time }));
//	}

//	public static void ColorUpdate(GameObject target, Hashtable args)
//	{
//		float updateTime;
//		CleanArgs(args);
//		Color[] colorArray = new Color[4];
//		if (!args.Contains("includechildren") || ((bool) args["includechildren"]))
//		{
//			foreach (Transform transform in target.transform)
//			{
//				ColorUpdate(transform.gameObject, args);
//			}
//		}
//		if (args.Contains("time"))
//		{
//			updateTime = (float) args["time"];
//			updateTime *= Defaults.updateTimePercentage;
//		}
//		else
//		{
//			updateTime = Defaults.updateTime;
//		}
//		if (target.GetComponent(typeof(GUITexture)) != null)
//		{
//			colorArray[0] = colorArray[1] = target.GetComponent<GUITexture>().color;
//		}
//		else if (target.GetComponent(typeof(GUIText)) != null)
//		{
//			colorArray[0] = colorArray[1] = target.GetComponent<GUIText>().material.color;
//		}
//		else if (target.GetComponent<Renderer>() != null)
//		{
//			colorArray[0] = colorArray[1] = target.GetComponent<Renderer>().material.color;
//		}
//		else if (target.GetComponent<Light>() != null)
//		{
//			colorArray[0] = colorArray[1] = target.GetComponent<Light>().color;
//		}
//		if (args.Contains("color"))
//		{
//			colorArray[1] = (Color) args["color"];
//		}
//		else
//		{
//			if (args.Contains("r"))
//			{
//				colorArray[1].r = (float) args["r"];
//			}
//			if (args.Contains("g"))
//			{
//				colorArray[1].g = (float) args["g"];
//			}
//			if (args.Contains("b"))
//			{
//				colorArray[1].b = (float) args["b"];
//			}
//			if (args.Contains("a"))
//			{
//				colorArray[1].a = (float) args["a"];
//			}
//		}
//		colorArray[3].r = Mathf.SmoothDamp(colorArray[0].r, colorArray[1].r, ref colorArray[2].r, updateTime);
//		colorArray[3].g = Mathf.SmoothDamp(colorArray[0].g, colorArray[1].g, ref colorArray[2].g, updateTime);
//		colorArray[3].b = Mathf.SmoothDamp(colorArray[0].b, colorArray[1].b, ref colorArray[2].b, updateTime);
//		colorArray[3].a = Mathf.SmoothDamp(colorArray[0].a, colorArray[1].a, ref colorArray[2].a, updateTime);
//		if (target.GetComponent(typeof(GUITexture)) != null)
//		{
//			target.GetComponent<GUITexture>().color = colorArray[3];
//		}
//		else if (target.GetComponent(typeof(GUIText)) != null)
//		{
//			target.GetComponent<GUIText>().material.color = colorArray[3];
//		}
//		else if (target.GetComponent<Renderer>() != null)
//		{
//			target.GetComponent<Renderer>().material.color = colorArray[3];
//		}
//		else if (target.GetComponent<Light>() != null)
//		{
//			target.GetComponent<Light>().color = colorArray[3];
//		}
//	}

//	public static void ColorUpdate(GameObject target, Color color, float time)
//	{
//		ColorUpdate(target, Hash(new object[] { "color", color, "time", time }));
//	}

//	private void ConflictCheck()
//	{
//		Component[] components = base.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if (tween.type == "value")
//			{
//				break;
//			}
//			if (tween.isRunning && (tween.type == this.type))
//			{
//				if (tween.method != this.method)
//				{
//					break;
//				}
//				if (tween.tweenArguments.Count != this.tweenArguments.Count)
//				{
//					tween.Dispose();
//					break;
//				}
//				foreach (DictionaryEntry entry in this.tweenArguments)
//				{
//					if (!tween.tweenArguments.Contains(entry.Key))
//					{
//						tween.Dispose();
//						break;
//					}
//					if (!(tween.tweenArguments[entry.Key].Equals(this.tweenArguments[entry.Key]) || !(((string) entry.Key) != "id")))
//					{
//						tween.Dispose();
//						break;
//					}
//				}
//				this.Dispose();
//			}
//		}
//	}

//	public static int Count()
//	{
//		return tweens.Count;
//	}

//	public static int Count(string type)
//	{
//		int num = 0;
//		for (int i = 0; i < tweens.Count; i++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[i];
//			if ((((string) hashtable["type"]) + ((string) hashtable["method"])).Substring(0, type.Length).ToLower() == type.ToLower())
//			{
//				num++;
//			}
//		}
//		return num;
//	}

//	public static int Count(GameObject target)
//	{
//		return target.GetComponents(typeof(iTween)).Length;
//	}

//	public static int Count(GameObject target, string type)
//	{
//		int num = 0;
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
//			{
//				num++;
//			}
//		}
//		return num;
//	}

//	private void DisableKinematic()
//	{
//	}

//	private void Dispose()
//	{
//		for (int i = 0; i < tweens.Count; i++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[i];
//			if (((string) hashtable["id"]) == this.id)
//			{
//				tweens.RemoveAt(i);
//				break;
//			}
//		}
//		UnityEngine.Object.Destroy(this);
//	}

//	public static void DrawLine(Transform[] line)
//	{
//		if (line.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[line.Length];
//			for (int i = 0; i < line.Length; i++)
//			{
//				vectorArray[i] = line[i].position;
//			}
//			DrawLineHelper(vectorArray, Defaults.color, "gizmos");
//		}
//	}

//	public static void DrawLine(Vector3[] line)
//	{
//		if (line.Length > 0)
//		{
//			DrawLineHelper(line, Defaults.color, "gizmos");
//		}
//	}

//	public static void DrawLine(Transform[] line, Color color)
//	{
//		if (line.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[line.Length];
//			for (int i = 0; i < line.Length; i++)
//			{
//				vectorArray[i] = line[i].position;
//			}
//			DrawLineHelper(vectorArray, color, "gizmos");
//		}
//	}

//	public static void DrawLine(Vector3[] line, Color color)
//	{
//		if (line.Length > 0)
//		{
//			DrawLineHelper(line, color, "gizmos");
//		}
//	}

//	public static void DrawLineGizmos(Transform[] line)
//	{
//		if (line.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[line.Length];
//			for (int i = 0; i < line.Length; i++)
//			{
//				vectorArray[i] = line[i].position;
//			}
//			DrawLineHelper(vectorArray, Defaults.color, "gizmos");
//		}
//	}

//	public static void DrawLineGizmos(Vector3[] line)
//	{
//		if (line.Length > 0)
//		{
//			DrawLineHelper(line, Defaults.color, "gizmos");
//		}
//	}

//	public static void DrawLineGizmos(Transform[] line, Color color)
//	{
//		if (line.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[line.Length];
//			for (int i = 0; i < line.Length; i++)
//			{
//				vectorArray[i] = line[i].position;
//			}
//			DrawLineHelper(vectorArray, color, "gizmos");
//		}
//	}

//	public static void DrawLineGizmos(Vector3[] line, Color color)
//	{
//		if (line.Length > 0)
//		{
//			DrawLineHelper(line, color, "gizmos");
//		}
//	}

//	public static void DrawLineHandles(Transform[] line)
//	{
//		if (line.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[line.Length];
//			for (int i = 0; i < line.Length; i++)
//			{
//				vectorArray[i] = line[i].position;
//			}
//			DrawLineHelper(vectorArray, Defaults.color, "handles");
//		}
//	}

//	public static void DrawLineHandles(Vector3[] line)
//	{
//		if (line.Length > 0)
//		{
//			DrawLineHelper(line, Defaults.color, "handles");
//		}
//	}

//	public static void DrawLineHandles(Transform[] line, Color color)
//	{
//		if (line.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[line.Length];
//			for (int i = 0; i < line.Length; i++)
//			{
//				vectorArray[i] = line[i].position;
//			}
//			DrawLineHelper(vectorArray, color, "handles");
//		}
//	}

//	public static void DrawLineHandles(Vector3[] line, Color color)
//	{
//		if (line.Length > 0)
//		{
//			DrawLineHelper(line, color, "handles");
//		}
//	}

//	private static void DrawLineHelper(Vector3[] line, Color color, string method)
//	{
//		Gizmos.color = color;
//		for (int i = 0; i < (line.Length - 1); i++)
//		{
//			if (method == "gizmos")
//			{
//				Gizmos.DrawLine(line[i], line[i + 1]);
//			}
//			else if (method == "handles")
//			{
//				//Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
//			}
//		}
//	}

//	public static void DrawPath(Transform[] path)
//	{
//		if (path.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[path.Length];
//			for (int i = 0; i < path.Length; i++)
//			{
//				vectorArray[i] = path[i].position;
//			}
//			DrawPathHelper(vectorArray, Defaults.color, "gizmos");
//		}
//	}

//	public static void DrawPath(Vector3[] path)
//	{
//		if (path.Length > 0)
//		{
//			DrawPathHelper(path, Defaults.color, "gizmos");
//		}
//	}

//	public static void DrawPath(Transform[] path, Color color)
//	{
//		if (path.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[path.Length];
//			for (int i = 0; i < path.Length; i++)
//			{
//				vectorArray[i] = path[i].position;
//			}
//			DrawPathHelper(vectorArray, color, "gizmos");
//		}
//	}

//	public static void DrawPath(Vector3[] path, Color color)
//	{
//		if (path.Length > 0)
//		{
//			DrawPathHelper(path, color, "gizmos");
//		}
//	}

//	public static void DrawPathGizmos(Transform[] path)
//	{
//		if (path.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[path.Length];
//			for (int i = 0; i < path.Length; i++)
//			{
//				vectorArray[i] = path[i].position;
//			}
//			DrawPathHelper(vectorArray, Defaults.color, "gizmos");
//		}
//	}

//	public static void DrawPathGizmos(Vector3[] path)
//	{
//		if (path.Length > 0)
//		{
//			DrawPathHelper(path, Defaults.color, "gizmos");
//		}
//	}

//	public static void DrawPathGizmos(Transform[] path, Color color)
//	{
//		if (path.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[path.Length];
//			for (int i = 0; i < path.Length; i++)
//			{
//				vectorArray[i] = path[i].position;
//			}
//			DrawPathHelper(vectorArray, color, "gizmos");
//		}
//	}

//	public static void DrawPathGizmos(Vector3[] path, Color color)
//	{
//		if (path.Length > 0)
//		{
//			DrawPathHelper(path, color, "gizmos");
//		}
//	}

//	public static void DrawPathHandles(Transform[] path)
//	{
//		if (path.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[path.Length];
//			for (int i = 0; i < path.Length; i++)
//			{
//				vectorArray[i] = path[i].position;
//			}
//			DrawPathHelper(vectorArray, Defaults.color, "handles");
//		}
//	}

//	public static void DrawPathHandles(Vector3[] path)
//	{
//		if (path.Length > 0)
//		{
//			DrawPathHelper(path, Defaults.color, "handles");
//		}
//	}

//	public static void DrawPathHandles(Transform[] path, Color color)
//	{
//		if (path.Length > 0)
//		{
//			Vector3[] vectorArray = new Vector3[path.Length];
//			for (int i = 0; i < path.Length; i++)
//			{
//				vectorArray[i] = path[i].position;
//			}
//			DrawPathHelper(vectorArray, color, "handles");
//		}
//	}

//	public static void DrawPathHandles(Vector3[] path, Color color)
//	{
//		if (path.Length > 0)
//		{
//			DrawPathHelper(path, color, "handles");
//		}
//	}

//	private static void DrawPathHelper(Vector3[] path, Color color, string method)
//	{
//		Vector3[] pts = PathControlPointGenerator(path);
//		Vector3 to = Interp(pts, 0f);
//		Gizmos.color = color;
//		int num = path.Length * 20;
//		for (int i = 1; i <= num; i++)
//		{
//			float t = ((float) i) / ((float) num);
//			Vector3 from = Interp(pts, t);
//			if (method == "gizmos")
//			{
//				Gizmos.DrawLine(from, to);
//			}
//			else if (method == "handles")
//			{
//				//Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
//			}
//			to = from;
//		}
//	}

//	private float easeInBack(float start, float end, float value)
//	{
//		end -= start;
//		value /= 1f;
//		float num = 1.70158f;
//		return ((((end * value) * value) * (((num + 1f) * value) - num)) + start);
//	}

//	private float easeInBounce(float start, float end, float value)
//	{
//		end -= start;
//		float num = 1f;
//		return ((end - this.easeOutBounce(0f, end, num - value)) + start);
//	}

//	private float easeInCirc(float start, float end, float value)
//	{
//		end -= start;
//		return ((-end * (Mathf.Sqrt(1f - (value * value)) - 1f)) + start);
//	}

//	private float easeInCubic(float start, float end, float value)
//	{
//		end -= start;
//		return ((((end * value) * value) * value) + start);
//	}

//	private float easeInElastic(float start, float end, float value)
//	{
//		end -= start;
//		float num = 1f;
//		float num2 = num * 0.3f;
//		float num3 = 0f;
//		float num4 = 0f;
//		if (value == 0f)
//		{
//			return start;
//		}
//		if ((value /= num) == 1f)
//		{
//			return (start + end);
//		}
//		if ((num4 == 0f) || (num4 < Mathf.Abs(end)))
//		{
//			num4 = end;
//			num3 = num2 / 4f;
//		}
//		else
//		{
//			num3 = (num2 / 6.283185f) * Mathf.Asin(end / num4);
//		}
//		return (-((num4 * Mathf.Pow(2f, 10f * --value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2)) + start);
//	}

//	private float easeInExpo(float start, float end, float value)
//	{
//		end -= start;
//		return ((end * Mathf.Pow(2f, 10f * ((value / 1f) - 1f))) + start);
//	}

//	private float easeInOutBack(float start, float end, float value)
//	{
//		float num = 1.70158f;
//		end -= start;
//		value /= 0.5f;
//		if (value < 1f)
//		{
//			num *= 1.525f;
//			return (((end / 2f) * ((value * value) * (((num + 1f) * value) - num))) + start);
//		}
//		value -= 2f;
//		num *= 1.525f;
//		return (((end / 2f) * (((value * value) * (((num + 1f) * value) + num)) + 2f)) + start);
//	}

//	private float easeInOutBounce(float start, float end, float value)
//	{
//		end -= start;
//		float num = 1f;
//		if (value < (num / 2f))
//		{
//			return ((this.easeInBounce(0f, end, value * 2f) * 0.5f) + start);
//		}
//		return (((this.easeOutBounce(0f, end, (value * 2f) - num) * 0.5f) + (end * 0.5f)) + start);
//	}

//	private float easeInOutCirc(float start, float end, float value)
//	{
//		value /= 0.5f;
//		end -= start;
//		if (value < 1f)
//		{
//			return (((-end / 2f) * (Mathf.Sqrt(1f - (value * value)) - 1f)) + start);
//		}
//		value -= 2f;
//		return (((end / 2f) * (Mathf.Sqrt(1f - (value * value)) + 1f)) + start);
//	}

//	private float easeInOutCubic(float start, float end, float value)
//	{
//		value /= 0.5f;
//		end -= start;
//		if (value < 1f)
//		{
//			return (((((end / 2f) * value) * value) * value) + start);
//		}
//		value -= 2f;
//		return (((end / 2f) * (((value * value) * value) + 2f)) + start);
//	}

//	private float easeInOutElastic(float start, float end, float value)
//	{
//		end -= start;
//		float num = 1f;
//		float num2 = num * 0.3f;
//		float num3 = 0f;
//		float num4 = 0f;
//		if (value == 0f)
//		{
//			return start;
//		}
//		if ((value /= (num / 2f)) == 2f)
//		{
//			return (start + end);
//		}
//		if ((num4 == 0f) || (num4 < Mathf.Abs(end)))
//		{
//			num4 = end;
//			num3 = num2 / 4f;
//		}
//		else
//		{
//			num3 = (num2 / 6.283185f) * Mathf.Asin(end / num4);
//		}
//		if (value < 1f)
//		{
//			return ((-0.5f * ((num4 * Mathf.Pow(2f, 10f * --value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2))) + start);
//		}
//		return (((((num4 * Mathf.Pow(2f, -10f * --value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2)) * 0.5f) + end) + start);
//	}

//	private float easeInOutExpo(float start, float end, float value)
//	{
//		value /= 0.5f;
//		end -= start;
//		if (value < 1f)
//		{
//			return (((end / 2f) * Mathf.Pow(2f, 10f * (value - 1f))) + start);
//		}
//		value--;
//		return (((end / 2f) * (-Mathf.Pow(2f, -10f * value) + 2f)) + start);
//	}

//	private float easeInOutQuad(float start, float end, float value)
//	{
//		value /= 0.5f;
//		end -= start;
//		if (value < 1f)
//		{
//			return ((((end / 2f) * value) * value) + start);
//		}
//		value--;
//		return (((-end / 2f) * ((value * (value - 2f)) - 1f)) + start);
//	}

//	private float easeInOutQuart(float start, float end, float value)
//	{
//		value /= 0.5f;
//		end -= start;
//		if (value < 1f)
//		{
//			return ((((((end / 2f) * value) * value) * value) * value) + start);
//		}
//		value -= 2f;
//		return (((-end / 2f) * ((((value * value) * value) * value) - 2f)) + start);
//	}

//	private float easeInOutQuint(float start, float end, float value)
//	{
//		value /= 0.5f;
//		end -= start;
//		if (value < 1f)
//		{
//			return (((((((end / 2f) * value) * value) * value) * value) * value) + start);
//		}
//		value -= 2f;
//		return (((end / 2f) * (((((value * value) * value) * value) * value) + 2f)) + start);
//	}

//	private float easeInOutSine(float start, float end, float value)
//	{
//		end -= start;
//		return (((-end / 2f) * (Mathf.Cos((3.141593f * value) / 1f) - 1f)) + start);
//	}

//	private float easeInQuad(float start, float end, float value)
//	{
//		end -= start;
//		return (((end * value) * value) + start);
//	}

//	private float easeInQuart(float start, float end, float value)
//	{
//		end -= start;
//		return (((((end * value) * value) * value) * value) + start);
//	}

//	private float easeInQuint(float start, float end, float value)
//	{
//		end -= start;
//		return ((((((end * value) * value) * value) * value) * value) + start);
//	}

//	private float easeInSine(float start, float end, float value)
//	{
//		end -= start;
//		return (((-end * Mathf.Cos((value / 1f) * 1.570796f)) + end) + start);
//	}

//	private float easeOutBack(float start, float end, float value)
//	{
//		float num = 1.70158f;
//		end -= start;
//		value = (value / 1f) - 1f;
//		return ((end * (((value * value) * (((num + 1f) * value) + num)) + 1f)) + start);
//	}

//	private float easeOutBounce(float start, float end, float value)
//	{
//		value /= 1f;
//		end -= start;
//		if (value < 0.3636364f)
//		{
//			return ((end * ((7.5625f * value) * value)) + start);
//		}
//		if (value < 0.7272727f)
//		{
//			value -= 0.5454546f;
//			return ((end * (((7.5625f * value) * value) + 0.75f)) + start);
//		}
//		if (value < 0.90909090909090906)
//		{
//			value -= 0.8181818f;
//			return ((end * (((7.5625f * value) * value) + 0.9375f)) + start);
//		}
//		value -= 0.9545454f;
//		return ((end * (((7.5625f * value) * value) + 0.984375f)) + start);
//	}

//	private float easeOutCirc(float start, float end, float value)
//	{
//		value--;
//		end -= start;
//		return ((end * Mathf.Sqrt(1f - (value * value))) + start);
//	}

//	private float easeOutCubic(float start, float end, float value)
//	{
//		value--;
//		end -= start;
//		return ((end * (((value * value) * value) + 1f)) + start);
//	}

//	private float easeOutElastic(float start, float end, float value)
//	{
//		end -= start;
//		float num = 1f;
//		float num2 = num * 0.3f;
//		float num3 = 0f;
//		float num4 = 0f;
//		if (value == 0f)
//		{
//			return start;
//		}
//		if ((value /= num) == 1f)
//		{
//			return (start + end);
//		}
//		if ((num4 == 0f) || (num4 < Mathf.Abs(end)))
//		{
//			num4 = end;
//			num3 = num2 / 4f;
//		}
//		else
//		{
//			num3 = (num2 / 6.283185f) * Mathf.Asin(end / num4);
//		}
//		return ((((num4 * Mathf.Pow(2f, -10f * value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2)) + end) + start);
//	}

//	private float easeOutExpo(float start, float end, float value)
//	{
//		end -= start;
//		return ((end * (-Mathf.Pow(2f, (-10f * value) / 1f) + 1f)) + start);
//	}

//	private float easeOutQuad(float start, float end, float value)
//	{
//		end -= start;
//		return (((-end * value) * (value - 2f)) + start);
//	}

//	private float easeOutQuart(float start, float end, float value)
//	{
//		value--;
//		end -= start;
//		return ((-end * ((((value * value) * value) * value) - 1f)) + start);
//	}

//	private float easeOutQuint(float start, float end, float value)
//	{
//		value--;
//		end -= start;
//		return ((end * (((((value * value) * value) * value) * value) + 1f)) + start);
//	}

//	private float easeOutSine(float start, float end, float value)
//	{
//		end -= start;
//		return ((end * Mathf.Sin((value / 1f) * 1.570796f)) + start);
//	}

//	private void EnableKinematic()
//	{
//	}

//	public static void FadeFrom(GameObject target, Hashtable args)
//	{
//		ColorFrom(target, args);
//	}

//	public static void FadeFrom(GameObject target, float alpha, float time)
//	{
//		FadeFrom(target, Hash(new object[] { "alpha", alpha, "time", time }));
//	}

//	public static void FadeTo(GameObject target, Hashtable args)
//	{
//		ColorTo(target, args);
//	}

//	public static void FadeTo(GameObject target, float alpha, float time)
//	{
//		FadeTo(target, Hash(new object[] { "alpha", alpha, "time", time }));
//	}

//	public static void FadeUpdate(GameObject target, Hashtable args)
//	{
//		args["a"] = args["alpha"];
//		ColorUpdate(target, args);
//	}

//	public static void FadeUpdate(GameObject target, float alpha, float time)
//	{
//		FadeUpdate(target, Hash(new object[] { "alpha", alpha, "time", time }));
//	}

//	private void FixedUpdate()
//	{
//		if (this.isRunning && this.physics)
//		{
//			if (!this.reverse)
//			{
//				if (this.percentage < 1f)
//				{
//					this.TweenUpdate();
//				}
//				else
//				{
//					this.TweenComplete();
//				}
//			}
//			else if (this.percentage > 0f)
//			{
//				this.TweenUpdate();
//			}
//			else
//			{
//				this.TweenComplete();
//			}
//		}
//	}

//	public static float FloatUpdate(float currentValue, float targetValue, float speed)
//	{
//		float num = targetValue - currentValue;
//		currentValue += (num * speed) * Time.deltaTime;
//		return currentValue;
//	}

//	private void GenerateAudioToTargets()
//	{
//		this.vector2s = new Vector2[3];
//		if (this.tweenArguments.Contains("audiosource"))
//		{
//			this.audioSource = (AudioSource) this.tweenArguments["audiosource"];
//		}
//		else if (base.GetComponent(typeof(AudioSource)) != null)
//		{
//			this.audioSource = base.GetComponent<AudioSource>();
//		}
//		else
//		{
//			//Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
//			this.Dispose();
//		}
//		this.vector2s[0] = this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch);
//		if (this.tweenArguments.Contains("volume"))
//		{
//			this.vector2s[1].x = (float) this.tweenArguments["volume"];
//		}
//		if (this.tweenArguments.Contains("pitch"))
//		{
//			this.vector2s[1].y = (float) this.tweenArguments["pitch"];
//		}
//	}

//	private void GenerateColorTargets()
//	{
//		this.colors = new Color[1, 3];
//		(this.colors[0, 0]) = (Color) this.tweenArguments["from"];
//		(this.colors[0, 1]) = (Color) this.tweenArguments["to"];
//	}

//	private void GenerateColorToTargets()
//	{
//		int num;
//		if (base.GetComponent(typeof(GUITexture)) != null)
//		{
//			this.colors = new Color[1, 3];
//			(this.colors[0, 0]) = (this.colors[0, 1]) = base.GetComponent<GUITexture>().color;
//		}
//		else if (base.GetComponent(typeof(GUIText)) != null)
//		{
//			this.colors = new Color[1, 3];
//			(this.colors[0, 0]) = (this.colors[0, 1]) = base.GetComponent<GUIText>().material.color;
//		}
//		else if (base.GetComponent<Renderer>() != null)
//		{
//			this.colors = new Color[base.GetComponent<Renderer>().materials.Length, 3];
//			for (num = 0; num < base.GetComponent<Renderer>().materials.Length; num++)
//			{
//				(this.colors[num, 0]) = base.GetComponent<Renderer>().materials[num].GetColor(this.namedcolorvalue.ToString());
//				(this.colors[num, 1]) = base.GetComponent<Renderer>().materials[num].GetColor(this.namedcolorvalue.ToString());
//			}
//		}
//		else if (base.GetComponent<Light>() != null)
//		{
//			this.colors = new Color[1, 3];
//			(this.colors[0, 0]) = (this.colors[0, 1]) = base.GetComponent<Light>().color;
//		}
//		else
//		{
//			this.colors = new Color[1, 3];
//		}
//		if (this.tweenArguments.Contains("color"))
//		{
//			for (num = 0; num < this.colors.GetLength(0); num++)
//			{
//				(this.colors[num, 1]) = (Color) this.tweenArguments["color"];
//			}
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("r"))
//			{
//				for (num = 0; num < this.colors.GetLength(0); num++)
//				{
//					this.colors[num, 1].r = (float) this.tweenArguments["r"];
//				}
//			}
//			if (this.tweenArguments.Contains("g"))
//			{
//				for (num = 0; num < this.colors.GetLength(0); num++)
//				{
//					this.colors[num, 1].g = (float) this.tweenArguments["g"];
//				}
//			}
//			if (this.tweenArguments.Contains("b"))
//			{
//				for (num = 0; num < this.colors.GetLength(0); num++)
//				{
//					this.colors[num, 1].b = (float) this.tweenArguments["b"];
//				}
//			}
//			if (this.tweenArguments.Contains("a"))
//			{
//				for (num = 0; num < this.colors.GetLength(0); num++)
//				{
//					this.colors[num, 1].a = (float) this.tweenArguments["a"];
//				}
//			}
//		}
//		if (this.tweenArguments.Contains("amount"))
//		{
//			for (num = 0; num < this.colors.GetLength(0); num++)
//			{
//				this.colors[num, 1].a = (float) this.tweenArguments["amount"];
//			}
//		}
//		else if (this.tweenArguments.Contains("alpha"))
//		{
//			for (num = 0; num < this.colors.GetLength(0); num++)
//			{
//				this.colors[num, 1].a = (float) this.tweenArguments["alpha"];
//			}
//		}
//	}

//	private void GenerateFloatTargets()
//	{
//		this.floats = new float[3];
//		this.floats[0] = (float) this.tweenArguments["from"];
//		this.floats[1] = (float) this.tweenArguments["to"];
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs((float) (this.floats[0] - this.floats[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private static string GenerateID()
//	{
//		int num = 15;
//		char[] chArray = new char[] { 
//			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 
//			'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 
//			'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 
//			'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8'
//		};
//		int max = chArray.Length - 1;
//		string str = "";
//		for (int i = 0; i < num; i++)
//		{
//			str = str + chArray[(int) Mathf.Floor((float) UnityEngine.Random.Range(0, max))];
//		}
//		return str;
//	}

//	private void GenerateLookToTargets()
//	{
//		this.vector3s = new Vector3[3];
//		this.vector3s[0] = base.transform.eulerAngles;
//		if (this.tweenArguments.Contains("looktarget"))
//		{
//			Vector3? nullable;
//			if (this.tweenArguments["looktarget"].GetType() == typeof(Transform))
//			{
//				nullable = (Vector3?) this.tweenArguments["up"];
//				base.transform.LookAt((Transform) this.tweenArguments["looktarget"], nullable.HasValue ? nullable.GetValueOrDefault() : Defaults.up);
//			}
//			else if (this.tweenArguments["looktarget"].GetType() == typeof(Vector3))
//			{
//				nullable = (Vector3?) this.tweenArguments["up"];
//				base.transform.LookAt((Vector3) this.tweenArguments["looktarget"], nullable.HasValue ? nullable.GetValueOrDefault() : Defaults.up);
//			}
//		}
//		else
//		{
//			//Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
//			this.Dispose();
//		}
//		this.vector3s[1] = base.transform.eulerAngles;
//		base.transform.eulerAngles = this.vector3s[0];
//		if (this.tweenArguments.Contains("axis"))
//		{
//			string str = (string) this.tweenArguments["axis"];
//			if (str != null)
//			{
//				if (!(str == "x"))
//				{
//					if (str == "y")
//					{
//						this.vector3s[1].x = this.vector3s[0].x;
//						this.vector3s[1].z = this.vector3s[0].z;
//					}
//					else if (str == "z")
//					{
//						this.vector3s[1].x = this.vector3s[0].x;
//						this.vector3s[1].y = this.vector3s[0].y;
//					}
//				}
//				else
//				{
//					this.vector3s[1].y = this.vector3s[0].y;
//					this.vector3s[1].z = this.vector3s[0].z;
//				}
//			}
//		}
//		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateMoveByTargets()
//	{
//		Vector3 vector;
//		this.vector3s = new Vector3[6];
//		this.vector3s[4] = base.transform.eulerAngles;
//		this.vector3s[3] = vector = base.transform.position;
//		this.vector3s[0] = this.vector3s[1] = vector;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] = this.vector3s[0] + ((Vector3) this.tweenArguments["amount"]);
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = this.vector3s[0].x + ((float) this.tweenArguments["x"]);
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = this.vector3s[0].y + ((float) this.tweenArguments["y"]);
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = this.vector3s[0].z + ((float) this.tweenArguments["z"]);
//			}
//		}
//		base.transform.Translate(this.vector3s[1], this.space);
//		this.vector3s[5] = base.transform.position;
//		base.transform.position = this.vector3s[0];
//		if (this.tweenArguments.Contains("orienttopath") && ((bool) this.tweenArguments["orienttopath"]))
//		{
//			this.tweenArguments["looktarget"] = this.vector3s[1];
//		}
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateMoveToPathTargets()
//	{
//		Vector3[] vectorArray;
//		bool flag;
//		int num2;
//		if (this.tweenArguments["path"].GetType() == typeof(Vector3[]))
//		{
//			Vector3[] sourceArray = (Vector3[]) this.tweenArguments["path"];
//			if (sourceArray.Length == 1)
//			{
//				//Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
//				this.Dispose();
//			}
//			vectorArray = new Vector3[sourceArray.Length];
//			Array.Copy(sourceArray, vectorArray, sourceArray.Length);
//		}
//		else
//		{
//			Transform[] transformArray = (Transform[]) this.tweenArguments["path"];
//			if (transformArray.Length == 1)
//			{
//				//Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
//				this.Dispose();
//			}
//			vectorArray = new Vector3[transformArray.Length];
//			for (int i = 0; i < transformArray.Length; i++)
//			{
//				vectorArray[i] = transformArray[i].position;
//			}
//		}
//		if (base.transform.position != vectorArray[0])
//		{
//			if (!(this.tweenArguments.Contains("movetopath") && !((bool) this.tweenArguments["movetopath"])))
//			{
//				flag = true;
//				num2 = 3;
//			}
//			else
//			{
//				flag = false;
//				num2 = 2;
//			}
//		}
//		else
//		{
//			flag = false;
//			num2 = 2;
//		}
//		this.vector3s = new Vector3[vectorArray.Length + num2];
//		if (flag)
//		{
//			this.vector3s[1] = base.transform.position;
//			num2 = 2;
//		}
//		else
//		{
//			num2 = 1;
//		}
//		Array.Copy(vectorArray, 0, this.vector3s, num2, vectorArray.Length);
//		this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
//		this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
//		if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
//		{
//			Vector3[] destinationArray = new Vector3[this.vector3s.Length];
//			Array.Copy(this.vector3s, destinationArray, this.vector3s.Length);
//			destinationArray[0] = destinationArray[destinationArray.Length - 3];
//			destinationArray[destinationArray.Length - 1] = destinationArray[2];
//			this.vector3s = new Vector3[destinationArray.Length];
//			Array.Copy(destinationArray, this.vector3s, destinationArray.Length);
//		}
//		this.path = new CRSpline(this.vector3s);
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num3 = PathLength(this.vector3s);
//			this.time = num3 / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateMoveToTargets()
//	{
//		this.vector3s = new Vector3[3];
//		if (this.isLocal)
//		{
//			this.vector3s[0] = this.vector3s[1] = base.transform.localPosition;
//		}
//		else
//		{
//			this.vector3s[0] = this.vector3s[1] = base.transform.position;
//		}
//		if (this.tweenArguments.Contains("position"))
//		{
//			if (this.tweenArguments["position"].GetType() == typeof(Transform))
//			{
//				Transform transform = (Transform) this.tweenArguments["position"];
//				this.vector3s[1] = transform.position;
//			}
//			else if (this.tweenArguments["position"].GetType() == typeof(Vector3))
//			{
//				this.vector3s[1] = (Vector3) this.tweenArguments["position"];
//			}
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//		if (this.tweenArguments.Contains("orienttopath") && ((bool) this.tweenArguments["orienttopath"]))
//		{
//			this.tweenArguments["looktarget"] = this.vector3s[1];
//		}
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GeneratePunchPositionTargets()
//	{
//		this.vector3s = new Vector3[5];
//		this.vector3s[4] = base.transform.eulerAngles;
//		this.vector3s[0] = base.transform.position;
//		this.vector3s[1] = this.vector3s[3] = Vector3.zero;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//	}

//	private void GeneratePunchRotationTargets()
//	{
//		this.vector3s = new Vector3[4];
//		this.vector3s[0] = base.transform.eulerAngles;
//		this.vector3s[1] = this.vector3s[3] = Vector3.zero;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//	}

//	private void GeneratePunchScaleTargets()
//	{
//		this.vector3s = new Vector3[3];
//		this.vector3s[0] = base.transform.localScale;
//		this.vector3s[1] = Vector3.zero;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//	}

//	private void GenerateRectTargets()
//	{
//		this.rects = new Rect[3];
//		this.rects[0] = (Rect) this.tweenArguments["from"];
//		this.rects[1] = (Rect) this.tweenArguments["to"];
//	}

//	private void GenerateRotateAddTargets()
//	{
//		Vector3 vector;
//		this.vector3s = new Vector3[5];
//		this.vector3s[3] = vector = base.transform.eulerAngles;
//		this.vector3s[0] = this.vector3s[1] = vector;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] += (Vector3) this.tweenArguments["amount"];
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x += (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y += (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z += (float) this.tweenArguments["z"];
//			}
//		}
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateRotateByTargets()
//	{
//		Vector3 vector;
//		this.vector3s = new Vector3[4];
//		this.vector3s[3] = vector = base.transform.eulerAngles;
//		this.vector3s[0] = this.vector3s[1] = vector;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] += Vector3.Scale((Vector3) this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x += 360f * ((float) this.tweenArguments["x"]);
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y += 360f * ((float) this.tweenArguments["y"]);
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z += 360f * ((float) this.tweenArguments["z"]);
//			}
//		}
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateRotateToTargets()
//	{
//		this.vector3s = new Vector3[3];
//		if (this.isLocal)
//		{
//			this.vector3s[0] = this.vector3s[1] = base.transform.localEulerAngles;
//		}
//		else
//		{
//			this.vector3s[0] = this.vector3s[1] = base.transform.eulerAngles;
//		}
//		if (this.tweenArguments.Contains("rotation"))
//		{
//			if (this.tweenArguments["rotation"].GetType() == typeof(Transform))
//			{
//				Transform transform = (Transform) this.tweenArguments["rotation"];
//				this.vector3s[1] = transform.eulerAngles;
//			}
//			else if (this.tweenArguments["rotation"].GetType() == typeof(Vector3))
//			{
//				this.vector3s[1] = (Vector3) this.tweenArguments["rotation"];
//			}
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateScaleAddTargets()
//	{
//		this.vector3s = new Vector3[3];
//		this.vector3s[0] = this.vector3s[1] = base.transform.localScale;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] += (Vector3) this.tweenArguments["amount"];
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x += (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y += (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z += (float) this.tweenArguments["z"];
//			}
//		}
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateScaleByTargets()
//	{
//		this.vector3s = new Vector3[3];
//		this.vector3s[0] = this.vector3s[1] = base.transform.localScale;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3) this.tweenArguments["amount"]);
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x *= (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y *= (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z *= (float) this.tweenArguments["z"];
//			}
//		}
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateScaleToTargets()
//	{
//		this.vector3s = new Vector3[3];
//		this.vector3s[0] = this.vector3s[1] = base.transform.localScale;
//		if (this.tweenArguments.Contains("scale"))
//		{
//			if (this.tweenArguments["scale"].GetType() == typeof(Transform))
//			{
//				Transform transform = (Transform) this.tweenArguments["scale"];
//				this.vector3s[1] = transform.localScale;
//			}
//			else if (this.tweenArguments["scale"].GetType() == typeof(Vector3))
//			{
//				this.vector3s[1] = (Vector3) this.tweenArguments["scale"];
//			}
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateShakePositionTargets()
//	{
//		this.vector3s = new Vector3[4];
//		this.vector3s[3] = base.transform.eulerAngles;
//		this.vector3s[0] = base.transform.position;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//	}

//	private void GenerateShakeRotationTargets()
//	{
//		this.vector3s = new Vector3[3];
//		this.vector3s[0] = base.transform.eulerAngles;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//	}

//	private void GenerateShakeScaleTargets()
//	{
//		this.vector3s = new Vector3[3];
//		this.vector3s[0] = base.transform.localScale;
//		if (this.tweenArguments.Contains("amount"))
//		{
//			this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
//		}
//		else
//		{
//			if (this.tweenArguments.Contains("x"))
//			{
//				this.vector3s[1].x = (float) this.tweenArguments["x"];
//			}
//			if (this.tweenArguments.Contains("y"))
//			{
//				this.vector3s[1].y = (float) this.tweenArguments["y"];
//			}
//			if (this.tweenArguments.Contains("z"))
//			{
//				this.vector3s[1].z = (float) this.tweenArguments["z"];
//			}
//		}
//	}

//	private void GenerateStabTargets()
//	{
//		if (this.tweenArguments.Contains("audiosource"))
//		{
//			this.audioSource = (AudioSource) this.tweenArguments["audiosource"];
//		}
//		else if (base.GetComponent(typeof(AudioSource)) != null)
//		{
//			this.audioSource = base.GetComponent<AudioSource>();
//		}
//		else
//		{
//			base.gameObject.AddComponent(typeof(AudioSource));
//			this.audioSource = base.GetComponent<AudioSource>();
//			this.audioSource.playOnAwake = false;
//		}
//		this.audioSource.clip = (AudioClip) this.tweenArguments["audioclip"];
//		if (this.tweenArguments.Contains("pitch"))
//		{
//			this.audioSource.pitch = (float) this.tweenArguments["pitch"];
//		}
//		if (this.tweenArguments.Contains("volume"))
//		{
//			this.audioSource.volume = (float) this.tweenArguments["volume"];
//		}
//		this.time = this.audioSource.clip.length / this.audioSource.pitch;
//	}

//	private void GenerateTargets()
//	{
//		string type = this.type;
//		switch (type)
//		{
//		case "value":
//			switch (this.method)
//			{
//			case "float":
//				this.GenerateFloatTargets();
//				this.apply = new ApplyTween(this.ApplyFloatTargets);
//				return;

//			case "vector2":
//				this.GenerateVector2Targets();
//				this.apply = new ApplyTween(this.ApplyVector2Targets);
//				return;

//			case "vector3":
//				this.GenerateVector3Targets();
//				this.apply = new ApplyTween(this.ApplyVector3Targets);
//				return;

//			case "color":
//				this.GenerateColorTargets();
//				this.apply = new ApplyTween(this.ApplyColorTargets);
//				return;

//			case "rect":
//				this.GenerateRectTargets();
//				this.apply = new ApplyTween(this.ApplyRectTargets);
//				return;
//			}
//			break;

//		case "color":
//			type = this.method;
//			if ((type != null) && (type == "to"))
//			{
//				this.GenerateColorToTargets();
//				this.apply = new ApplyTween(this.ApplyColorToTargets);
//			}
//			break;

//		case "audio":
//			type = this.method;
//			if ((type != null) && (type == "to"))
//			{
//				this.GenerateAudioToTargets();
//				this.apply = new ApplyTween(this.ApplyAudioToTargets);
//			}
//			break;

//		case "move":
//			switch (this.method)
//			{
//			case "to":
//				if (this.tweenArguments.Contains("path"))
//				{
//					this.GenerateMoveToPathTargets();
//					this.apply = new ApplyTween(this.ApplyMoveToPathTargets);
//				}
//				else
//				{
//					this.GenerateMoveToTargets();
//					this.apply = new ApplyTween(this.ApplyMoveToTargets);
//				}
//				return;

//			case "by":
//			case "add":
//				this.GenerateMoveByTargets();
//				this.apply = new ApplyTween(this.ApplyMoveByTargets);
//				return;
//			}
//			break;

//		case "scale":
//			switch (this.method)
//			{
//			case "to":
//				this.GenerateScaleToTargets();
//				this.apply = new ApplyTween(this.ApplyScaleToTargets);
//				return;

//			case "by":
//				this.GenerateScaleByTargets();
//				this.apply = new ApplyTween(this.ApplyScaleToTargets);
//				return;

//			case "add":
//				this.GenerateScaleAddTargets();
//				this.apply = new ApplyTween(this.ApplyScaleToTargets);
//				return;
//			}
//			break;

//		case "rotate":
//			switch (this.method)
//			{
//			case "to":
//				this.GenerateRotateToTargets();
//				this.apply = new ApplyTween(this.ApplyRotateToTargets);
//				return;

//			case "add":
//				this.GenerateRotateAddTargets();
//				this.apply = new ApplyTween(this.ApplyRotateAddTargets);
//				return;

//			case "by":
//				this.GenerateRotateByTargets();
//				this.apply = new ApplyTween(this.ApplyRotateAddTargets);
//				return;
//			}
//			break;

//		case "shake":
//			switch (this.method)
//			{
//			case "position":
//				this.GenerateShakePositionTargets();
//				this.apply = new ApplyTween(this.ApplyShakePositionTargets);
//				return;

//			case "scale":
//				this.GenerateShakeScaleTargets();
//				this.apply = new ApplyTween(this.ApplyShakeScaleTargets);
//				return;

//			case "rotation":
//				this.GenerateShakeRotationTargets();
//				this.apply = new ApplyTween(this.ApplyShakeRotationTargets);
//				return;
//			}
//			break;

//		case "punch":
//			switch (this.method)
//			{
//			case "position":
//				this.GeneratePunchPositionTargets();
//				this.apply = new ApplyTween(this.ApplyPunchPositionTargets);
//				return;

//			case "rotation":
//				this.GeneratePunchRotationTargets();
//				this.apply = new ApplyTween(this.ApplyPunchRotationTargets);
//				return;

//			case "scale":
//				this.GeneratePunchScaleTargets();
//				this.apply = new ApplyTween(this.ApplyPunchScaleTargets);
//				return;
//			}
//			break;

//		case "look":
//			type = this.method;
//			if ((type != null) && (type == "to"))
//			{
//				this.GenerateLookToTargets();
//				this.apply = new ApplyTween(this.ApplyLookToTargets);
//			}
//			break;

//		case "stab":
//			this.GenerateStabTargets();
//			this.apply = new ApplyTween(this.ApplyStabTargets);
//			break;
//		}
//	}

//	private void GenerateVector2Targets()
//	{
//		this.vector2s = new Vector2[3];
//		this.vector2s[0] = (Vector2) this.tweenArguments["from"];
//		this.vector2s[1] = (Vector2) this.tweenArguments["to"];
//		if (this.tweenArguments.Contains("speed"))
//		{
//			Vector3 a = new Vector3(this.vector2s[0].x, this.vector2s[0].y, 0f);
//			Vector3 b = new Vector3(this.vector2s[1].x, this.vector2s[1].y, 0f);
//			float num = Math.Abs(Vector3.Distance(a, b));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GenerateVector3Targets()
//	{
//		this.vector3s = new Vector3[3];
//		this.vector3s[0] = (Vector3) this.tweenArguments["from"];
//		this.vector3s[1] = (Vector3) this.tweenArguments["to"];
//		if (this.tweenArguments.Contains("speed"))
//		{
//			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
//			this.time = num / ((float) this.tweenArguments["speed"]);
//		}
//	}

//	private void GetEasingFunction()
//	{
//		switch (this.easeType)
//		{
//		case EaseType.easeInQuad:
//			this.ease = new EasingFunction(this.easeInQuad);
//			break;

//		case EaseType.easeOutQuad:
//			this.ease = new EasingFunction(this.easeOutQuad);
//			break;

//		case EaseType.easeInOutQuad:
//			this.ease = new EasingFunction(this.easeInOutQuad);
//			break;

//		case EaseType.easeInCubic:
//			this.ease = new EasingFunction(this.easeInCubic);
//			break;

//		case EaseType.easeOutCubic:
//			this.ease = new EasingFunction(this.easeOutCubic);
//			break;

//		case EaseType.easeInOutCubic:
//			this.ease = new EasingFunction(this.easeInOutCubic);
//			break;

//		case EaseType.easeInQuart:
//			this.ease = new EasingFunction(this.easeInQuart);
//			break;

//		case EaseType.easeOutQuart:
//			this.ease = new EasingFunction(this.easeOutQuart);
//			break;

//		case EaseType.easeInOutQuart:
//			this.ease = new EasingFunction(this.easeInOutQuart);
//			break;

//		case EaseType.easeInQuint:
//			this.ease = new EasingFunction(this.easeInQuint);
//			break;

//		case EaseType.easeOutQuint:
//			this.ease = new EasingFunction(this.easeOutQuint);
//			break;

//		case EaseType.easeInOutQuint:
//			this.ease = new EasingFunction(this.easeInOutQuint);
//			break;

//		case EaseType.easeInSine:
//			this.ease = new EasingFunction(this.easeInSine);
//			break;

//		case EaseType.easeOutSine:
//			this.ease = new EasingFunction(this.easeOutSine);
//			break;

//		case EaseType.easeInOutSine:
//			this.ease = new EasingFunction(this.easeInOutSine);
//			break;

//		case EaseType.easeInExpo:
//			this.ease = new EasingFunction(this.easeInExpo);
//			break;

//		case EaseType.easeOutExpo:
//			this.ease = new EasingFunction(this.easeOutExpo);
//			break;

//		case EaseType.easeInOutExpo:
//			this.ease = new EasingFunction(this.easeInOutExpo);
//			break;

//		case EaseType.easeInCirc:
//			this.ease = new EasingFunction(this.easeInCirc);
//			break;

//		case EaseType.easeOutCirc:
//			this.ease = new EasingFunction(this.easeOutCirc);
//			break;

//		case EaseType.easeInOutCirc:
//			this.ease = new EasingFunction(this.easeInOutCirc);
//			break;

//		case EaseType.linear:
//			this.ease = new EasingFunction(this.linear);
//			break;

//		case EaseType.spring:
//			this.ease = new EasingFunction(this.spring);
//			break;

//		case EaseType.easeInBounce:
//			this.ease = new EasingFunction(this.easeInBounce);
//			break;

//		case EaseType.easeOutBounce:
//			this.ease = new EasingFunction(this.easeOutBounce);
//			break;

//		case EaseType.easeInOutBounce:
//			this.ease = new EasingFunction(this.easeInOutBounce);
//			break;

//		case EaseType.easeInBack:
//			this.ease = new EasingFunction(this.easeInBack);
//			break;

//		case EaseType.easeOutBack:
//			this.ease = new EasingFunction(this.easeOutBack);
//			break;

//		case EaseType.easeInOutBack:
//			this.ease = new EasingFunction(this.easeInOutBack);
//			break;

//		case EaseType.easeInElastic:
//			this.ease = new EasingFunction(this.easeInElastic);
//			break;

//		case EaseType.easeOutElastic:
//			this.ease = new EasingFunction(this.easeOutElastic);
//			break;

//		case EaseType.easeInOutElastic:
//			this.ease = new EasingFunction(this.easeInOutElastic);
//			break;
//		}
//	}

//	public static Hashtable Hash(params object[] args)
//	{
//		Hashtable hashtable = new Hashtable(args.Length / 2);
//		if ((args.Length % 2) != 0)
//		{
//			//Debug.LogError("Tween Error: Hash requires an even number of arguments!");
//			return null;
//		}
//		for (int i = 0; i < (args.Length - 1); i += 2)
//		{
//			hashtable.Add(args[i], args[i + 1]);
//		}
//		return hashtable;
//	}

//	public static void Init(GameObject target)
//	{
//		MoveBy(target, Vector3.zero, 0f);
//	}

//	private static Vector3 Interp(Vector3[] pts, float t)
//	{
//		int num = pts.Length - 3;
//		int index = Mathf.Min(Mathf.FloorToInt(t * num), num - 1);
//		float num3 = (t * num) - index;
//		Vector3 vector = pts[index];
//		Vector3 vector2 = pts[index + 1];
//		Vector3 vector3 = pts[index + 2];
//		Vector3 vector4 = pts[index + 3];
//		return (Vector3) (0.5f * (((((((-vector + (3f * vector2)) - (3f * vector3)) + vector4) * ((num3 * num3) * num3)) + (((((2f * vector) - (5f * vector2)) + (4f * vector3)) - vector4) * (num3 * num3))) + ((-vector + vector3) * num3)) + (2f * vector2)));
//	}

//	private void LateUpdate()
//	{
//		if ((this.tweenArguments.Contains("looktarget") && this.isRunning) && (((this.type == "move") || (this.type == "shake")) || (this.type == "punch")))
//		{
//			LookUpdate(base.gameObject, this.tweenArguments);
//		}
//	}

//	private static void Launch(GameObject target, Hashtable args)
//	{
//		if (!args.Contains("id"))
//		{
//			args["id"] = GenerateID();
//		}
//		if (!args.Contains("target"))
//		{
//			args["target"] = target;
//		}
//		tweens.Insert(0, args);
//		target.AddComponent<iTween>();
//	}

//	private float linear(float start, float end, float value)
//	{
//		return Mathf.Lerp(start, end, value);
//	}

//	public static void LookFrom(GameObject target, Hashtable args)
//	{
//		Vector3? nullable;
//		args = CleanArgs(args);
//		Vector3 eulerAngles = target.transform.eulerAngles;
//		if (args["looktarget"].GetType() == typeof(Transform))
//		{
//			nullable = (Vector3?) args["up"];
//			target.transform.LookAt((Transform) args["looktarget"], nullable.HasValue ? nullable.GetValueOrDefault() : Defaults.up);
//		}
//		else if (args["looktarget"].GetType() == typeof(Vector3))
//		{
//			nullable = (Vector3?) args["up"];
//			target.transform.LookAt((Vector3) args["looktarget"], nullable.HasValue ? nullable.GetValueOrDefault() : Defaults.up);
//		}
//		if (args.Contains("axis"))
//		{
//			Vector3 vector2 = target.transform.eulerAngles;
//			string str = (string) args["axis"];
//			if (str != null)
//			{
//				if (!(str == "x"))
//				{
//					if (str == "y")
//					{
//						vector2.x = eulerAngles.x;
//						vector2.z = eulerAngles.z;
//					}
//					else if (str == "z")
//					{
//						vector2.x = eulerAngles.x;
//						vector2.y = eulerAngles.y;
//					}
//				}
//				else
//				{
//					vector2.y = eulerAngles.y;
//					vector2.z = eulerAngles.z;
//				}
//			}
//			target.transform.eulerAngles = vector2;
//		}
//		args["rotation"] = eulerAngles;
//		args["type"] = "rotate";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
//	{
//		LookFrom(target, Hash(new object[] { "looktarget", looktarget, "time", time }));
//	}

//	public static void LookTo(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		if (args.Contains("looktarget") && (args["looktarget"].GetType() == typeof(Transform)))
//		{
//			Transform transform = (Transform) args["looktarget"];
//			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
//			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
//		}
//		args["type"] = "look";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void LookTo(GameObject target, Vector3 looktarget, float time)
//	{
//		LookTo(target, Hash(new object[] { "looktarget", looktarget, "time", time }));
//	}

//	public static void LookUpdate(GameObject target, Hashtable args)
//	{
//		float updateTime;
//		CleanArgs(args);
//		Vector3[] vectorArray = new Vector3[5];
//		if (args.Contains("looktime"))
//		{
//			updateTime = (float) args["looktime"];
//			updateTime *= Defaults.updateTimePercentage;
//		}
//		else if (args.Contains("time"))
//		{
//			updateTime = ((float) args["time"]) * 0.15f;
//			updateTime *= Defaults.updateTimePercentage;
//		}
//		else
//		{
//			updateTime = Defaults.updateTime;
//		}
//		vectorArray[0] = target.transform.eulerAngles;
//		if (args.Contains("looktarget"))
//		{
//			Vector3? nullable;
//			if (args["looktarget"].GetType() == typeof(Transform))
//			{
//				nullable = (Vector3?) args["up"];
//				target.transform.LookAt((Transform) args["looktarget"], nullable.HasValue ? nullable.GetValueOrDefault() : Defaults.up);
//			}
//			else if (args["looktarget"].GetType() == typeof(Vector3))
//			{
//				nullable = (Vector3?) args["up"];
//				target.transform.LookAt((Vector3) args["looktarget"], nullable.HasValue ? nullable.GetValueOrDefault() : Defaults.up);
//			}
//		}
//		else
//		{
//			//Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
//			return;
//		}
//		vectorArray[1] = target.transform.eulerAngles;
//		target.transform.eulerAngles = vectorArray[0];
//		vectorArray[3].x = Mathf.SmoothDampAngle(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
//		vectorArray[3].y = Mathf.SmoothDampAngle(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
//		vectorArray[3].z = Mathf.SmoothDampAngle(vectorArray[0].z, vectorArray[1].z, ref vectorArray[2].z, updateTime);
//		target.transform.eulerAngles = vectorArray[3];
//		if (args.Contains("axis"))
//		{
//			vectorArray[4] = target.transform.eulerAngles;
//			string str = (string) args["axis"];
//			if (str != null)
//			{
//				if (!(str == "x"))
//				{
//					if (str == "y")
//					{
//						vectorArray[4].x = vectorArray[0].x;
//						vectorArray[4].z = vectorArray[0].z;
//					}
//					else if (str == "z")
//					{
//						vectorArray[4].x = vectorArray[0].x;
//						vectorArray[4].y = vectorArray[0].y;
//					}
//				}
//				else
//				{
//					vectorArray[4].y = vectorArray[0].y;
//					vectorArray[4].z = vectorArray[0].z;
//				}
//			}
//			target.transform.eulerAngles = vectorArray[4];
//		}
//	}

//	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
//	{
//		LookUpdate(target, Hash(new object[] { "looktarget", looktarget, "time", time }));
//	}

//	public static void MoveAdd(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "move";
//		args["method"] = "add";
//		Launch(target, args);
//	}

//	public static void MoveAdd(GameObject target, Vector3 amount, float time)
//	{
//		MoveAdd(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void MoveBy(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "move";
//		args["method"] = "by";
//		Launch(target, args);
//	}

//	public static void MoveBy(GameObject target, Vector3 amount, float time)
//	{
//		MoveBy(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void MoveFrom(GameObject target, Hashtable args)
//	{
//		bool isLocal;
//		args = CleanArgs(args);
//		if (args.Contains("islocal"))
//		{
//			isLocal = (bool) args["islocal"];
//		}
//		else
//		{
//			isLocal = Defaults.isLocal;
//		}
//		if (args.Contains("path"))
//		{
//			Vector3[] vectorArray2;
//			if (args["path"].GetType() == typeof(Vector3[]))
//			{
//				Vector3[] sourceArray = (Vector3[]) args["path"];
//				vectorArray2 = new Vector3[sourceArray.Length];
//				Array.Copy(sourceArray, vectorArray2, sourceArray.Length);
//			}
//			else
//			{
//				Transform[] transformArray = (Transform[]) args["path"];
//				vectorArray2 = new Vector3[transformArray.Length];
//				for (int i = 0; i < transformArray.Length; i++)
//				{
//					vectorArray2[i] = transformArray[i].position;
//				}
//			}
//			if (vectorArray2[vectorArray2.Length - 1] != target.transform.position)
//			{
//				Vector3[] destinationArray = new Vector3[vectorArray2.Length + 1];
//				Array.Copy(vectorArray2, destinationArray, vectorArray2.Length);
//				if (isLocal)
//				{
//					destinationArray[destinationArray.Length - 1] = target.transform.localPosition;
//					target.transform.localPosition = destinationArray[0];
//				}
//				else
//				{
//					destinationArray[destinationArray.Length - 1] = target.transform.position;
//					target.transform.position = destinationArray[0];
//				}
//				args["path"] = destinationArray;
//			}
//			else
//			{
//				if (isLocal)
//				{
//					target.transform.localPosition = vectorArray2[0];
//				}
//				else
//				{
//					target.transform.position = vectorArray2[0];
//				}
//				args["path"] = vectorArray2;
//			}
//		}
//		else
//		{
//			Vector3 vector;
//			Vector3 position;
//			if (isLocal)
//			{
//				vector = position = target.transform.localPosition;
//			}
//			else
//			{
//				vector = position = target.transform.position;
//			}
//			if (args.Contains("position"))
//			{
//				if (args["position"].GetType() == typeof(Transform))
//				{
//					Transform transform = (Transform) args["position"];
//					position = transform.position;
//				}
//				else if (args["position"].GetType() == typeof(Vector3))
//				{
//					position = (Vector3) args["position"];
//				}
//			}
//			else
//			{
//				if (args.Contains("x"))
//				{
//					position.x = (float) args["x"];
//				}
//				if (args.Contains("y"))
//				{
//					position.y = (float) args["y"];
//				}
//				if (args.Contains("z"))
//				{
//					position.z = (float) args["z"];
//				}
//			}
//			if (isLocal)
//			{
//				target.transform.localPosition = position;
//			}
//			else
//			{
//				target.transform.position = position;
//			}
//			args["position"] = vector;
//		}
//		args["type"] = "move";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void MoveFrom(GameObject target, Vector3 position, float time)
//	{
//		MoveFrom(target, Hash(new object[] { "position", position, "time", time }));
//	}

//	public static void MoveTo(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		if (args.Contains("position") && (args["position"].GetType() == typeof(Transform)))
//		{
//			Transform transform = (Transform) args["position"];
//			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
//			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
//			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
//		}
//		args["type"] = "move";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void MoveTo(GameObject target, Vector3 position, float time)
//	{
//		MoveTo(target, Hash(new object[] { "position", position, "time", time }));
//	}

//	public static void MoveUpdate(GameObject target, Hashtable args)
//	{
//		float updateTime;
//		bool isLocal;
//		CleanArgs(args);
//		Vector3[] vectorArray = new Vector3[4];
//		Vector3 position = target.transform.position;
//		if (args.Contains("time"))
//		{
//			updateTime = (float) args["time"];
//			updateTime *= Defaults.updateTimePercentage;
//		}
//		else
//		{
//			updateTime = Defaults.updateTime;
//		}
//		if (args.Contains("islocal"))
//		{
//			isLocal = (bool) args["islocal"];
//		}
//		else
//		{
//			isLocal = Defaults.isLocal;
//		}
//		if (isLocal)
//		{
//			vectorArray[0] = vectorArray[1] = target.transform.localPosition;
//		}
//		else
//		{
//			vectorArray[0] = vectorArray[1] = target.transform.position;
//		}
//		if (args.Contains("position"))
//		{
//			if (args["position"].GetType() == typeof(Transform))
//			{
//				Transform transform = (Transform) args["position"];
//				vectorArray[1] = transform.position;
//			}
//			else if (args["position"].GetType() == typeof(Vector3))
//			{
//				vectorArray[1] = (Vector3) args["position"];
//			}
//		}
//		else
//		{
//			if (args.Contains("x"))
//			{
//				vectorArray[1].x = (float) args["x"];
//			}
//			if (args.Contains("y"))
//			{
//				vectorArray[1].y = (float) args["y"];
//			}
//			if (args.Contains("z"))
//			{
//				vectorArray[1].z = (float) args["z"];
//			}
//		}
//		vectorArray[3].x = Mathf.SmoothDamp(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
//		vectorArray[3].y = Mathf.SmoothDamp(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
//		vectorArray[3].z = Mathf.SmoothDamp(vectorArray[0].z, vectorArray[1].z, ref vectorArray[2].z, updateTime);
//		if (args.Contains("orienttopath") && ((bool) args["orienttopath"]))
//		{
//			args["looktarget"] = vectorArray[3];
//		}
//		if (args.Contains("looktarget"))
//		{
//			LookUpdate(target, args);
//		}
//		if (isLocal)
//		{
//			target.transform.localPosition = vectorArray[3];
//		}
//		else
//		{
//			target.transform.position = vectorArray[3];
//		}
//		if (target.GetComponent<Rigidbody>() != null)
//		{
//			Vector3 vector2 = target.transform.position;
//			target.transform.position = position;
//			target.GetComponent<Rigidbody>().MovePosition(vector2);
//		}
//	}

//	public static void MoveUpdate(GameObject target, Vector3 position, float time)
//	{
//		MoveUpdate(target, Hash(new object[] { "position", position, "time", time }));
//	}

//	private void OnDisable()
//	{
//		this.DisableKinematic();
//	}

//	private void OnEnable()
//	{
//		if (this.isRunning)
//		{
//			this.EnableKinematic();
//		}
//		if (this.isPaused)
//		{
//			this.isPaused = false;
//			if (this.delay > 0f)
//			{
//				this.wasPaused = true;
//				this.ResumeDelay();
//			}
//		}
//	}

//	private static Vector3[] PathControlPointGenerator(Vector3[] path)
//	{
//		Vector3[] sourceArray = path;
//		int num = 2;
//		Vector3[] destinationArray = new Vector3[sourceArray.Length + num];
//		Array.Copy(sourceArray, 0, destinationArray, 1, sourceArray.Length);
//		destinationArray[0] = destinationArray[1] + (destinationArray[1] - destinationArray[2]);
//		destinationArray[destinationArray.Length - 1] = destinationArray[destinationArray.Length - 2] + (destinationArray[destinationArray.Length - 2] - destinationArray[destinationArray.Length - 3]);
//		if (destinationArray[1] == destinationArray[destinationArray.Length - 2])
//		{
//			Vector3[] vectorArray3 = new Vector3[destinationArray.Length];
//			Array.Copy(destinationArray, vectorArray3, destinationArray.Length);
//			vectorArray3[0] = vectorArray3[vectorArray3.Length - 3];
//			vectorArray3[vectorArray3.Length - 1] = vectorArray3[2];
//			destinationArray = new Vector3[vectorArray3.Length];
//			Array.Copy(vectorArray3, destinationArray, vectorArray3.Length);
//		}
//		return destinationArray;
//	}

//	public static float PathLength(Transform[] path)
//	{
//		int num2;
//		Vector3[] vectorArray = new Vector3[path.Length];
//		float num = 0f;
//		for (num2 = 0; num2 < path.Length; num2++)
//		{
//			vectorArray[num2] = path[num2].position;
//		}
//		Vector3[] pts = PathControlPointGenerator(vectorArray);
//		Vector3 a = Interp(pts, 0f);
//		int num3 = path.Length * 20;
//		for (num2 = 1; num2 <= num3; num2++)
//		{
//			float t = ((float) num2) / ((float) num3);
//			Vector3 b = Interp(pts, t);
//			num += Vector3.Distance(a, b);
//			a = b;
//		}
//		return num;
//	}

//	public static float PathLength(Vector3[] path)
//	{
//		float num = 0f;
//		Vector3[] pts = PathControlPointGenerator(path);
//		Vector3 a = Interp(pts, 0f);
//		int num2 = path.Length * 20;
//		for (int i = 1; i <= num2; i++)
//		{
//			float t = ((float) i) / ((float) num2);
//			Vector3 b = Interp(pts, t);
//			num += Vector3.Distance(a, b);
//			a = b;
//		}
//		return num;
//	}

//	public static void Pause()
//	{
//		for (int i = 0; i < tweens.Count; i++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[i];
//			GameObject target = (GameObject) hashtable["target"];
//			Pause(target);
//		}
//	}

//	public static void Pause(string type)
//	{
//		int num;
//		ArrayList list = new ArrayList();
//		for (num = 0; num < tweens.Count; num++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[num];
//			GameObject obj2 = (GameObject) hashtable["target"];
//			list.Insert(list.Count, obj2);
//		}
//		for (num = 0; num < list.Count; num++)
//		{
//			Pause((GameObject) list[num], type);
//		}
//	}

//	public static void Pause(GameObject target)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if (tween.delay > 0f)
//			{
//				tween.delay -= Time.time - tween.delayStarted;
//				tween.StopCoroutine("TweenDelay");
//			}
//			tween.isPaused = true;
//			tween.enabled = false;
//		}
//	}

//	public static void Pause(GameObject target, bool includechildren)
//	{
//		Pause(target);
//		if (includechildren)
//		{
//			foreach (Transform transform in target.transform)
//			{
//				Pause(transform.gameObject, true);
//			}
//		}
//	}

//	public static void Pause(GameObject target, string type)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
//			{
//				if (tween.delay > 0f)
//				{
//					tween.delay -= Time.time - tween.delayStarted;
//					tween.StopCoroutine("TweenDelay");
//				}
//				tween.isPaused = true;
//				tween.enabled = false;
//			}
//		}
//	}

//	public static void Pause(GameObject target, string type, bool includechildren)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
//			{
//				if (tween.delay > 0f)
//				{
//					tween.delay -= Time.time - tween.delayStarted;
//					tween.StopCoroutine("TweenDelay");
//				}
//				tween.isPaused = true;
//				tween.enabled = false;
//			}
//		}
//		if (includechildren)
//		{
//			foreach (Transform transform in target.transform)
//			{
//				Pause(transform.gameObject, type, true);
//			}
//		}
//	}

//	public static Vector3 PointOnPath(Transform[] path, float percent)
//	{
//		Vector3[] vectorArray = new Vector3[path.Length];
//		for (int i = 0; i < path.Length; i++)
//		{
//			vectorArray[i] = path[i].position;
//		}
//		return Interp(PathControlPointGenerator(vectorArray), percent);
//	}

//	public static Vector3 PointOnPath(Vector3[] path, float percent)
//	{
//		return Interp(PathControlPointGenerator(path), percent);
//	}

//	private float punch(float amplitude, float value)
//	{
//		float num = 9f;
//		if (value == 0f)
//		{
//			return 0f;
//		}
//		if (value == 1f)
//		{
//			return 0f;
//		}
//		float num2 = 0.3f;
//		num = (num2 / 6.283185f) * Mathf.Asin(0f);
//		return ((amplitude * Mathf.Pow(2f, -10f * value)) * Mathf.Sin((((value * 1f) - num) * 6.283185f) / num2));
//	}

//	public static void PunchPosition(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "punch";
//		args["method"] = "position";
//		args["easetype"] = EaseType.punch;
//		Launch(target, args);
//	}

//	public static void PunchPosition(GameObject target, Vector3 amount, float time)
//	{
//		PunchPosition(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void PunchRotation(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "punch";
//		args["method"] = "rotation";
//		args["easetype"] = EaseType.punch;
//		Launch(target, args);
//	}

//	public static void PunchRotation(GameObject target, Vector3 amount, float time)
//	{
//		PunchRotation(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void PunchScale(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "punch";
//		args["method"] = "scale";
//		args["easetype"] = EaseType.punch;
//		Launch(target, args);
//	}

//	public static void PunchScale(GameObject target, Vector3 amount, float time)
//	{
//		PunchScale(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void PutOnPath(GameObject target, Transform[] path, float percent)
//	{
//		Vector3[] vectorArray = new Vector3[path.Length];
//		for (int i = 0; i < path.Length; i++)
//		{
//			vectorArray[i] = path[i].position;
//		}
//		target.transform.position = Interp(PathControlPointGenerator(vectorArray), percent);
//	}

//	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
//	{
//		target.transform.position = Interp(PathControlPointGenerator(path), percent);
//	}

//	public static void PutOnPath(Transform target, Transform[] path, float percent)
//	{
//		Vector3[] vectorArray = new Vector3[path.Length];
//		for (int i = 0; i < path.Length; i++)
//		{
//			vectorArray[i] = path[i].position;
//		}
//		target.position = Interp(PathControlPointGenerator(vectorArray), percent);
//	}

//	public static void PutOnPath(Transform target, Vector3[] path, float percent)
//	{
//		target.position = Interp(PathControlPointGenerator(path), percent);
//	}

//	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
//	{
//		return new Rect(FloatUpdate(currentValue.x, targetValue.x, speed), FloatUpdate(currentValue.y, targetValue.y, speed), FloatUpdate(currentValue.width, targetValue.width, speed), FloatUpdate(currentValue.height, targetValue.height, speed));
//	}

//	public static void Resume()
//	{
//		for (int i = 0; i < tweens.Count; i++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[i];
//			GameObject target = (GameObject) hashtable["target"];
//			Resume(target);
//		}
//	}

//	public static void Resume(string type)
//	{
//		int num;
//		ArrayList list = new ArrayList();
//		for (num = 0; num < tweens.Count; num++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[num];
//			GameObject obj2 = (GameObject) hashtable["target"];
//			list.Insert(list.Count, obj2);
//		}
//		for (num = 0; num < list.Count; num++)
//		{
//			Resume((GameObject) list[num], type);
//		}
//	}

//	public static void Resume(GameObject target)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			tween.enabled = true;
//		}
//	}

//	public static void Resume(GameObject target, bool includechildren)
//	{
//		Resume(target);
//		if (includechildren)
//		{
//			foreach (Transform transform in target.transform)
//			{
//				Resume(transform.gameObject, true);
//			}
//		}
//	}

//	public static void Resume(GameObject target, string type)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
//			{
//				tween.enabled = true;
//			}
//		}
//	}

//	public static void Resume(GameObject target, string type, bool includechildren)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
//			{
//				tween.enabled = true;
//			}
//		}
//		if (includechildren)
//		{
//			foreach (Transform transform in target.transform)
//			{
//				Resume(transform.gameObject, type, true);
//			}
//		}
//	}

//	private void ResumeDelay()
//	{
//		base.StartCoroutine("TweenDelay");
//	}

//	private void RetrieveArgs()
//	{
//		foreach (Hashtable hashtable in tweens)
//		{
//			if (((GameObject) hashtable["target"]) == base.gameObject)
//			{
//				this.tweenArguments = hashtable;
//				break;
//			}
//		}
//		this.id = (string) this.tweenArguments["id"];
//		this.type = (string) this.tweenArguments["type"];
//		this._name = (string) this.tweenArguments["name"];
//		this.method = (string) this.tweenArguments["method"];
//		if (this.tweenArguments.Contains("time"))
//		{
//			this.time = (float) this.tweenArguments["time"];
//		}
//		else
//		{
//			this.time = Defaults.time;
//		}
//		if (base.GetComponent<Rigidbody>() != null)
//		{
//			this.physics = true;
//		}
//		if (this.tweenArguments.Contains("delay"))
//		{
//			this.delay = (float) this.tweenArguments["delay"];
//		}
//		else
//		{
//			this.delay = Defaults.delay;
//		}
//		if (this.tweenArguments.Contains("namedcolorvalue"))
//		{
//			if (this.tweenArguments["namedcolorvalue"].GetType() == typeof(NamedValueColor))
//			{
//				this.namedcolorvalue = (NamedValueColor) this.tweenArguments["namedcolorvalue"];
//			}
//			else
//			{
//				try
//				{
//					this.namedcolorvalue = (NamedValueColor) Enum.Parse(typeof(NamedValueColor), (string) this.tweenArguments["namedcolorvalue"], true);
//				}
//				catch
//				{
//					//Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
//					this.namedcolorvalue = NamedValueColor._Color;
//				}
//			}
//		}
//		else
//		{
//			this.namedcolorvalue = Defaults.namedColorValue;
//		}
//		if (this.tweenArguments.Contains("looptype"))
//		{
//			if (this.tweenArguments["looptype"].GetType() == typeof(LoopType))
//			{
//				this.loopType = (LoopType) this.tweenArguments["looptype"];
//			}
//			else
//			{
//				try
//				{
//					this.loopType = (LoopType) Enum.Parse(typeof(LoopType), (string) this.tweenArguments["looptype"], true);
//				}
//				catch
//				{
//					//Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
//					this.loopType = LoopType.none;
//				}
//			}
//		}
//		else
//		{
//			this.loopType = LoopType.none;
//		}
//		if (this.tweenArguments.Contains("easetype"))
//		{
//			if (this.tweenArguments["easetype"].GetType() == typeof(EaseType))
//			{
//				this.easeType = (EaseType) this.tweenArguments["easetype"];
//			}
//			else
//			{
//				try
//				{
//					this.easeType = (EaseType) Enum.Parse(typeof(EaseType), (string) this.tweenArguments["easetype"], true);
//				}
//				catch
//				{
//					//Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
//					this.easeType = Defaults.easeType;
//				}
//			}
//		}
//		else
//		{
//			this.easeType = Defaults.easeType;
//		}
//		if (this.tweenArguments.Contains("space"))
//		{
//			if (this.tweenArguments["space"].GetType() == typeof(Space))
//			{
//				this.space = (Space) this.tweenArguments["space"];
//			}
//			else
//			{
//				try
//				{
//					this.space = (Space) Enum.Parse(typeof(Space), (string) this.tweenArguments["space"], true);
//				}
//				catch
//				{
//					//Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
//					this.space = Defaults.space;
//				}
//			}
//		}
//		else
//		{
//			this.space = Defaults.space;
//		}
//		if (this.tweenArguments.Contains("islocal"))
//		{
//			this.isLocal = (bool) this.tweenArguments["islocal"];
//		}
//		else
//		{
//			this.isLocal = Defaults.isLocal;
//		}
//		if (this.tweenArguments.Contains("ignoretimescale"))
//		{
//			this.useRealTime = (bool) this.tweenArguments["ignoretimescale"];
//		}
//		else
//		{
//			this.useRealTime = Defaults.useRealTime;
//		}
//		this.GetEasingFunction();
//	}

//	public static void RotateAdd(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "rotate";
//		args["method"] = "add";
//		Launch(target, args);
//	}

//	public static void RotateAdd(GameObject target, Vector3 amount, float time)
//	{
//		RotateAdd(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void RotateBy(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "rotate";
//		args["method"] = "by";
//		Launch(target, args);
//	}

//	public static void RotateBy(GameObject target, Vector3 amount, float time)
//	{
//		RotateBy(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void RotateFrom(GameObject target, Hashtable args)
//	{
//		Vector3 vector;
//		Vector3 eulerAngles;
//		bool isLocal;
//		args = CleanArgs(args);
//		if (args.Contains("islocal"))
//		{
//			isLocal = (bool) args["islocal"];
//		}
//		else
//		{
//			isLocal = Defaults.isLocal;
//		}
//		if (isLocal)
//		{
//			vector = eulerAngles = target.transform.localEulerAngles;
//		}
//		else
//		{
//			vector = eulerAngles = target.transform.eulerAngles;
//		}
//		if (args.Contains("rotation"))
//		{
//			if (args["rotation"].GetType() == typeof(Transform))
//			{
//				Transform transform = (Transform) args["rotation"];
//				eulerAngles = transform.eulerAngles;
//			}
//			else if (args["rotation"].GetType() == typeof(Vector3))
//			{
//				eulerAngles = (Vector3) args["rotation"];
//			}
//		}
//		else
//		{
//			if (args.Contains("x"))
//			{
//				eulerAngles.x = (float) args["x"];
//			}
//			if (args.Contains("y"))
//			{
//				eulerAngles.y = (float) args["y"];
//			}
//			if (args.Contains("z"))
//			{
//				eulerAngles.z = (float) args["z"];
//			}
//		}
//		if (isLocal)
//		{
//			target.transform.localEulerAngles = eulerAngles;
//		}
//		else
//		{
//			target.transform.eulerAngles = eulerAngles;
//		}
//		args["rotation"] = vector;
//		args["type"] = "rotate";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
//	{
//		RotateFrom(target, Hash(new object[] { "rotation", rotation, "time", time }));
//	}

//	public static void RotateTo(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		if (args.Contains("rotation") && (args["rotation"].GetType() == typeof(Transform)))
//		{
//			Transform transform = (Transform) args["rotation"];
//			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
//			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
//			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
//		}
//		args["type"] = "rotate";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void RotateTo(GameObject target, Vector3 rotation, float time)
//	{
//		RotateTo(target, Hash(new object[] { "rotation", rotation, "time", time }));
//	}

//	public static void RotateUpdate(GameObject target, Hashtable args)
//	{
//		bool isLocal;
//		float updateTime;
//		CleanArgs(args);
//		Vector3[] vectorArray = new Vector3[4];
//		Vector3 eulerAngles = target.transform.eulerAngles;
//		if (args.Contains("time"))
//		{
//			updateTime = (float) args["time"];
//			updateTime *= Defaults.updateTimePercentage;
//		}
//		else
//		{
//			updateTime = Defaults.updateTime;
//		}
//		if (args.Contains("islocal"))
//		{
//			isLocal = (bool) args["islocal"];
//		}
//		else
//		{
//			isLocal = Defaults.isLocal;
//		}
//		if (isLocal)
//		{
//			vectorArray[0] = target.transform.localEulerAngles;
//		}
//		else
//		{
//			vectorArray[0] = target.transform.eulerAngles;
//		}
//		if (args.Contains("rotation"))
//		{
//			if (args["rotation"].GetType() == typeof(Transform))
//			{
//				Transform transform = (Transform) args["rotation"];
//				vectorArray[1] = transform.eulerAngles;
//			}
//			else if (args["rotation"].GetType() == typeof(Vector3))
//			{
//				vectorArray[1] = (Vector3) args["rotation"];
//			}
//		}
//		vectorArray[3].x = Mathf.SmoothDampAngle(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
//		vectorArray[3].y = Mathf.SmoothDampAngle(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
//		vectorArray[3].z = Mathf.SmoothDampAngle(vectorArray[0].z, vectorArray[1].z, ref vectorArray[2].z, updateTime);
//		if (isLocal)
//		{
//			target.transform.localEulerAngles = vectorArray[3];
//		}
//		else
//		{
//			target.transform.eulerAngles = vectorArray[3];
//		}
//		if (target.GetComponent<Rigidbody>() != null)
//		{
//			Vector3 euler = target.transform.eulerAngles;
//			target.transform.eulerAngles = eulerAngles;
//			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(euler));
//		}
//	}

//	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
//	{
//		RotateUpdate(target, Hash(new object[] { "rotation", rotation, "time", time }));
//	}

//	public static void ScaleAdd(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "scale";
//		args["method"] = "add";
//		Launch(target, args);
//	}

//	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
//	{
//		ScaleAdd(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void ScaleBy(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "scale";
//		args["method"] = "by";
//		Launch(target, args);
//	}

//	public static void ScaleBy(GameObject target, Vector3 amount, float time)
//	{
//		ScaleBy(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void ScaleFrom(GameObject target, Hashtable args)
//	{
//		Vector3 localScale;
//		args = CleanArgs(args);
//		Vector3 vector = localScale = target.transform.localScale;
//		if (args.Contains("scale"))
//		{
//			if (args["scale"].GetType() == typeof(Transform))
//			{
//				Transform transform = (Transform) args["scale"];
//				localScale = transform.localScale;
//			}
//			else if (args["scale"].GetType() == typeof(Vector3))
//			{
//				localScale = (Vector3) args["scale"];
//			}
//		}
//		else
//		{
//			if (args.Contains("x"))
//			{
//				localScale.x = (float) args["x"];
//			}
//			if (args.Contains("y"))
//			{
//				localScale.y = (float) args["y"];
//			}
//			if (args.Contains("z"))
//			{
//				localScale.z = (float) args["z"];
//			}
//		}
//		target.transform.localScale = localScale;
//		args["scale"] = vector;
//		args["type"] = "scale";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
//	{
//		ScaleFrom(target, Hash(new object[] { "scale", scale, "time", time }));
//	}

//	public static void ScaleTo(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		if (args.Contains("scale") && (args["scale"].GetType() == typeof(Transform)))
//		{
//			Transform transform = (Transform) args["scale"];
//			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
//			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
//			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
//		}
//		args["type"] = "scale";
//		args["method"] = "to";
//		Launch(target, args);
//	}

//	public static void ScaleTo(GameObject target, Vector3 scale, float time)
//	{
//		ScaleTo(target, Hash(new object[] { "scale", scale, "time", time }));
//	}

//	public static void ScaleUpdate(GameObject target, Hashtable args)
//	{
//		float updateTime;
//		CleanArgs(args);
//		Vector3[] vectorArray = new Vector3[4];
//		if (args.Contains("time"))
//		{
//			updateTime = (float) args["time"];
//			updateTime *= Defaults.updateTimePercentage;
//		}
//		else
//		{
//			updateTime = Defaults.updateTime;
//		}
//		vectorArray[0] = vectorArray[1] = target.transform.localScale;
//		if (args.Contains("scale"))
//		{
//			if (args["scale"].GetType() == typeof(Transform))
//			{
//				Transform transform = (Transform) args["scale"];
//				vectorArray[1] = transform.localScale;
//			}
//			else if (args["scale"].GetType() == typeof(Vector3))
//			{
//				vectorArray[1] = (Vector3) args["scale"];
//			}
//		}
//		else
//		{
//			if (args.Contains("x"))
//			{
//				vectorArray[1].x = (float) args["x"];
//			}
//			if (args.Contains("y"))
//			{
//				vectorArray[1].y = (float) args["y"];
//			}
//			if (args.Contains("z"))
//			{
//				vectorArray[1].z = (float) args["z"];
//			}
//		}
//		vectorArray[3].x = Mathf.SmoothDamp(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
//		vectorArray[3].y = Mathf.SmoothDamp(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
//		vectorArray[3].z = Mathf.SmoothDamp(vectorArray[0].z, vectorArray[1].z, ref vectorArray[2].z, updateTime);
//		target.transform.localScale = vectorArray[3];
//	}

//	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
//	{
//		ScaleUpdate(target, Hash(new object[] { "scale", scale, "time", time }));
//	}

//	public static void ShakePosition(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "shake";
//		args["method"] = "position";
//		Launch(target, args);
//	}

//	public static void ShakePosition(GameObject target, Vector3 amount, float time)
//	{
//		ShakePosition(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void ShakeRotation(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "shake";
//		args["method"] = "rotation";
//		Launch(target, args);
//	}

//	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
//	{
//		ShakeRotation(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	public static void ShakeScale(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "shake";
//		args["method"] = "scale";
//		Launch(target, args);
//	}

//	public static void ShakeScale(GameObject target, Vector3 amount, float time)
//	{
//		ShakeScale(target, Hash(new object[] { "amount", amount, "time", time }));
//	}

//	private float spring(float start, float end, float value)
//	{
//		value = Mathf.Clamp01(value);
//		value = ((Mathf.Sin((value * 3.141593f) * (0.2f + (((2.5f * value) * value) * value))) * Mathf.Pow(1f - value, 2.2f)) + value) * (1f + (1.2f * (1f - value)));
//		return (start + ((end - start) * value));
//	}

//	public static void Stab(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		args["type"] = "stab";
//		Launch(target, args);
//	}

//	public static void Stab(GameObject target, AudioClip audioclip, float delay)
//	{
//		Stab(target, Hash(new object[] { "audioclip", audioclip, "delay", delay }));
//	}

//	private IEnumerator Start()
//	{
//		if (this.delay > 0f)
//		{
//			yield return this.StartCoroutine("TweenDelay");
//		}
//		this.TweenStart();
//	}

//	public static void Stop()
//	{
//		for (int i = 0; i < tweens.Count; i++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[i];
//			GameObject target = (GameObject) hashtable["target"];
//			Stop(target);
//		}
//		tweens.Clear();
//	}

//	public static void Stop(string type)
//	{
//		int num;
//		ArrayList list = new ArrayList();
//		for (num = 0; num < tweens.Count; num++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[num];
//			GameObject obj2 = (GameObject) hashtable["target"];
//			list.Insert(list.Count, obj2);
//		}
//		for (num = 0; num < list.Count; num++)
//		{
//			Stop((GameObject) list[num], type);
//		}
//	}

//	public static void Stop(GameObject target)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			tween.Dispose();
//		}
//	}

//	public static void Stop(GameObject target, bool includechildren)
//	{
//		Stop(target);
//		if (includechildren)
//		{
//			foreach (Transform transform in target.transform)
//			{
//				Stop(transform.gameObject, true);
//			}
//		}
//	}

//	public static void Stop(GameObject target, string type)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
//			{
//				tween.Dispose();
//			}
//		}
//	}

//	public static void Stop(GameObject target, string type, bool includechildren)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
//			{
//				tween.Dispose();
//			}
//		}
//		if (includechildren)
//		{
//			foreach (Transform transform in target.transform)
//			{
//				Stop(transform.gameObject, type, true);
//			}
//		}
//	}

//	public static void StopByName(string name)
//	{
//		int num;
//		ArrayList list = new ArrayList();
//		for (num = 0; num < tweens.Count; num++)
//		{
//			Hashtable hashtable = (Hashtable) tweens[num];
//			GameObject obj2 = (GameObject) hashtable["target"];
//			list.Insert(list.Count, obj2);
//		}
//		for (num = 0; num < list.Count; num++)
//		{
//			StopByName((GameObject) list[num], name);
//		}
//	}

//	public static void StopByName(GameObject target, string name)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if (tween._name == name)
//			{
//				tween.Dispose();
//			}
//		}
//	}

//	public static void StopByName(GameObject target, string name, bool includechildren)
//	{
//		Component[] components = target.GetComponents(typeof(iTween));
//		foreach (iTween tween in components)
//		{
//			if (tween._name == name)
//			{
//				tween.Dispose();
//			}
//		}
//		if (includechildren)
//		{
//			foreach (Transform transform in target.transform)
//			{
//				StopByName(transform.gameObject, name, true);
//			}
//		}
//	}

//	private void TweenComplete()
//	{
//		this.isRunning = false;
//		if (this.percentage > 0.5f)
//		{
//			this.percentage = 1f;
//		}
//		else
//		{
//			this.percentage = 0f;
//		}
//		this.apply();
//		if (this.type == "value")
//		{
//			this.CallBack("onupdate");
//		}
//		if (this.loopType == LoopType.none)
//		{
//			this.Dispose();
//		}
//		else
//		{
//			this.TweenLoop();
//		}
//		this.CallBack("oncomplete");
//	}

//	private IEnumerator TweenDelay()
//	{
//		this.delayStarted = Time.time;
//		yield return new WaitForSeconds(this.delay);
//		if (this.wasPaused)
//		{
//			this.wasPaused = false;
//			this.TweenStart();
//		}
//	}

//	private void TweenLoop()
//	{
//		this.DisableKinematic();
//		switch (this.loopType)
//		{
//		case LoopType.loop:
//			this.percentage = 0f;
//			this.runningTime = 0f;
//			this.apply();
//			base.StartCoroutine("TweenRestart");
//			break;

//		case LoopType.pingPong:
//			this.reverse = !this.reverse;
//			this.runningTime = 0f;
//			base.StartCoroutine("TweenRestart");
//			break;
//		}
//	}

//	private IEnumerator TweenRestart()
//	{
//		if (this.delay > 0f)
//		{
//			this.delayStarted = Time.time;
//			yield return new WaitForSeconds(this.delay);
//		}
//		this.loop = true;
//		this.TweenStart();
//	}

//	private void TweenStart()
//	{
//		this.CallBack("onstart");
//		if (!this.loop)
//		{
//			this.ConflictCheck();
//			this.GenerateTargets();
//		}
//		if (this.type == "stab")
//		{
//			this.audioSource.PlayOneShot(this.audioSource.clip);
//		}
//		if (((((this.type == "move") || (this.type == "scale")) || ((this.type == "rotate") || (this.type == "punch"))) || ((this.type == "shake") || (this.type == "curve"))) || (this.type == "look"))
//		{
//			this.EnableKinematic();
//		}
//		this.isRunning = true;
//	}

//	private void TweenUpdate()
//	{
//		this.apply();
//		this.CallBack("onupdate");
//		this.UpdatePercentage();
//	}

//	private void Update()
//	{
//		if (this.isRunning && !this.physics)
//		{
//			if (!this.reverse)
//			{
//				if (this.percentage < 1f)
//				{
//					this.TweenUpdate();
//				}
//				else
//				{
//					this.TweenComplete();
//				}
//			}
//			else if (this.percentage > 0f)
//			{
//				this.TweenUpdate();
//			}
//			else
//			{
//				this.TweenComplete();
//			}
//		}
//	}

//	private void UpdatePercentage()
//	{
//		if (this.useRealTime)
//		{
//			this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
//		}
//		else
//		{
//			this.runningTime += Time.deltaTime;
//		}
//		if (this.reverse)
//		{
//			this.percentage = 1f - (this.runningTime / this.time);
//		}
//		else
//		{
//			this.percentage = this.runningTime / this.time;
//		}
//		this.lastRealTime = Time.realtimeSinceStartup;
//	}

//	public static void ValueTo(GameObject target, Hashtable args)
//	{
//		args = CleanArgs(args);
//		if (!((args.Contains("onupdate") && args.Contains("from")) && args.Contains("to")))
//		{
//			//Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
//		}
//		else
//		{
//			args["type"] = "value";
//			if (args["from"].GetType() == typeof(Vector2))
//			{
//				args["method"] = "vector2";
//			}
//			else if (args["from"].GetType() == typeof(Vector3))
//			{
//				args["method"] = "vector3";
//			}
//			else if (args["from"].GetType() == typeof(Rect))
//			{
//				args["method"] = "rect";
//			}
//			else if (args["from"].GetType() == typeof(float))
//			{
//				args["method"] = "float";
//			}
//			else if (args["from"].GetType() == typeof(Color))
//			{
//				args["method"] = "color";
//			}
//			else
//			{
//				//Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
//				return;
//			}
//			if (!args.Contains("easetype"))
//			{
//				args.Add("easetype", EaseType.linear);
//			}
//			Launch(target, args);
//		}
//	}

//	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
//	{
//		Vector2 vector = targetValue - currentValue;
//		currentValue += (Vector2) ((vector * speed) * Time.deltaTime);
//		return currentValue;
//	}

//	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
//	{
//		Vector3 vector = targetValue - currentValue;
//		currentValue += (Vector3) ((vector * speed) * Time.deltaTime);
//		return currentValue;
//	}

//	[CompilerGenerated]
//	private sealed class d_4 : IEnumerator<object>, IEnumerator, IDisposable
//	{
//		private int __state;
//		private object __current;
//		public iTween __this;

//		[DebuggerHidden]
//		public d_4(int __state)
//		{
//			this.__state = __state;
//		}

//		public bool MoveNext()
//		{
//			switch (this.__state)
//			{
//			case 0:
//				this.__state = -1;
//				if (this.__this.delay <= 0f)
//				{
//					break;
//				}
//				this.__current = this.__this.StartCoroutine("TweenDelay");
//				this.__state = 1;
//				return true;

//			case 1:
//				this.__state = -1;
//				break;

//			default:
//				//goto Label_0075;
//				break;
//			}
//			this.__this.TweenStart();
//			//Label_0075:
//			return false;
//		}

//		[DebuggerHidden]
//		void IEnumerator.Reset()
//		{
//			throw new NotSupportedException();
//		}

//		void IDisposable.Dispose()
//		{
//		}

//		object IEnumerator<object>.Current
//		{
//			[DebuggerHidden]
//			get
//			{
//				return this.__current;
//			}
//		}

//		object IEnumerator.Current
//		{
//			[DebuggerHidden]
//			get
//			{
//				return this.__current;
//			}
//		}
//	}

//	[CompilerGenerated]
//	private sealed class d__0 : IEnumerator<object>, IEnumerator, IDisposable
//	{
//		private int __state;
//		private object __current;
//		public iTween __this;

//		[DebuggerHidden]
//		public d__0(int __state)
//		{
//			this.__state = __state;
//		}

//		public bool MoveNext()
//		{
//			switch (this.__state)
//			{
//			case 0:
//				this.__state = -1;
//				this.__this.delayStarted = Time.time;
//				this.__current = new WaitForSeconds(this.__this.delay);
//				this.__state = 1;
//				return true;

//			case 1:
//				this.__state = -1;
//				if (this.__this.wasPaused)
//				{
//					this.__this.wasPaused = false;
//					this.__this.TweenStart();
//				}
//				break;
//			}
//			return false;
//		}

//		[DebuggerHidden]
//		void IEnumerator.Reset()
//		{
//			throw new NotSupportedException();
//		}

//		void IDisposable.Dispose()
//		{
//		}

//		object IEnumerator<object>.Current
//		{
//			[DebuggerHidden]
//			get
//			{
//				return this.__current;
//			}
//		}

//		object IEnumerator.Current
//		{
//			[DebuggerHidden]
//			get
//			{
//				return this.__current;
//			}
//		}
//	}

//	[CompilerGenerated]
//	private sealed class d__2 : IEnumerator<object>, IEnumerator, IDisposable
//	{
//		private int __state;
//		private object __current;
//		public iTween __this;

//		[DebuggerHidden]
//		public d__2(int __state)
//		{
//			this.__state = __state;
//		}

//		public bool MoveNext()
//		{
//			switch (this.__state)
//			{
//			case 0:
//				this.__state = -1;
//				if (this.__this.delay <= 0f)
//				{
//					break;
//				}
//				this.__this.delayStarted = Time.time;
//				this.__current = new WaitForSeconds(this.__this.delay);
//				this.__state = 1;
//				return true;

//			case 1:
//				this.__state = -1;
//				break;

//			default:
//				//goto Label_0091;
//				break;
//			}
//			this.__this.loop = true;
//			this.__this.TweenStart();
//			//Label_0091:
//			return false;
//		}

//		[DebuggerHidden]
//		void IEnumerator.Reset()
//		{
//			throw new NotSupportedException();
//		}

//		void IDisposable.Dispose()
//		{
//		}

//		object IEnumerator<object>.Current
//		{
//			[DebuggerHidden]
//			get
//			{
//				return this.__current;
//			}
//		}

//		object IEnumerator.Current
//		{
//			[DebuggerHidden]
//			get
//			{
//				return this.__current;
//			}
//		}
//	}

//	private delegate void ApplyTween();

//	private class CRSpline
//	{
//		public Vector3[] pts;

//		public CRSpline(params Vector3[] pts)
//		{
//			this.pts = new Vector3[pts.Length];
//			Array.Copy(pts, this.pts, pts.Length);
//		}

//		public Vector3 Interp(float t)
//		{
//			int num = this.pts.Length - 3;
//			int index = Mathf.Min(Mathf.FloorToInt(t * num), num - 1);
//			float num3 = (t * num) - index;
//			Vector3 vector = this.pts[index];
//			Vector3 vector2 = this.pts[index + 1];
//			Vector3 vector3 = this.pts[index + 2];
//			Vector3 vector4 = this.pts[index + 3];
//			return (Vector3) (0.5f * (((((((-vector + (3f * vector2)) - (3f * vector3)) + vector4) * ((num3 * num3) * num3)) + (((((2f * vector) - (5f * vector2)) + (4f * vector3)) - vector4) * (num3 * num3))) + ((-vector + vector3) * num3)) + (2f * vector2)));
//		}
//	}

//	public static class Defaults
//	{
//		public static int cameraFadeDepth = 0xf423f;
//		public static Color color = Color.white;
//		public static float delay = 0f;
//		public static iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
//		public static bool isLocal = false;
//		public static float lookAhead = 0.05f;
//		public static float lookSpeed = 3f;
//		public static iTween.LoopType loopType = iTween.LoopType.none;
//		public static iTween.NamedValueColor namedColorValue = iTween.NamedValueColor._Color;
//		public static bool orientToPath = false;
//		public static Space space = Space.Self;
//		public static float time = 1f;
//		public static Vector3 up = Vector3.up;
//		public static float updateTime = (1f * updateTimePercentage);
//		public static float updateTimePercentage = 0.05f;
//		public static bool useRealTime = false;
//	}

//	public enum EaseType
//	{
//		easeInQuad,
//		easeOutQuad,
//		easeInOutQuad,
//		easeInCubic,
//		easeOutCubic,
//		easeInOutCubic,
//		easeInQuart,
//		easeOutQuart,
//		easeInOutQuart,
//		easeInQuint,
//		easeOutQuint,
//		easeInOutQuint,
//		easeInSine,
//		easeOutSine,
//		easeInOutSine,
//		easeInExpo,
//		easeOutExpo,
//		easeInOutExpo,
//		easeInCirc,
//		easeOutCirc,
//		easeInOutCirc,
//		linear,
//		spring,
//		easeInBounce,
//		easeOutBounce,
//		easeInOutBounce,
//		easeInBack,
//		easeOutBack,
//		easeInOutBack,
//		easeInElastic,
//		easeOutElastic,
//		easeInOutElastic,
//		punch
//	}

//	private delegate float EasingFunction(float start, float end, float value);

//	public enum LoopType
//	{
//		none,
//		loop,
//		pingPong
//	}

//	public enum NamedValueColor
//	{
//		_Color,
//		_SpecColor,
//		_Emission,
//		_ReflectColor
//	}
//}
