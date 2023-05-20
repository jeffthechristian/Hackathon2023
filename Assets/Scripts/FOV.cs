using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    //FOV values
    public float radius;
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    float timer = 0f;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine(){
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while(true){
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck(){
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if(rangeChecks.Length!=0){
            Debug.Log("1");
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                Debug.Log("2");
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, directionToTarget, out RaycastHit test, distanceToTarget,  obstructionMask)){
                    Debug.Log("3");
                    canSeePlayer = true;

                    // Debug.Break();
                }
                else{
                    Debug.Log("4");
                    canSeePlayer = false;
                    }
                    
            }
            
            else {
                Debug.Log("5");
                canSeePlayer = false;}
        }
    }

    void Update() {
        if (canSeePlayer) {
            timer += Time.deltaTime;
        } else {
            timer = 0f;
        }
    }
}