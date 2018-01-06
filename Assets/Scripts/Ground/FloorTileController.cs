using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileController : MonoBehaviour {

    GameObject player;
    Renderer rend;

    bool isDamaged = false;
    Color normalColor;
    Color damagedColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        normalColor = rend.material.color;
        ColorUtility.TryParseHtmlString("#03A9F4", out damagedColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isDamaged = true;
            rend.material.color = damagedColor;
        }
        else if (other.gameObject.tag == "Player")
        {
            isDamaged = false;
            rend.material.color = normalColor;
        }
    }
}
