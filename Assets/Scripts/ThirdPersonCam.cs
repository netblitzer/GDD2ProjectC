using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour {

	private const float Y_ANGLE_MIN = -15.0f;
	private const float Y_ANGLE_MAX = 30.0f;
	private const float X_ANGLE_MIN = -30.0f;
	private const float X_ANGLE_MAX = 30.0f;

	// lookAt is player transform
	public Transform lookAt;
	public Transform camTrans;

    private Vector2 mousePos;

	private Camera cam;
    

	private float dist = 4.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
    //	private float sensitivityX = 4.0f;
    //	private float sensitivityY = 1.0f;

	// Use this for initialization
	private void Start () {
		camTrans = transform;
		cam = Camera.main;
        mousePos.x = (Screen.width / 2) / Screen.width;
        mousePos.y = (Screen.height / 2) / Screen.height;
    }

	private void Update(){
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            mousePos.x = Input.GetAxis("Mouse X");
            currentX = 0;
        }

        currentX += mousePos.x - Input.GetAxis("Mouse X");
		currentY -= -(mousePos.y - Input.GetAxis("Mouse Y"));

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		currentX = Mathf.Clamp(currentX, X_ANGLE_MIN, X_ANGLE_MAX);
	}
	
	private void LateUpdate () {
		Vector3 dir = new Vector3(0,0,-dist);
        Quaternion rot;

        Vector3 targetPos = lookAt.position;
        targetPos.y += 2.5f;

        rot = Quaternion.Euler(currentY, currentX, 0);
        camTrans.position = targetPos + lookAt.rotation * rot * dir;
        
        // rotates player with mouse
        
        //Vector3 forward = camTrans.forward;
        //forward.y = 0;
        //lookAt.forward = forward;

        camTrans.LookAt(lookAt.position);
	}
}
