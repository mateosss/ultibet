using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileController : MonoBehaviour {

    GameObject player;
    BoxCollider playerCollider;
    BoxCollider playerAttackCollider;
    PlayerMovement playerMovement;

    MeshRenderer meshRend;
    Renderer rend;

    public bool isDamaged = false;
    //Color normalColor;
    //Color damagedColor;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponents<BoxCollider>()[0];
        playerAttackCollider = player.GetComponents<BoxCollider>()[1];
        playerMovement = player.GetComponent<PlayerMovement>();

        meshRend = GetComponent<MeshRenderer>();
        meshRend.enabled = false;
        rend = GetComponent<Renderer>();
        //normalColor = rend.material.color;
        //ColorUtility.TryParseHtmlString("#5ed32280", out damagedColor);
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
        isDamaged = true;
        meshRend.enabled = true;
        //rend.material.color = damagedColor;
    }

    public void Heal()
    {
        isDamaged = false;
        meshRend.enabled = false;
        //rend.material.color = normalColor;
    }
}
