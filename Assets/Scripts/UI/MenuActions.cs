using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour {

    RandomSound sound;
    AudioSource music;

    private void Awake()
    {
        sound = GetComponent<RandomSound>();
        music = GameObject.Find("Audio").GetComponents<AudioSource>()[1];
    }

    public void Play()
    {
        sound.Play();
        music.volume = 0.7f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
