using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdriveIndicator : MonoBehaviour {

    public GameObject player;

    PlayerMovement playerMovement;
    Vector3 initialScale;

    Renderer rend;
    bool ready = false;
    Color notReadyColor;
    Color readyColor;

    private void Awake()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        initialScale = gameObject.transform.localScale;

        rend = GetComponent<Renderer>();
        notReadyColor = rend.material.color;
        ColorUtility.TryParseHtmlString("#FF5722", out readyColor);
    }

    private void LateUpdate()
    {
        if (playerMovement.overdriving)
        {
            gameObject.transform.localScale = initialScale * Mathf.Clamp(1 - playerMovement.overdriveDistance / playerMovement.maxOverdriveDistance, 0.2f, 1.0f);
        } else
        {
            gameObject.transform.localScale = initialScale * Mathf.Clamp(playerMovement.OverdriveCharge, 0.2f, 1.0f);
        }
        
        if (!ready && playerMovement.OverdriveCharge >= 1f)
        {
            Ready();
        }
        else if (ready && playerMovement.OverdriveCharge < 1f)
        {
            NotReady();
        }
    }

    void Ready()
    {
        ready = true;
        rend.material.color = readyColor;
    }

    void NotReady()
    {
        ready = false;
        rend.material.color = notReadyColor;
    }
}
