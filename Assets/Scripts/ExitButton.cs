using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnMouseUp()
    {
        Application.Quit();
        print("exitbuttontriggered");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
