using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    bool paused;

	// Use this for initialization
	void Start () {
        paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
	}

    void OnGUI()
    {
        if(paused)
        {
            //if it is paused, show pause menu
            GUILayout.Label("Game is paused!");
        }
    }

    void TogglePause()
    {
        if (paused)
            paused = false;
        if (!paused)
            paused = true;
    }
}
