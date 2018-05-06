using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {

    static Text highscoreText;
    static Text scoreText;
    static string defaultText;
    static int highscore;
    
	void Awake () {
        highscoreText = GetComponent<Text>();
        GameObject scoreObject = GameObject.Find("Score");
        if (scoreObject != null) scoreText = scoreObject.GetComponent<Text>();
        defaultText = highscoreText.text;
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = defaultText + highscore.ToString();
    }

    public static void SetHighscore(int score)
    {
        if (scoreText != null) scoreText.text = score.ToString();
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
