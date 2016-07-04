using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {

	private Vector3 delta;

	void OnEnable(){
		EasyTouch.On_Swipe2Fingers += On_Swipe;
		EasyTouch.On_Drag2Fingers += On_Drag;
		EasyTouch.On_Twist += On_Twist;
		EasyTouch.On_Pinch += On_Pinch;
	}

    private void OnDisable()
    {
        EasyTouch.On_Swipe2Fingers -= On_Swipe;
        EasyTouch.On_Drag2Fingers -= On_Drag;
        EasyTouch.On_Twist -= On_Twist;
        EasyTouch.On_Pinch -= On_Pinch;
    }

    void OnDestroy(){
		EasyTouch.On_Swipe2Fingers -= On_Swipe;
		EasyTouch.On_Drag2Fingers -= On_Drag;
		EasyTouch.On_Twist -= On_Twist;
        EasyTouch.On_Pinch -= On_Pinch;
    }


	void On_Drag (Gesture gesture){
		On_Swipe( gesture);
	}

	void On_Swipe (Gesture gesture){

		transform.Translate( Vector3.left * gesture.deltaPosition.x / Screen.width);
		transform.Translate( Vector3.back * gesture.deltaPosition.y / Screen.height);
	}

    void On_Twist(Gesture gesture)
    {

        transform.Rotate(Vector3.up * gesture.twistAngle);
    }

    void On_Pinch (Gesture gesture){	
		Camera.main.fieldOfView += gesture.deltaPinch * Time.deltaTime;
	}

}
