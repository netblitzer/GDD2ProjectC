using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndTransition : MonoBehaviour {
    public static float numFol=0;
    private bool lastscreen = false;
    private bool deathscreen = true;
	// Use this for initialization
	void Start () {
        if(SceneManager.GetActiveScene().name=="EndScene")
        {
            GameObject.FindGameObjectWithTag("Floor").GetComponent<TextMesh>().text = "" + numFol + " Yetis saved out of 5.";
            lastscreen = true;
        }
        else if(SceneManager.GetActiveScene().name == "DeathScene")
        {
            deathscreen = true;
        }
    }

    private void Update()
    {
        if(lastscreen == true || deathscreen == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
                print("application quit");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                numFol = 0;
                SceneManager.LoadScene("Level Test");
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject[] following = GameObject.FindGameObjectsWithTag("Yeti");
        if (following.Length > 0)
        {
            for (int i = 0; i < following.Length; i++)
            {
                if (following[i].GetComponent<YetiFlocker>().following)
                {
                    numFol++;
                }
            }
        }
        SceneManager.LoadScene("EndScene");

    }
}
