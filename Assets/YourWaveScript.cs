using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YourWaveScript : MonoBehaviour
{
	private Material _mat;

	private const float Count = 20;
	private const float Delta = 1 / (Count - 1f);

	//x y z means color, w means pos of uv.y(or x)
	private Vector4 _fixColorBase = new Vector4(1.9f,		1f,			1.5f,			1f) ;
	private Vector4 _fixColorZero = new Vector4(0f,		0f,			0f,			100f) ;

	private Vector4 _modelColor0 = new Vector4(1.9f * 0.1f,	1f,			1.5f,			-0.7f) ;
	private Vector4 _modelColor1 = new Vector4(1.9f,			1f * 0.1f,	1.5f,			-0.5f) ;
	private Vector4 _modelColor2 = new Vector4(1.9f,			1f,			1.5f * 0.1f,	-0.3f) ;
	private Vector4 _modelColor3 = new Vector4(1.9f * 0.1f,	1f * 0.1f,	1.5f,			0f) ;
	private Vector4 _modelColor4 = new Vector4(1.9f,			1f * 0.1f,	1.5f * 0.1f,	0.2f) ;
	private Vector4 _modelColor5 = new Vector4(1.9f * 0.1f,	1f,			1.5f * 0.1f,	0.4f) ;
	private Vector4 _modelColor6 = new Vector4(1.9f * 0.1f,	1f * 0.1f,	1.5f * 0.1f,	0.5f) ;
	private Vector4 _modelColor7 = new Vector4(1.9f * 0.5f,	1f,			1.5f,			0.7f) ;
	private Vector4 _modelColor8 = new Vector4(1.9f,			1f * 0.5f,	1.5f,			0.9f) ;
	private Vector4 _modelColor9 = new Vector4(1.9f,			1f,			1.5f * 0.5f,	1.2f) ;
	private Vector4 _modelColor10 = new Vector4(1.9f * 0.1f,	1f,			1.5f,			-0.7f) ;
	private Vector4 _modelColor11 = new Vector4(1.9f,			1f * 0.1f,	1.5f,			-0.5f) ;
	private Vector4 _modelColor12 = new Vector4(1.9f,			1f,			1.5f * 0.1f,	-0.3f) ;
	private Vector4 _modelColor13 = new Vector4(1.9f * 0.1f,	1f * 0.1f,	1.5f,			0f) ;
	private Vector4 _modelColor14 = new Vector4(1.9f,			1f * 0.1f,	1.5f * 0.1f,	0.2f) ;
	private Vector4 _modelColor15 = new Vector4(1.9f * 0.1f,	1f,			1.5f * 0.1f,	0.4f) ;
	private Vector4 _modelColor16 = new Vector4(1.9f * 0.1f,	1f * 0.1f,	1.5f * 0.1f,	0.5f) ;
	private Vector4 _modelColor17 = new Vector4(1.9f * 0.5f,	1f,			1.5f,			0.7f) ;
	private Vector4 _modelColor18 = new Vector4(1.9f,			1f * 0.5f,	1.5f,			0.9f) ;
	private Vector4 _modelColor19 = new Vector4(1.9f,			1f,			1.5f * 0.5f,	1.2f) ;
	private Vector4[] _fixColors;
	private Vector4[] _modelColors;

	private Queue<KeyValuePair<float, int>> _beatsQueue;
	private KeyValuePair<float, int> _curBeat;

	private float _beatTimer;
	private float _waveTimer;


	private const float TimeScale = 0.3f;
	private const float PI = 3.1415926f;
	private const float PI2 = PI * 2f;


	private void Awake ()
	{
		_mat = GetComponent<UnityEngine.UI.RawImage> ().material;


		_fixColors = new Vector4[(int)Count];
		for (int i = 0; i < Count; i++) {
			_fixColors [i] = _fixColorZero;
		}
		_modelColors = new Vector4[(int)Count]
		{
			_modelColor0, _modelColor1, _modelColor2, _modelColor3, _modelColor4, _modelColor5, _modelColor6, _modelColor7, _modelColor8, _modelColor9,
			_modelColor10, _modelColor11, _modelColor12, _modelColor13, _modelColor14, _modelColor15, _modelColor16, _modelColor17, _modelColor18, _modelColor19
		};

		//_fixColors = _modelColors;

		_beatTimer = _waveTimer = 0f;

		_mat.SetInt ("_MaxPointCount", (int)Count);

		InitSampleBeats ();

		_curBeat = _beatsQueue.Dequeue ();
	}

 	private	void Update ()
	{
		_beatTimer += Time.deltaTime;
		_waveTimer += Time.deltaTime;

		if (_waveTimer >= PI2)
			_waveTimer = 0f;

		_mat.SetFloat ("_Timer", _waveTimer);
		Debug.Log (_beatTimer);

		ResetDatasToMat ();
	}

	private void InitSampleBeats()
	{
		_beatsQueue = new Queue<KeyValuePair<float, int>> ();
        int b = 0;
		for (float i = 0f; i < 5000; i = i + 0.5f) 
		{
			KeyValuePair<float, int> beat = new KeyValuePair<float, int> (Random.Range(i - 0.23f, i + 0.23f), (b ++) % (int)Count);
			_beatsQueue.Enqueue(beat);
		}
	}

	private void ResetDatasToMat()
	{

		if (_beatTimer >= _curBeat.Key)
		{
			System.Array.Sort (_fixColors, delegate(Vector4 x, Vector4 y)
			{
				return x.w.CompareTo(y.w);
			});

			for (int i = (int)Count - 1; i > 1; i--) 
			{
				_fixColors [i] = _fixColors [i - 1];
			}

			_fixColors [1] = _modelColors [_curBeat.Value];
			_fixColors [1].w = 0f;
			
			_curBeat = _beatsQueue.Dequeue ();

			_fixColors [0] = _modelColors [_curBeat.Value];
			_fixColors [0].w = 0f; 
			
//			var ds = string.Empty;
//			for (int i = 0; i < Count; i++) {
//				ds += _fixColors [i].w.ToString("0.0000") + " ";
//				if (i > 0) {
//					ds += "[" + (_fixColors [i].w - _fixColors [i - 1].w).ToString("0.0000") + "] ";
//				}
//			}
//			Debug.Log (ds);
		}

		for (int i = 1; i < _fixColors.Length; i++) 
		{
//			if ((Vector3)_fixColors [i] == Vector3.zero)
//				continue;
			
			_fixColors [i].w += Time.deltaTime * TimeScale;
		}


		for (int i = 0; i < Count; i++)
		{
			_mat.SetVector ("_FixColor" + i, _fixColors [i]);
		}

		return;

		/*
		var tmod = Time.time % Count * Delta;
		_mat.SetFloat ("_StepTimerMod", tmod);
		
		if (tmod < Delta * 1) 
		{
			_mat.SetVector ("_FixColor0", _fixColors [0]);
			_mat.SetVector ("_FixColor1", _fixColors [6]);
			_mat.SetVector ("_FixColor2", _fixColors [5]);
			_mat.SetVector ("_FixColor3", _fixColors [4]);
			_mat.SetVector ("_FixColor4", _fixColors [3]);
			_mat.SetVector ("_FixColor5", _fixColors [2]);
			_mat.SetVector ("_FixColor6", _fixColors [1]);
		}
		else if(tmod < Delta * 2)
		{
			_mat.SetVector ("_FixColor0", _fixColors [1]);
			_mat.SetVector ("_FixColor1", _fixColors [0]);
			_mat.SetVector ("_FixColor2", _fixColors [6]);
			_mat.SetVector ("_FixColor3", _fixColors [5]);
			_mat.SetVector ("_FixColor4", _fixColors [4]);
			_mat.SetVector ("_FixColor5", _fixColors [3]);
			_mat.SetVector ("_FixColor6", _fixColors [2]);
		}
		else if(tmod < Delta * 3)
		{
			_mat.SetVector ("_FixColor0", _fixColors [2]);
			_mat.SetVector ("_FixColor1", _fixColors [1]);
			_mat.SetVector ("_FixColor2", _fixColors [0]);
			_mat.SetVector ("_FixColor3", _fixColors [6]);
			_mat.SetVector ("_FixColor4", _fixColors [5]);
			_mat.SetVector ("_FixColor5", _fixColors [4]);
			_mat.SetVector ("_FixColor6", _fixColors [3]);
		}
		else if(tmod < Delta * 4)
		{
			_mat.SetVector ("_FixColor0", _fixColors [3]);
			_mat.SetVector ("_FixColor1", _fixColors [2]);
			_mat.SetVector ("_FixColor2", _fixColors [1]);
			_mat.SetVector ("_FixColor3", _fixColors [0]);
			_mat.SetVector ("_FixColor4", _fixColors [6]);
			_mat.SetVector ("_FixColor5", _fixColors [5]);
			_mat.SetVector ("_FixColor6", _fixColors [4]);
		}
		else if(tmod < Delta * 5)
		{
			_mat.SetVector ("_FixColor0", _fixColors [4]);
			_mat.SetVector ("_FixColor1", _fixColors [3]);
			_mat.SetVector ("_FixColor2", _fixColors [2]);
			_mat.SetVector ("_FixColor3", _fixColors [1]);
			_mat.SetVector ("_FixColor4", _fixColors [0]);
			_mat.SetVector ("_FixColor5", _fixColors [6]);
			_mat.SetVector ("_FixColor6", _fixColors [5]);
		}
		else if(tmod < Delta * 6)
		{
			_mat.SetVector ("_FixColor0", _fixColors [5]);
			_mat.SetVector ("_FixColor1", _fixColors [4]);
			_mat.SetVector ("_FixColor2", _fixColors [3]);
			_mat.SetVector ("_FixColor3", _fixColors [2]);
			_mat.SetVector ("_FixColor4", _fixColors [1]);
			_mat.SetVector ("_FixColor5", _fixColors [0]);
			_mat.SetVector ("_FixColor6", _fixColors [6]);
		}
		else 
		{
			_mat.SetVector ("_FixColor0", _fixColors [6]);
			_mat.SetVector ("_FixColor1", _fixColors [5]);
			_mat.SetVector ("_FixColor2", _fixColors [4]);
			_mat.SetVector ("_FixColor3", _fixColors [3]);
			_mat.SetVector ("_FixColor4", _fixColors [2]);
			_mat.SetVector ("_FixColor5", _fixColors [1]);
			_mat.SetVector ("_FixColor6", _fixColors [0]);
		}
		*/
	}


		
}
