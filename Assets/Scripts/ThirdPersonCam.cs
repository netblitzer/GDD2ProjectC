using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour {

	private const float Y_ANGLE_MIN = 0.0f;
	private const float Y_ANGLE_MAX = 50.0f;
	private const float X_ANGLE_MIN = -30.0f;
	private const float X_ANGLE_MAX = 30.0f;

	// lookAt is player transform
	public Transform lookAt;
	public Transform camTrans;

	private Camera cam;

	private float dist = 10.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
	private float sensitivityX = 4.0f;
	private float sensitivityY = 1.0f;

	// Use this for initialization
	private void Start () {
		camTrans = transform;
		cam = Camera.main;
	}

	private void update(){
		currentX += Input.GetAxis("Mouse X");
		currentY += Input.GetAxis("Mouse Y");

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		currentX = Mathf.Clamp(currentX, X_ANGLE_MIN, X_ANGLE_MAX);
	}
	
	private void LateUpdate () {
		Vector3 dir = new Vector3(0,0,-dist);
		Quaternion rot = Quaternion.Euler(currentY, currentX, 0);
		camTrans.position = lookAt.position + rot * dir;
		camTrans.LookAt(lookAt.position);
	}
}
