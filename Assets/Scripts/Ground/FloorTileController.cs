using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileController : MonoBehaviour {

    GameObject player;
    PlayerMovement playerMovement;
    BoxCollider playerCollider;
    Renderer rend;

    bool isDamaged = false;
    Color normalColor;
    Color damagedColor;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerCollider = player.GetComponents<BoxCollider>()[0];
        rend = GetComponent<Renderer>();
        normalColor = rend.material.color;
        ColorUtility.TryParseHtmlString("#03A9F4", out damagedColor);
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
    }

    public void Damage()
    {
        isDamaged = true;
        rend.material.color = damagedColor;
    }

    public void Heal()
    {
        isDamaged = false;
        rend.material.color = normalColor;
    }
}
