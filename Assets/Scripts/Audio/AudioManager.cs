using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    AudioClip[] sounds;

    public static AudioManager instance;
    public static bool playMusic = true;
    public bool persist = true;
    AudioSource source;
    AudioSource musicSource;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        if (persist) DontDestroyOnLoad(gameObject);
        source = GetComponents<AudioSource>()[0];
        if (playMusic)
        {
            musicSource = GetComponents<AudioSource>().Last();
            musicSource.Play();
        }
    }

    public void Play(AudioClip clip, bool loop = false, float volume = 1f)
    {
        //source.clip = clip;
        //source.loop = loop;
        //source.volume = volume;
        //source.Play();
        source.PlayOneShot(clip);
    }

    public void CleanManager()
    {
        instance = null;
        if (playMusic && musicSource != null)
        {
            musicSource.Stop();
        }
        Destroy(gameObject);
    }
}
