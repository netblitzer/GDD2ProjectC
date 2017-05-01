using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private bool paused;
    public GameObject pausemenu;

	// Use this for initialization
	void Start () {
        paused = false;
        pausemenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(paused)
            {
                Application.Quit();
                print("application quit");
            }

        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(paused)
            {
                RestartLevel();
                print("Restarting level..");
            }
        }
	}

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void TogglePause()
    {
        if (paused)
        {
            paused = false;
            pausemenu.SetActive(false);
            Time.timeScale = 1.0f;
            print("should be unpaused");
        }

        else //if not paused, make it paused
        {
            paused = true;
            pausemenu.SetActive(true);
            Time.timeScale = 0f;
            print("should be paused");
        }

    }
}
