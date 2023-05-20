using UnityEngine;
using UnityEngine.UI;

public class LoseWinText : MonoBehaviour
{
    public Text resultText;
    public BlueSuitPathing blueSuitPathing;
    public Lever lever;

    void Start()
    {
        resultText.enabled = false; // Hide the text initially
    }

    void Update()
    {
        if (BlueSuitPathing.gameLost)
        {
            resultText.enabled = true;
            resultText.text = "YOU LOSE.";
            resultText.color = Color.red;
        }
        else if (Lever.gameEnded)
        {
            resultText.enabled = true;
            resultText.text = "YOU WIN!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.enabled = false;
        }
    }
}
