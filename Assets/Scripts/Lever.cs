using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    public float endRotation = 35f;

    public static bool gameEnded = false;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (!gameEnded)
        {
            float currentRotation = Quaternion.Angle(initialRotation, transform.rotation);

            if (currentRotation >= endRotation)
            {
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        gameEnded = true;
        Debug.Log("Game Over");
        SceneManager.LoadScene(2);
    }
}