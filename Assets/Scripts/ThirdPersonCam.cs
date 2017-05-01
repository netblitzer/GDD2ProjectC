using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour {

	private const float Y_ANGLE_MIN = 5.0f;
	private const float Y_ANGLE_MAX = 35.0f;
	private const float X_ANGLE_MIN = -20.0f;
	private const float X_ANGLE_MAX = 20.0f;

	// lookAt is player transform
	public Transform lookAt;
	public Transform camTrans;

	private Camera cam;

    public Texture2D crosshair;

	private float dist = 7.5f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
    //	private float sensitivityX = 4.0f;
    //	private float sensitivityY = 1.0f;

	// Use this for initialization
	private void Start () {
		camTrans = transform;
		cam = Camera.main;
	}

	private void Update(){
		currentX += Input.GetAxis("Mouse X");
		currentY -= Input.GetAxis("Mouse Y");

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		currentX = Mathf.Clamp(currentX, X_ANGLE_MIN, X_ANGLE_MAX);
	}
	
	private void LateUpdate () {
		Vector3 dir = new Vector3(0,0,-dist);
		Quaternion rot = Quaternion.Euler(currentY, currentX, 0);

        Vector3 targetPos = lookAt.position;
        targetPos.y += 2.5f;
        
        camTrans.position = targetPos + lookAt.rotation * rot * dir;
		camTrans.LookAt(lookAt.position);
	}

    private void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crosshair.width / 2);
        float yMin = (Screen.height * 1 / 3) - (crosshair.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshair.width, crosshair.height), crosshair);
    }
}
