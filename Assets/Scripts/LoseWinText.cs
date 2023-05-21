using UnityEngine;
using UnityEngine.UI;

public class LoseWinText : MonoBehaviour
{
    public Text resultText;
    public BlueSuitPathing blueSuitPathing;
    public AI_ABC_CBA_Path AI_ABC_CBA_Path;
    public Lever lever;

    void Start()
    {
        resultText.enabled = false; // Hide the text initially
    }

    void Update()
    {
        if (BlueSuitPathing.gameLost || AI_ABC_CBA_Path.gameLost)
        {
            resultText.enabled = true;
            resultText.text = "EXAM FAILED";
            resultText.color = Color.red;
        }
        else if (Lever.gameEnded)
        {
            resultText.enabled = true;
            resultText.text = "EXAM CANCELLED!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.enabled = false;
        }
    }
}
