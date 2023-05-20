using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
    public GameObject player;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 randomMoveTarget;
    private Vector3 lastKnownPlayerPosition;

    public float detectionRange;
    public float attackRange;
    public float randomMoveRadius;
    private bool isMovingRandomly;
    private bool canMoveTowardsPlayer;
    private bool canSeePlayer;

    void Start() {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        SetRandomMoveTarget();
        lastKnownPlayerPosition = player.transform.position;
    }

    void Update() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit hit;

        if (distance <= detectionRange && Physics.Raycast(transform.position, player.transform.position - transform.position, out hit) && hit.transform.CompareTag("Player")) {
            canSeePlayer = true;
            isMovingRandomly = false;
            canMoveTowardsPlayer = true;
            Chase();

            // Remember the last known position of the player
            lastKnownPlayerPosition = player.transform.position;
        } else {
            canSeePlayer = false;

            if (navMeshAgent.remainingDistance < 0.1f) {
                if (isMovingRandomly) {
                    SetRandomMoveTarget();
                }
                else if (lastKnownPlayerPosition != Vector3.zero) {
                    // If the enemy can no longer see the player and there is a last known position, move towards it
                    Chase();

                    // Check if the enemy is stuck and change path if necessary
                    if (navMeshAgent.velocity.magnitude < 0.1f && lastKnownPlayerPosition == navMeshAgent.destination) {
                        SetRandomMoveTarget();
                    }
                    else if (navMeshAgent.remainingDistance < 0.1f) {
                        // If the enemy has reached the last known position and still cannot see the player, clear the last known position
                        lastKnownPlayerPosition = Vector3.zero;
                    }
                } else {
                    // If not already moving randomly and there is no last known position, start moving randomly
                    isMovingRandomly = true;
                    SetRandomMoveTarget();
                }
            }

            WalkAround();

            // Check if the enemy is stuck and change path if necessary
            if (navMeshAgent.velocity.magnitude < 0.1f && randomMoveTarget == navMeshAgent.destination) {
                SetRandomMoveTarget();
            }
        }

        if (distance <= attackRange && canSeePlayer) {
            // If the enemy is close enough to the player to attack and can see the player, stop moving and play the attack animation
            Attack();
        }
        else if (distance > attackRange && canMoveTowardsPlayer && !isMovingRandomly) {
            // If the enemy is too far from the player to attack and not already moving randomly, move towards the player and play the run animation
            Chase();
        }
        else if (canMoveTowardsPlayer && !isMovingRandomly) {
            // If the enemy is in the minimum attack range and not already moving randomly, turn towards the player and play the idle animation
            navMeshAgent.isStopped = true;
            transform.LookAt(player.transform);
            animator.SetBool("isIdling", true);
            animator.SetBool("isWalking", false);
        }
    }

    void Attack() {
        navMeshAgent.isStopped = true;
        Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(playerPosition);

        animator.SetBool("isIdling", true);
        animator.SetBool("isWalking", false);
    }

    void WalkAround() {
        navMeshAgent.SetDestination(randomMoveTarget);
        navMeshAgent.speed = 2f;
        animator.SetBool("isIdling", false);
        animator.SetBool("isWalking", true);
    }

    void Chase() {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.transform.position);
        navMeshAgent.speed = 2f;
        animator.SetBool("isIdling", false);
        animator.SetBool("isWalking", true);
    }

    void SetRandomMoveTarget() {
        randomMoveTarget = transform.position + Random.insideUnitSphere * randomMoveRadius;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomMoveTarget, out hit, randomMoveRadius, UnityEngine.AI.NavMesh.AllAreas);
        randomMoveTarget = hit.position;
    }
}