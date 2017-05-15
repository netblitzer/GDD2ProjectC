using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorStartScript : MonoBehaviour {

    bool elevator;
    Vector3 startingposition;
    Vector3 endingposition;
    float startTime;
    private float journeyLength;

    // Use this for initialization
    void Start () {
        elevator = true;
        startingposition = new Vector3(0.05f, 8.83f, 34.22f);
        endingposition = new Vector3(0.05f, -0.25f, 34.41f);
        startTime = Time.time;
        journeyLength = Vector3.Distance(startingposition, endingposition);
    }
	
	// Update is called once per frame
	void Update () {
		if(elevator == true & transform.position != endingposition)
        {
            float speed = 1.0F;
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            Vector3 lerp = Vector3.Lerp(startingposition, endingposition, fracJourney);
            transform.position = lerp;
        }
        else
        {
            transform.GetChild(2).gameObject.GetComponent<Collider>().enabled = false;
            transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(7).gameObject.GetComponent<Camera>().enabled = false;
        }
	}
}
