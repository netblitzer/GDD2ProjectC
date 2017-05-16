using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        [SerializeField] float m_AnimSpeedMultiplier = 1f;

		public float throwForce = 10f;
		public float throwDistMultiplier = 1f;

        Rigidbody m_Rigidbody;
        Animator m_Animator;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;

        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 distance;

        private bool yetiHeld;
        private bool threw;
		private float yetiRollTimer = 0;
		private GameObject otherYeti;
        
        private float animStart;
        private float animCurrent;

        private Vector3 start;
        private Vector3 end;
        private Vector3 mid;

        private void Start()
        {
            yetiHeld = false;
            threw = false;
            
            m_Cam = Camera.main.transform;

            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;

            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            distance = new Vector3(0.1f, 0.0f, 0.0f);
        }


        private void Update()
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
            m_Animator.ResetTrigger("Throwing");
            m_Animator.SetFloat("Speed", 0.0f);

            if (!yetiHeld)
            {
                if (Input.GetMouseButtonDown(0) && !threw)
                {
                    // pick up yeti
					GameObject[] Yetis = GameObject.FindGameObjectsWithTag("Yeti");
					
					foreach(GameObject Yeti in Yetis){
						if(Vector3.Distance(Yeti.transform.position, transform.position) < 5){
							otherYeti = Yeti;
							Vector3 temp = transform.position;
							temp.y += 6f;
							otherYeti.transform.position = temp;
                            otherYeti.GetComponent<NavMeshAgent>().enabled = false;
                            otherYeti.GetComponent<YetiFlocker>().enabled = false;
							
							yetiHeld = true;
							break;
						}
					}
                }
            }
            else
            {

                Vector3 temp = transform.position;
                temp.y += 6.4f;
                otherYeti.transform.position = temp;
                
                if (Input.GetMouseButtonDown(0))
                {
                    // throw yeti
                    yetiHeld = false;
                    Throw(otherYeti);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    // drop yeti
                    temp = new Vector3(2.0f, 0.0f, 0.0f);
                    temp = Vector3.RotateTowards(temp, transform.forward, (float)(2.0 * Math.PI), 0.25f);
                    otherYeti.transform.position = transform.position - temp;

                    otherYeti.GetComponent<NavMeshAgent>().enabled = true;
                    otherYeti.GetComponent<YetiFlocker>().enabled = true;

                    yetiHeld = false;
                }
            }

            if (threw)
            {
                animCurrent = Time.time;

                float pos = (animCurrent - animStart) / 0.5f;
				if (otherYeti.transform.position.y < 2f)
                {
					yetiRollTimer += Time.deltaTime;

					if (yetiRollTimer > 1) {
						threw = false;
                        otherYeti.GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);
						otherYeti.GetComponent<NavMeshAgent> ().enabled = true;
						otherYeti.GetComponent<YetiFlocker> ().enabled = true;
						otherYeti.GetComponent<Rigidbody> ().useGravity = false;
						otherYeti.GetComponent<CapsuleCollider> ().enabled = true;
						otherYeti.GetComponent<SphereCollider> ().enabled = false;
					}
                }
                
                //otherYeti.transform.position = Vector3.Lerp(Vector3.Lerp(start, mid, pos), Vector3.Lerp(mid, end, pos), pos);
            }

            
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.down * 3f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up * 3f);
            }
            if (Input.GetKey(KeyCode.W) && !yetiHeld)
            {
                if (transform.eulerAngles.y == 270)
                {
                    transform.position -= Vector3.RotateTowards(distance, transform.forward, (float)(2.0 * Math.PI), 0.25f) / 3;
                }
                else
                {
                   transform.position += Vector3.RotateTowards(distance, transform.forward, (float)(2.0 * Math.PI), 0.25f) / 3;
                }
                m_Animator.SetFloat("Speed", 0.1f);
            }
            if (Input.GetKey(KeyCode.S) && !yetiHeld)
            {
                if (transform.eulerAngles.y == 270)
                {
                    transform.position += Vector3.RotateTowards(distance, transform.forward, (float)(2.0 * Math.PI), 0.1f) / 5;
                }
                else
                {
                    transform.position -= Vector3.RotateTowards(distance, transform.forward, (float)(2.0 * Math.PI), 0.1f) / 5;
                }
                m_Animator.SetFloat("Speed", -0.5f);
            }
        }

		public void Throw(GameObject _other)
        {
            threw = true;
            animStart = Time.time;
			yetiRollTimer = 0;

			float dist = (m_Cam.transform.position.y - 3) / 2f / throwDistMultiplier;
			dist = throwForce - dist;

            start = otherYeti.transform.position;
            end = transform.position + Vector3.RotateTowards(new Vector3(0.0f, 0.0f, dist), transform.forward, (float)(2.0 * Math.PI), 1.0f);
            mid = (start + end) / 2;

            mid.y = 6.0f;
            m_Animator.SetTrigger("Throwing");
			_other.GetComponent<Rigidbody> ().AddForce (gameObject.transform.forward * throwForce, ForceMode.Impulse);
			_other.GetComponent<Rigidbody> ().useGravity = true;
			_other.GetComponent<CapsuleCollider> ().enabled = false;
			_other.GetComponent<SphereCollider> ().enabled = true;
        }
    }
}
