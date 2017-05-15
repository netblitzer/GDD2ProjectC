using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class YetiFlocker : MonoBehaviour {

	public GameObject Player;

	public float speed = 7;
	public float turnSpeed = 2;
	public float followDistance = 3;
	public float fleeDistance = 5;
	public float fleeToDistance = 7;
	public float cowerTime = 5;		// cowers for how many seconds
	private float trueFollowDistance;

	public bool found = false;
	public bool following = false;
	public bool fleeing = false;
	public bool cowering = false;
	public bool dying = false;

	private GameObject fleeTarget;
    private int fleeCheck = 0;
	private GameObject[] enemies;
	private float cowerTimeCounter = 0;
	private float deathTimer = 0;

	private NavMeshAgent agent;
	private Vector3 pos;

	// Use this for initialization
	void Start () {
		trueFollowDistance = followDistance - Random.Range (0, 1f);

		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.speed = speed;
		agent.acceleration = 10;
		agent.angularSpeed = turnSpeed;

		enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

	float findAng (GameObject _other) {

		return Mathf.Rad2Deg * (Mathf.Acos(Vector3.Dot((pos - _other.transform.position).normalized, gameObject.transform.forward)) - Mathf.PI / 2);
	}

	void fleeUpdate () {

        fleeCheck = (fleeCheck + 1) % 5;

		if (fleeCheck == 0) {
			float closest = fleeDistance;
			int index = -1;
			for (int i = 0; i < enemies.Length; i++) {
				float distance = Vector3.Distance(enemies[i].transform.position, pos);

				if (distance < closest) {
					distance = closest;

					index = i;
				}
            }

            if (index > -1) {
				fleeTarget = enemies [index];
				fleeing = true;
            }
        }

        if (fleeTarget && fleeing) {
            float distance = Vector3.Distance(fleeTarget.transform.position, pos);

            // flee
            if (distance < fleeToDistance) {
                cowering = false;

                Vector3 dir = (pos - fleeTarget.transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f));

                agent.SetDestination(transform.position + transform.forward * 2);
            }
            else {

                if (!cowering) {
                    cowerTimeCounter = 0;

                    cowering = true;
                }
                else {
                    cowerTimeCounter += Time.deltaTime;

                    if (cowerTimeCounter > cowerTime) {
                        cowering = false;
                        fleeing = false;
                        fleeTarget = null;
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

		if (!dying) {

			pos = gameObject.transform.position;

			Vector3 playerPos = Player.transform.position;

			if (Vector3.Distance (pos, playerPos) < 10) {
				found = true;
				following = true;
			} else {
				following = false;
			}

			if (found && agent.isActiveAndEnabled) {

				if (following) {

					float distance = Vector3.Distance (Player.transform.position, pos);

					if (distance > trueFollowDistance) {

						Vector3 dir = ((Player.transform.position - Player.transform.forward) - pos).normalized;
						transform.rotation = Quaternion.LookRotation (Vector3.RotateTowards (transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f));

						agent.SetDestination (transform.position + transform.forward * 2);
					} else {

						Vector3 dir = (Player.transform.position - pos).normalized;
						transform.rotation = Quaternion.LookRotation (Vector3.RotateTowards (transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f));

						agent.velocity *= 0.95f;
					}
				} else {
					fleeUpdate ();
				}

			} else if (agent.isActiveAndEnabled) {
				fleeUpdate ();
			}
		} else {

			// let all the particles fade
			if (deathTimer > 1f) {
				Destroy (gameObject);
			}

			deathTimer += Time.deltaTime;
		}

	}

	public void kill () {
		if (deathTimer == 0) {
			gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ()[0].enabled = false;
			gameObject.GetComponent<ParticleSystem> ().Play ();
			gameObject.GetComponent<CapsuleCollider> ().enabled = false;
			agent.enabled = false;
			dying = true;
		}
	}
}
