using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour {

    public string groupName;
    public AudioClip[] clips;

    AudioClip lastClip;

	public void Play () {
        lastClip = clips[Random.Range(0, clips.Length)];
        AudioManager.instance.Play(lastClip);
	}
}
