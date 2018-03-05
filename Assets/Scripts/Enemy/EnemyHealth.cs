using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    SpawnManager spawnManager;
    RandomSound sound;
    Animator anim;
    EnemyMovement enemyMovement;

    private void Awake()
    {
        spawnManager = GameObject.Find("Spawns").GetComponent<SpawnManager>();
        sound = GetComponent<RandomSound>();
        anim = GetComponentInChildren<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void Death()
    {
        anim.SetTrigger("Dead");
        sound.Play();
        spawnManager.EnemyDied();
        enemyMovement.Stop();
        Invoke("Destroy", 3);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
