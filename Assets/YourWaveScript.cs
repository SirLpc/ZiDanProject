using UnityEngine;
using System.Collections;

public class YourWaveScript : MonoBehaviour
{
	private Material _mat;

	private const float Count = 5;
	private const float Delta = 1 / (Count - 1f);

	private Vector4 _fixColorBase = new Vector4(1.9f,		1f,			1.5f,			1f) ;
	private Vector4 _fixColor0 = new Vector4(1.9f * 0.1f,	1f,			1.5f,			1f) ;
	private Vector4 _fixColor1 = new Vector4(1.9f,			1f * 0.1f,	1.5f,			1f) ;
	private Vector4 _fixColor2 = new Vector4(1.9f,			1f,			1.5f * 0.1f,	1f) ;
	private Vector4 _fixColor3 = new Vector4(1.9f * 0.1f,	1f * 0.1f,	1.5f,			1f) ;
	private Vector4 _fixColor4 = new Vector4(1.9f,			1f * 0.1f,	1.5f * 0.1f,	1f) ;
	private Vector4[] _fixColors;
	
	private void Awake ()
	{
		_mat = GetComponent<UnityEngine.UI.RawImage> ().material;

		_fixColors = new Vector4[(int)Count] 
		{
			_fixColor0, _fixColor1, _fixColor2, _fixColor3, _fixColor4
		};
	}
	
 	private	void Update ()
	{
		_mat.SetFloat ("_Timer", Time.time);
		
		var tmod = Time.time % Count * Delta;
		_mat.SetFloat ("_StepTimerMod", tmod);
				
		if (tmod < Delta * 1) 
		{
			_mat.SetVector ("_FixColor0", _fixColors [0]);
			_mat.SetVector ("_FixColor1", _fixColors [4]);
			_mat.SetVector ("_FixColor2", _fixColors [3]);
			_mat.SetVector ("_FixColor3", _fixColors [2]);
			_mat.SetVector ("_FixColor4", _fixColors [1]);
		}
		else if(tmod < Delta * 2)
		{
			_mat.SetVector ("_FixColor0", _fixColors [1]);
			_mat.SetVector ("_FixColor1", _fixColors [0]);
			_mat.SetVector ("_FixColor2", _fixColors [4]);
			_mat.SetVector ("_FixColor3", _fixColors [3]);
			_mat.SetVector ("_FixColor4", _fixColors [2]);
		}
		else if(tmod < Delta * 3)
		{
			_mat.SetVector ("_FixColor0", _fixColors [2]);
			_mat.SetVector ("_FixColor1", _fixColors [1]);
			_mat.SetVector ("_FixColor2", _fixColors [0]);
			_mat.SetVector ("_FixColor3", _fixColors [4]);
			_mat.SetVector ("_FixColor4", _fixColors [3]);
		}
		else if(tmod < Delta * 4)
		{
			_mat.SetVector ("_FixColor0", _fixColors [3]);
			_mat.SetVector ("_FixColor1", _fixColors [2]);
			_mat.SetVector ("_FixColor2", _fixColors [1]);
			_mat.SetVector ("_FixColor3", _fixColors [0]);
			_mat.SetVector ("_FixColor4", _fixColors [4]);
		}
		else 
		{
			_mat.SetVector ("_FixColor0", _fixColors [4]);
			_mat.SetVector ("_FixColor1", _fixColors [3]);
			_mat.SetVector ("_FixColor2", _fixColors [2]);
			_mat.SetVector ("_FixColor3", _fixColors [1]);
			_mat.SetVector ("_FixColor4", _fixColors [0]);
		}
	}
}
