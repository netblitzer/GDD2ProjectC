using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TwoFace : MonoBehaviour {

    public GameObject target = null;
    public bool chasing = false;

    public List<PathNode> Nodes;
    public Vector3 preferredDirection;

    private int curNode;

    private NavMeshAgent agent;
    private float speed;
    private float chaseSpeed;
    private Vector3 pos;

    public float nodeWaitTime = 1f;
    public float reaquireTime = 2f;

    public float nodeWaiting = 0f;
    public float reaquiring = 0f;
    public bool sitting = false;

	// Use this for initialization
	void Start () {
        target = null;
        chasing = false;

        curNode = 0;
        speed = 10;
        chaseSpeed = 25;
        nodeWaiting = 0f;
        reaquiring = 0f;
        sitting = false;

        pos = gameObject.transform.position;

        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.acceleration = 5;
        agent.angularSpeed = 45;

        agent.SetDestination(Nodes[curNode].Position);
    }

    void Init (float _speed, float _chaseSpeed, float _nodeWaitTime, float _reaquireTime, GameObject _target) {
        speed = _speed;
        chaseSpeed = _chaseSpeed;
        agent.speed = _speed;
        nodeWaitTime = _nodeWaitTime;
        reaquireTime = _reaquireTime;

        target = _target;
    }
	
	// Update is called once per frame
	void Update () {

        pos = gameObject.transform.position;

        if (chasing && target == null) {
            chasing = false;
            agent.speed = speed;
            agent.acceleration = 5;
            agent.angularSpeed = 45;
        }
        else if (!chasing && target != null) {
            chasing = true;
            agent.speed = chaseSpeed;
            agent.acceleration = 20;
            agent.angularSpeed = 10;
        }

        if (target == null && Nodes.Count > 1) {

            if (nodeWaiting >= 0 && sitting) {
                nodeWaiting -= Time.deltaTime;
            } else if (nodeWaiting < 0 && sitting) {
                sitting = false;
                nodeWaiting = 0f;

                agent.SetDestination(Nodes[curNode].Position);
            } else {

                float distance = Vector3.Distance(Nodes[curNode].Position, pos);

                if (distance < 2f) {
                    curNode = (curNode + 1) % Nodes.Count;

                    sitting = true;
                    nodeWaiting = nodeWaitTime;
                }
            }
            
        }
        else if (target == null && Nodes.Count == 1) {
            float distance = Vector3.Distance(Nodes[curNode].Position, pos);

            if (distance > 2f) {
                agent.SetDestination(Nodes[curNode].Position);
            } else {

                Vector3 diff = gameObject.transform.forward - preferredDirection.normalized;

                

            }
        }
        else {

        }
	}
}
