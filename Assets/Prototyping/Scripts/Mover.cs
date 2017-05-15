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

		if(Vector3.Distance(lastPos, transform.position) > 0.01f)
        {
            m_Animator.SetFloat("Speed", 0.1f);
        }
	}

    void LateUpdate()
    {

        lastPos = transform.position;
    }
}
