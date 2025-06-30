using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vehicles_24_ScoreDisplay : MonoBehaviour
{
    public List<Text> scoreTexts; // Assign these in the Inspector (the 10 Text fields)

    void Start()
    {
        DisplayScores();
    }

    void DisplayScores()
    {
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            int score = PlayerPrefs.GetInt("Vehicles_24_Score_" + i, 0);
            scoreTexts[i].text = score + "  points";  // Only show the score, no numbering
        }
    }
}
