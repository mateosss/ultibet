using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject[] hideOnCredits;
    public GameObject[] showOnCredits;

    private bool showingCredits = false;
    RandomSound sound;

    private void Awake()
    {
        sound = GetComponent<RandomSound>();
    }

    public void Play()
    {
        AudioManager.instance.CleanManager();
        SceneManager.LoadScene("Game");
    }

    public void Tutorial()
    {
        sound.Play();
        SceneManager.LoadScene("Tutorial");
    }

    public void Exit()
    {
        if (showingCredits) Credits();
        else
        {
            sound.Play();
            Application.Quit();
        }
    }

    public void Credits()
    {
        sound.Play();
        showingCredits = !showingCredits;

        for (int i = 0; i < showOnCredits.Length; i++) showOnCredits[i].SetActive(showingCredits);
        for (int i = 0; i < hideOnCredits.Length; i++) hideOnCredits[i].SetActive(!showingCredits);
    }

    public void LinkMayo()
    {
        Application.OpenURL("https://mateosss.github.io/");
    }

    public void LinkRenzo()
    {
        Application.OpenURL("https://www.artstation.com/renzosartore");
    }

    public void LinkJose()
    {
        Application.OpenURL("https://www.artstation.com/joseluna");
    }
}
