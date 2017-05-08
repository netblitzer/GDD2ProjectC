using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        [SerializeField] float m_MoveSpeedMultiplier = 1f;
        [SerializeField] float m_AnimSpeedMultiplier = 1f;

        Rigidbody m_Rigidbody;
        Animator m_Animator;
        float m_ForwardAmount;
        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;

        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 move;
        private Vector3 distance;

        private bool yetiHeld;
		private GameObject otherYeti;

        private void Start()
        {
            yetiHeld = false;

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

            if (!yetiHeld)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // pick up yeti
					GameObject[] Yetis = GameObject.FindGameObjectsWithTag("Yeti");
					
					foreach(GameObject Yeti in Yetis){
						if(Vector3.Distance(Yeti.transform.position, transform.position) < 5){
							otherYeti = Yeti;
							Vector3 temp = transform.position;
							temp.y += 2.0f;
							Yeti.transform.position = temp;
							
							yetiHeld = true;
							break;
						}
					}
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // throw yeti
                    yetiHeld = false;
                    Throw();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    // drop yeti
                    yetiHeld = false;
                }
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
                transform.position += Vector3.RotateTowards(distance, transform.forward, (float)(2.0 * Math.PI), 0.25f);
            }
            if (Input.GetKey(KeyCode.S) && !yetiHeld)
            {
                transform.position -= Vector3.RotateTowards(distance, transform.forward, (float)(2.0 * Math.PI), 0.1f) / 2;
            }
        }
		
		private void YetiArc(GameObject yeti){
			
		}


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            
            move = v * Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized + h * m_Cam.right;
            
    
            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_ForwardAmount = Mathf.Abs(move.z);


            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            // update the animator parameters
            m_Animator.SetFloat("Speed", m_ForwardAmount, 0.1f, Time.deltaTime);

            m_Animator.ResetTrigger("Throwing");

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            m_Animator.speed = m_AnimSpeedMultiplier;
        }

        public void Throw()
        {
            m_Animator.SetTrigger("Throwing");
        }
    }
}
