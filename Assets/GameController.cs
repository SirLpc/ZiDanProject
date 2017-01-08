using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


	void Start ()
	{
		Application.runInBackground = true;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
//	void Update()
//	{
//		Debug.Log(string.Format("Sin is {0} when time is{1}", Mathf.Sin(Time.time), Time.time));
//	}

}
