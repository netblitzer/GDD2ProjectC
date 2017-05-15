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
		if(lastPos != transform.position)
        {
            m_Animator.SetFloat("Speed", 0.1f);
        }
	}

    void LateUpdate()
    {

        lastPos = transform.position;
    }
}
