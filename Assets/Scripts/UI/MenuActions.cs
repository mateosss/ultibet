using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuActions : MonoBehaviour {

    RandomSound sound;
    AudioSource music;

    private void Awake()
    {
        sound = GetComponent<RandomSound>();
        music = GameObject.Find("Audio").GetComponents<AudioSource>().Last();
    }

    public void Play()
    {
        sound.Play();
        music.volume = 0.7f;
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.CleanManager();
    }
}
