using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {

    static Text highscoreText;
    static string defaultText;
    static int highscore;
    
	void Awake () {
        highscoreText = GetComponent<Text>();
        defaultText = highscoreText.text;
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = defaultText + highscore.ToString();
    }

    public static void SetHighscore(int score)
    {
        if (score > highscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            SetScoreText(score);
        }
    }

    static void SetScoreText(int score)
    {
        highscoreText.text = defaultText + score.ToString();
    }

}
