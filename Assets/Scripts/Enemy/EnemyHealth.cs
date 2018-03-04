using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    SpawnManager spawnManager;
    RandomSound sound;

    private void Awake()
    {
        spawnManager = GameObject.Find("Spawns").GetComponent<SpawnManager>();
        sound = GetComponent<RandomSound>();
    }

    public void Death()
    {
        sound.Play();
        Destroy(gameObject);
        spawnManager.EnemyDied();
    }
}
