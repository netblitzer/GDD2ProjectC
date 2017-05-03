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

	private Camera cam;

    public Texture2D crosshair;

	private float dist = 5.0f;
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
        Quaternion rot;

        Vector3 targetPos = lookAt.position;
        targetPos.y += 3.0f;

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            rot = Quaternion.Euler(currentY, currentX, 0);
            camTrans.position = targetPos + lookAt.rotation * rot * dir;
        }
        else
        {
            rot = Quaternion.Euler(currentY, 0, 0);
            camTrans.position = targetPos + lookAt.rotation * rot * dir;
        }

        camTrans.LookAt(lookAt.position);
	}

    private void OnGUI()
    {
        float xMouse = (Input.mousePosition.x) - (crosshair.width / 2);
        float yMouse = (Input.mousePosition.y) + (crosshair.height / 2);
        GUI.DrawTexture(new Rect(xMouse, Screen.height - yMouse, crosshair.width, crosshair.height), crosshair);
    }
}
