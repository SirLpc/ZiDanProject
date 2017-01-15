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
	private Vector4 _fixColorZero = new Vector4(0f,		0f,			0f,			0.1f) ;

	private Vector4 _modelColor0 = new Vector4(1f,	0f,	0f,			-0.7f) ;
	private Vector4 _modelColor1 = new Vector4(1f,	0.651f,	0f,			-0.5f) ;
	private Vector4 _modelColor2 = new Vector4(1f,	1f,	0f,	-0.3f) ;
	private Vector4 _modelColor3 = new Vector4(0f,	1f,	0f,			0f) ;
	private Vector4 _modelColor4 = new Vector4(0f,	0.498f,	1f,	0.2f) ;
	private Vector4 _modelColor5 = new Vector4(0f,	0f,		1f,	0.4f) ;
	private Vector4 _modelColor6 = new Vector4(0.545f,	0f,	1f,	0.5f) ;
	private Vector4 _modelColor7 = new Vector4(0f,		0f,			0f,			1f) ;

	private Vector4[] _fixColors;
	private Vector4[] _modelColors;

	private List<KeyValuePair<float, int>> _beatsQueue;
	private KeyValuePair<float, int> _curBeat;
	private int _curBeatIndex;
	private int _curScoreIndex;

	private float _beatTimer;
	private float _waveTimer;
	private float _scoreTimer;
	
	private const float TimeScale = 0.2f;
	private const float PI = 3.1415926f;
	private const float PI2 = PI * 2f;

	private const float FirstBeatOffset = -3.828f;
	private const float PerfectThreshold = 0.1f;
	private const float AcceptableThreshold = 0.2f;

	private bool _isPlaying;


	private int _score = 0;


	private void Awake ()
	{
		_mat = GetComponent<UnityEngine.UI.RawImage> ().material;

		_isPlaying = false;
		
		_fixColors = new Vector4[(int)Count];
		for (int i = 0; i < Count; i++) {
			_fixColors [i] = _fixColorZero;
		}
		_modelColors = new Vector4[7]
		{
			_modelColor0, _modelColor1, _modelColor2, _modelColor3, _modelColor4, _modelColor5, _modelColor6
		};

		//_fixColors = _modelColors;

		_mat.SetInt ("_MaxPointCount", (int)Count);

		InitSampleBeats ();


		_beatTimer = _waveTimer = 0f;
		_scoreTimer = FirstBeatOffset;

		_curScoreIndex = 0;
		_curBeatIndex = 0;

		_curBeat = _beatsQueue[_curBeatIndex];
	}

 	private	void Update ()
	{
		if (!_isPlaying) 
		{
			if (Input.GetMouseButtonDown(0))
				_isPlaying = !_isPlaying;
			
			return;
		}

		UpdateTimer ();
		
		ResetDatasToMat ();

		CheckScore ();
	}

	private void InitSampleBeats()
	{
		_beatsQueue = new List<KeyValuePair<float, int>> ();
        int b = 0;
		for (float i = 0f; i < 5000; i = i + 1f) 
		{
			KeyValuePair<float, int> beat = new KeyValuePair<float, int> (i, (b ++) % 7);
			//KeyValuePair<float, int> beat = new KeyValuePair<float, int> (Random.Range(i - 0.23f, i + 0.23f), (b ++) % 7);
			_beatsQueue.Add(beat);
		}
	}

	private void UpdateTimer()
	{
		_beatTimer += Time.deltaTime;
		_waveTimer += Time.deltaTime;

		HUDView.Instance.DisplayTopText (
			string.Format ("[BT:{0}][ST:{1}][S:{2}]", _beatTimer.ToString ("0.000"), _scoreTimer.ToString ("0.000"), _score));

		if (_waveTimer >= PI2)
			_waveTimer = 0f;

		_mat.SetFloat ("_Timer", _waveTimer);
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

			_curBeat = _beatsQueue [++_curBeatIndex];

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

		for (int i = 0; i < _fixColors.Length; i++) 
		{
			if (_fixColors [i].w > 2f)
				break;

			if(i > 0)
				_fixColors [i].w += Time.deltaTime * TimeScale;
			_mat.SetVector ("_FixColor" + i, _fixColors [i]);
		}	
	}

	private void CheckScore()
	{
		_scoreTimer += Time.deltaTime;
		float timeDelta = _scoreTimer - _beatsQueue [_curScoreIndex].Key;
		if (timeDelta > AcceptableThreshold) 
		{
			_curScoreIndex++;
		}
	

		if (Input.GetMouseButtonDown (0))
		{
			if(Mathf.Abs(timeDelta) < PerfectThreshold)
			{
				_score += 2;
				HUDView.Instance.DisplayScore (true);
				return;
			}
			if(Mathf.Abs(timeDelta) < AcceptableThreshold)
			{
				HUDView.Instance.DisplayScore (false);
				_score++;
			}
		}
	}
		
}
