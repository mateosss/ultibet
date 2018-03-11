using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

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
        sound.Play();
        Application.Quit();
    }
}
