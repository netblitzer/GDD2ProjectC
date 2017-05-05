using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;

        private bool yetiHeld;

        private void Start()
        {
            yetiHeld = false;

            m_Cam = Camera.main.transform;

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {

            if (!yetiHeld)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // pick up yeti
                    yetiHeld = true;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // throw yeti
                    yetiHeld = false;
                    m_Character.Throw();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    // drop yeti
                    yetiHeld = false;
                }
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            
            m_Move = v * Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized + h * m_Cam.right;
            
            // pass all parameters to the character control script
            m_Character.Move(m_Move);
        }
    }
}
