using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TwoFace : MonoBehaviour {

    public GameObject target = null;
    public bool chasing = false;

    public List<PathNode> Nodes;
	public Vector3 preferredDirection = Vector3.forward;

	private Vector3 lastSeenDirection = Vector3.forward;
	private Vector3 checkDirection = Vector3.forward;
	private bool checkPositive = true;

    private int curNode;
    public float speed = 5;
    public float chaseSpeed = 25;
    public float accel = 5;
    public float chaseAccel = 10;
    public float turnSpeed = 2;
    public float chaseTurnSpeed = 0.25f;

	private NavMeshAgent agent;
    private Vector3 pos;

    public float nodeWaitTime = 1f;
    public float reaquireTime = 2f;
	public float minChaseTime = 1f;

    private float nodeWaiting = 0f;
    private float reaquiring = 0f;
	private float chaseTimer = 0f;
    private bool sitting = false;

    private AudioSource source;
    private AudioClip charge;
    private AudioClip confuse;
    private float chargeTimer = 0f;
    private float chargeLimit = 0.68f;
    private float confuseTimer = 0f;
    private float confuseLimit = 0.68f;

    // Use this for initialization
    void Start () {
        target = null;
        chasing = false;

		if (Random.Range (0, 1) > 0.5f) {
			checkPositive = true;
		} else {
			checkPositive = false;
		}

        curNode = 0;
        nodeWaiting = 0f;
        reaquiring = 0f;
        sitting = false;

        pos = gameObject.transform.position;

        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.acceleration = accel;
        agent.angularSpeed = 0;

        //agent.SetDestination(Nodes[curNode].Position);

        source = GetComponent<AudioSource>();
        charge = Resources.Load<AudioClip>("TwoFaced/KillCharge");
        confuse = Resources.Load<AudioClip>("TwoFaced/Confuse");
    }

    void Init (float _speed, float _chaseSpeed, float _nodeWaitTime, float _reaquireTime, GameObject _target) {
        speed = _speed;
        chaseSpeed = _chaseSpeed;
        agent.speed = _speed;
        nodeWaitTime = _nodeWaitTime;
        reaquireTime = _reaquireTime;

        target = _target;
    }

	// returns the angle between this and the _other object
    float findAng (GameObject _other) {

		return Mathf.Rad2Deg * (Mathf.Acos(Vector3.Dot((pos - _other.transform.position).normalized, gameObject.transform.forward)) - Mathf.PI / 2);
    }
    float findAng (Vector3 _dir) {

        return Mathf.Rad2Deg * (Mathf.Acos(Vector3.Dot(_dir.normalized, gameObject.transform.forward)) - Mathf.PI / 2);
    }
	
	// Update is called once per frame
	void Update () {

		pos = gameObject.transform.position;

		if (target || chasing) {

			if (!chasing) {

				Vector3 dir = (target.transform.position - pos).normalized;
				Vector3 rotateDir = Vector3.RotateTowards (transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f);
				transform.rotation = Quaternion.LookRotation (rotateDir);

                float ang = findAng(target);

				// facing the target basically
				if (ang > 80 || ang == float.NaN) {
				
					chasing = true;
					agent.speed = chaseSpeed;
					agent.acceleration = chaseAccel;
					agent.angularSpeed = chaseTurnSpeed;

					chaseTimer = minChaseTime;

                    agent.SetDestination(transform.position + transform.forward * 4);

                } else {
					agent.velocity *= 0.95f;
				}
            } else {
				// now 'chasing' the target

				// make sure we still have it in sights
				if (target) {
					lastSeenDirection = (target.transform.position - pos).normalized;
					Vector3 rotateDir = Vector3.RotateTowards (transform.forward, lastSeenDirection, chaseTurnSpeed * Time.deltaTime, 0.0f);
					transform.rotation = Quaternion.LookRotation (rotateDir);

                    agent.SetDestination(transform.position + transform.forward * 4);

                    chaseTimer = minChaseTime;

                    chargeTimer += Time.deltaTime;
                    if (chargeTimer > chargeLimit)
                    {
                        source.PlayOneShot(charge, .125f);
                        chargeTimer -= chargeLimit;
                    }

                } else if (chaseTimer >= 0) {
                    agent.SetDestination(transform.position + transform.forward * 4);

                    chaseTimer -= Time.deltaTime;
					if (chaseTimer < 0) {
						reaquiring = reaquireTime;

                        if (Random.Range(0, 1) > 0.5f) {
                            checkPositive = true;
                        }
                        else {
                            checkPositive = false;
                        }

                        if (!checkPositive) {
							checkDirection = Quaternion.Euler (0, -Random.Range(40, 70), 0) * lastSeenDirection;
						} else {
							checkDirection = Quaternion.Euler (0, Random.Range(40, 70), 0) * lastSeenDirection;
						}

						checkPositive = !checkPositive;
					}
				}

				if (reaquiring >= 0) {
					reaquiring -= Time.deltaTime;

					agent.velocity *= 0.95f;

					// Spin around semi randomly
					Vector3 rotateDir = Vector3.RotateTowards (transform.forward, checkDirection, turnSpeed * Time.deltaTime, 0.0f);
					transform.rotation = Quaternion.LookRotation (rotateDir);

                    float ang = findAng(checkDirection);
                    //Debug.Log (ang);

                    // facing the target basically
                    if (ang < -40 || ang == float.NaN) {
						if (!checkPositive) {
							checkDirection = Quaternion.Euler (0, -Random.Range(80, 140), 0) * lastSeenDirection;
						} else {
							checkDirection = Quaternion.Euler (0, Random.Range(80, 140), 0) * lastSeenDirection;
						}

						checkPositive = !checkPositive;
					}

					if (reaquiring < 0) {
						chasing = false;
						agent.speed = speed;
						agent.acceleration = accel;
						agent.angularSpeed = turnSpeed;
					}

                    confuseTimer += Time.deltaTime;
                    if (confuseTimer > confuseLimit)
                    {
                        source.PlayOneShot(confuse, .125f);
                        confuseTimer -= confuseLimit;
                    }
                }
			}

		}
		else if (target == null && Nodes.Count > 1) {

			if (nodeWaiting >= 0 && sitting) {
				
				nodeWaiting -= Time.deltaTime;
                agent.velocity *= 0.95f;
                
                Vector3 dir = (Nodes[curNode].Position - pos).normalized;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f));

            }
            else if (nodeWaiting < 0 && sitting) {
                agent.speed = speed;
                agent.acceleration = accel;
                agent.angularSpeed = turnSpeed;

                sitting = false;
                nodeWaiting = 0f;

                //agent.SetDestination(Nodes[curNode].Position);
			}
            else {

				Vector3 dir = (Nodes [curNode].Position - pos).normalized;
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f));

				float ang = findAng(Nodes [curNode].gameObject);
				//Debug.Log (ang);

				if(ang > 70 || ang == float.NaN) {

                    agent.SetDestination(transform.position + transform.forward * 2);

                    int walkMask = 1 << NavMesh.GetAreaFromName("Walkable");
                    NavMeshHit hit;
                    //agent.SamplePathPosition(walkMask, (pos + transform.forward * agent.speed), out hit);
					//agent.SetDestination(hit.position);
				} else if (ang < 0) {
					agent.velocity *= 0.95f;
				}

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

				Vector3 dir = (Nodes [curNode].Position - pos).normalized;
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f));

                float ang = findAng(Nodes[curNode].gameObject);

                agent.SetDestination(transform.position + transform.forward * 4);
            } else {
				agent.velocity *= 0.95f;

				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, preferredDirection, turnSpeed * Time.deltaTime, 0.0f));
            }
        }
        else {

        }
	}

	void OnCollisionEnter (Collision _col) {

		GameObject _other = _col.gameObject;

		if (_other.tag == "Yeti") {
			// Yeti death
			_other.GetComponent<YetiFlocker>().kill ();
			target = null;

		} else if (_other.tag == "Player") {
			// Player death

		}
	}
}
