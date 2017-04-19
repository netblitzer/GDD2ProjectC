using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    void OnDrawGizmos () {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, 1);
    }
}
