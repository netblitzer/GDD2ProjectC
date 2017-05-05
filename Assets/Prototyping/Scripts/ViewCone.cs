using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCone : MonoBehaviour {

	private TwoFace parent;

	void Start () {
		parent = gameObject.GetComponentInParent<TwoFace> ();
	}

	void OnTriggerStay (Collider _other) {

        Debug.Log("Collider hit");

        if (_other.tag == "Yeti" || _other.tag == "Player") {

            Vector3 dir = (_other.transform.position - parent.transform.position).normalized;
            Ray check = new Ray(parent.transform.position, dir);
            RaycastHit hit;
            Physics.Raycast(check, out hit, float.MaxValue);

            if (hit.collider == _other) {
                if (parent.target) {

                    float dist = Vector3.Distance(parent.transform.position, _other.transform.position);
                    float curDist = Vector3.Distance(parent.transform.position, parent.target.transform.position);

                    if (dist < curDist) {
                        parent.target = _other.gameObject;
                    }
                }
                else {
                    parent.target = _other.gameObject;
                }
			} else if (parent.target) {

				// lost sight because it went behind cover
				if (_other == parent.target) {
					parent.target = null;
				}
			}
		}
	}

    void OnTriggerExit (Collider _other) {
        
        if (_other.tag == "Yeti" || _other.tag == "Player") {

            if (parent.target == _other.gameObject) {

                parent.target = null;
            }
        }
    }
}
