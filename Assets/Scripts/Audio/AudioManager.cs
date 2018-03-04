using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    AudioClip[] sounds;

    public static AudioManager instance;

    AudioSource source;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, bool loop = false, float volume = 1f)
    {
        //source.clip = clip;
        //source.loop = loop;
        //source.volume = volume;
        //source.Play();
        source.PlayOneShot(clip);
    }
}
