using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AI_ABC_CBA_Path : MonoBehaviour
{
    public Transform patrolRouteObject;
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public float rotationSpeed;
    public float gravity = 20f;
    public float stopTime;
    public float timer = 0f; 
    private Rigidbody rb;
    Vector3 target;
    public FOV fov;
    float oldspeed;
    private bool playerSeen = false;
    private float playerSeenTimer = 0f;
    public static bool gameLost = false;
    public bool incrementDirection;
    void Start()
    {
        oldspeed = speed;
        incrementDirection = false;
        Transform[] waypoints = patrolRouteObject.GetComponentsInChildren<Transform>();
        List<Transform> tempPatrolPoints = new List<Transform>();
        foreach(Transform waypoint in waypoints){
            if(waypoint != patrolRouteObject){
                tempPatrolPoints.Add(waypoint);
            }
        }
        patrolPoints = tempPatrolPoints.ToArray();
        targetPoint = 0;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(fov.canSeePlayer){
            playerSeenTimer += Time.deltaTime;

            if (!playerSeen && playerSeenTimer >= 4f)
            {
                gameLost = true;
                SceneManager.LoadScene(2);
            }
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isSearching", true);
            speed = 0;
        } else {
            GetComponent<Animator>().SetBool("isWalking", true);
            GetComponent<Animator>().SetBool("isSearching", false);
            playerSeenTimer = 0;
            speed = oldspeed;
        }
         if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.3f)
    {
        if (!GetComponent<Animator>().GetBool("isSearching"))
        {
            timer += Time.deltaTime;
            if (timer >= stopTime)
            {
                increaseTargetInt();
                timer = 0f;
            }
        }
    }
    else
    {
        timer = 0f; // Reset the timer when not close to the target point
    }

        if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) > 0.3f)
        {
            GetComponent<Animator>().SetBool("isWalking", true);
            Vector3 direction = patrolPoints[targetPoint].position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            GetComponent<Animator>().SetBool("isWalking", false);
        }
        

        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);

        rb.AddForce(Vector3.down * gravity);
    }

    void increaseTargetInt()
{
    if (incrementDirection)
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = patrolPoints.Length - 1;
            incrementDirection = false;
        }
    }
    else
    {
        targetPoint--;
        if (targetPoint < 0)
        {
            targetPoint = 1;
            incrementDirection = true;
        }
    }
}
}