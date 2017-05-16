using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTransition : MonoBehaviour {
    public static float numFol=0;
	// Use this for initialization
	void Start () {

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
