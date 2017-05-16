using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class Mover : MonoBehaviour {
    
    Animator m_Animator;

    Vector3 lastPos;

    // Use this for initialization
    void Start () {
        m_Animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        m_Animator.SetFloat("Speed", 0.0f);

		float dist = (Vector3.Distance (lastPos, transform.position) / Time.deltaTime * 0.3f);

		if (dist > 0.5f) {
            m_Animator.SetFloat("Speed", dist);
		} else {
			m_Animator.SetFloat("Speed", 0.0f);
		}
	}

    void LateUpdate()
    {

        lastPos = transform.position;
    }
}
