using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileController : MonoBehaviour {

    GameObject player;
    BoxCollider playerCollider;
    BoxCollider playerAttackCollider;
    PlayerMovement playerMovement;
    Animator anim;
    RandomSound damageSound;
    RandomSound healSound;

    public bool isDamaged = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponents<BoxCollider>()[0];
        playerAttackCollider = player.GetComponents<BoxCollider>()[1];
        playerMovement = player.GetComponent<PlayerMovement>();
        anim = GetComponentInChildren<Animator>();
        damageSound = GetComponents<RandomSound>()[0];
        healSound = GetComponents<RandomSound>()[1];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDamaged)
        {
            if (other.gameObject.tag == "Enemy" || other == playerCollider)
            {
                Damage();
            }
        }

        if (playerMovement.overdriving)
        {
            if (other == playerAttackCollider)
            {
                Heal();
            }
        }
    }

    public void Damage()
    {
        if (!isDamaged)
        {
            damageSound.Play();
            isDamaged = true;
            anim.SetTrigger("Damage");
        }
    }

    public void Heal()
    {
        if (isDamaged)
        {
            healSound.Play();
            isDamaged = false;
            anim.SetTrigger("Heal");
        }
    }
}
