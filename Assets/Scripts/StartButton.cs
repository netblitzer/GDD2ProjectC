﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnMouseUp()
    {
        Application.LoadLevel("Level Test");
        print("startbuttonhit");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
