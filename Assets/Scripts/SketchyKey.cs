using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SketchyKey : MonoBehaviour {
    public GameObject door;
    public Animator animator;
    public bool isOpen;

    public float endRotation = 40f;

    public static bool gameEnded = false;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
        isOpen = false;
    }

    private void Update()
    {
        if (!gameEnded)
        {
            float currentRotation = Quaternion.Angle(initialRotation, transform.rotation);

            if (currentRotation >= endRotation)
            {
                isOpen = true;
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        if (isOpen) {
        animator.SetBool("isOpen", true);
        }
    }
}