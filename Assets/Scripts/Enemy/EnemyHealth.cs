using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    SpawnManager spawnManager;
    RandomSound sound;
    Animator anim;
    EnemyMovement enemyMovement;

    int maxSounds;
    static int playing = 0;
    static bool willReset;

    private void Awake()
    {
        spawnManager = GameObject.Find("Spawns").GetComponent<SpawnManager>();
        sound = GetComponent<RandomSound>();
        anim = GetComponentInChildren<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        maxSounds = sound.clips.Length;
    }

    public void Death()
    {
        anim.SetTrigger("Dead");
        spawnManager.EnemyDied();
        if (playing < maxSounds)
        {
            sound.Play();
            playing++;
            willReset = true;
        }
        enemyMovement.Stop();
        Invoke("Destroy", 3);
    }

    void Destroy()
    {
        if (willReset)
        {
            playing = 0;
            willReset = false;
        }
        Destroy(gameObject);
    }
}
