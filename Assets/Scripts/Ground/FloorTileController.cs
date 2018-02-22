using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileController : MonoBehaviour {

    GameObject player;
    BoxCollider playerCollider;
    BoxCollider playerAttackCollider;
    PlayerMovement playerMovement;

    //MeshRenderer meshRend;
    Renderer rend;

    SpriteRenderer display;
    Animator anim;

    public bool isDamaged = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponents<BoxCollider>()[0];
        playerAttackCollider = player.GetComponents<BoxCollider>()[1];
        playerMovement = player.GetComponent<PlayerMovement>();

        //meshRend = GetComponent<MeshRenderer>();
        //meshRend.enabled = false;
        display = GetComponentInChildren<SpriteRenderer>();
        //display.enabled = false;
        anim = GetComponentInChildren<Animator>();
        rend = GetComponent<Renderer>();
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
            isDamaged = true;
            //meshRend.enabled = true;
            //display.enabled = true;
            anim.SetTrigger("Damage");
        }
    }

    public void Heal()
    {
        if (isDamaged)
        {
            isDamaged = false;
            //meshRend.enabled = false;
            //display.enabled = false;
            anim.SetTrigger("Heal");
        }
    }
}
