using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDView : MonoBehaviour 
{
	[SerializeField]
	private Text _topText;
	[SerializeField]
	private Text _score;

	public static HUDView Instance = null;

	private void Awake()
	{
		Instance = this;
	}

	public void DisplayTopText (string msg)
	{
		_topText.text = msg;
	}

	public void DisplayScore(bool perfect)
	{
		var dis = perfect ? "完美!!!" : "得分!";
		var scale = perfect ? Vector3.one * 2 : Vector3.one; 

		StopAllCoroutines ();

		_score.text = dis;
		_score.transform.localScale = scale;
		_score.gameObject.SetActive (true);

		StartCoroutine (CoHideScore());
	}

	private IEnumerator CoHideScore()
	{
		yield return new WaitForSeconds (.7f);
		_score.gameObject.SetActive (false);
	}
}
