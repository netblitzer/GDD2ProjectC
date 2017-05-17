using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {
    public GameObject player;

    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        Vector3 newPos = player.transform.position;
        newPos.y += 40;

        transform.position = newPos;
	}
}
