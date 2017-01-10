using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YourWaveScript : MonoBehaviour
{
	private Material _mat;

	private const float Count = 7;
	private const float Delta = 1 / (Count - 1f);

	//x y z means color, w means pos of uv.y(or x)
	private Vector4 _fixColorBase = new Vector4(1.9f,		1f,			1.5f,			1f) ;
	private Vector4 _fixColorZero = new Vector4(0f,		0f,			0f,			1.2f) ;

	private Vector4 _fixColor0 = new Vector4(1.9f * 0.1f,	1f,			1.5f,			-0.2f) ;
	private Vector4 _fixColor1 = new Vector4(1.9f,			1f * 0.1f,	1.5f,			0.3f) ;
	private Vector4 _fixColor2 = new Vector4(1.9f,			1f,			1.5f * 0.1f,	0.5f) ;
	private Vector4 _fixColor3 = new Vector4(1.9f * 0.1f,	1f * 0.1f,	1.5f,			0.7f) ;
	private Vector4 _fixColor4 = new Vector4(1.9f,			1f * 0.1f,	1.5f * 0.1f,	0.8f) ;
	private Vector4 _fixColor5 = new Vector4(1.9f * 0.1f,	1f,			1.5f * 0.1f,	0.9f) ;
	private Vector4 _fixColor6 = new Vector4(1.9f * 0.1f,	1f * 0.1f,	1.5f * 0.1f,	1.2f) ;
	private Vector4[] _fixColors;
	private Vector4[] _modelColors;

	private Queue<KeyValuePair<float, int>> _beatsQueue;
	private KeyValuePair<float, int> _curBeat;
	private int _floatIndex;

	private const float TimeScale = 0.1f;

	
	private void Awake ()
	{
		_mat = GetComponent<UnityEngine.UI.RawImage> ().material;


		_fixColors = new Vector4[(int)Count]
		{
			_fixColorZero, _fixColorZero, _fixColorZero, _fixColorZero, _fixColorZero, _fixColorZero, _fixColorZero
		};
		_modelColors = new Vector4[(int)Count]
		{
			_fixColor0, _fixColor1, _fixColor2, _fixColor3, _fixColor4, _fixColor5, _fixColor6
		};

		//_fixColors = _modelColors;

		_mat.SetInt ("_MaxPointCount", _fixColors.Length);

		InitSampleBeats ();

		_curBeat = _beatsQueue.Dequeue ();
		_floatIndex = 7;
	}

 	private	void Update ()
	{

		_mat.SetFloat ("_Timer", Time.time);
		

//		for (int i = 0; i < _fixColors.Length; i++) 
//		{
//			_fixColors [i].w += Time.deltaTime * 0.1f;
//		}

		ResetDatasToMat ();
	}

	private void InitSampleBeats()
	{
		_beatsQueue = new Queue<KeyValuePair<float, int>> ();
		for (float i = 0f; i < 100f; i = i + 0.2f) 
		{
			KeyValuePair<float, int> beat = new KeyValuePair<float, int> (i, Random.Range (0, _fixColors.Length));
			_beatsQueue.Enqueue(beat);
		}
	}

	private void ResetDatasToMat()
	{

		if (Time.time * TimeScale >= _curBeat.Key) 
		{
			//加了之后为1，之所以不从0开始，是为了在上界上有个
			_floatIndex--;
			if (_floatIndex < 0)
				_floatIndex = 6;
			
			_fixColors [_floatIndex] = _modelColors [_curBeat.Value];
			_fixColors [_floatIndex].w = 0f;

			_curBeat = _beatsQueue.Dequeue ();

			int nexIdx = _floatIndex - 1;
			if (nexIdx < 0)
				nexIdx = 6;
			_fixColors [nexIdx] = _modelColors [_curBeat.Value];
			_fixColors [nexIdx].w = -0.2f;
		}

		for (int i = 0; i < _fixColors.Length; i++) 
		{
			if ((Vector3)_fixColors [i] == Vector3.zero)
				continue;
			
			_fixColors [i].w += Time.deltaTime * TimeScale;
		}

		_mat.SetVector ("_FixColor0", _fixColors [0]);
		_mat.SetVector ("_FixColor1", _fixColors [1]);
		_mat.SetVector ("_FixColor2", _fixColors [2]);
		_mat.SetVector ("_FixColor3", _fixColors [3]);
		_mat.SetVector ("_FixColor4", _fixColors [4]);
		_mat.SetVector ("_FixColor5", _fixColors [5]);
		_mat.SetVector ("_FixColor6", _fixColors [6]);



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
