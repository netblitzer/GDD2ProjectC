using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    public Vector3 Position;

    void Start () {
        Position = gameObject.transform.position;
		Position.y -= 0.5f;
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, 1);
    }
}
