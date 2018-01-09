using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour {

    public GameObject player;

    PlayerAttack playerAttack;
    Vector3 initialScale;

    Renderer rend;
    bool ready = false;
    Color notReadyColor;
    Color readyColor;

    private void Awake()
    {
        playerAttack = player.GetComponent<PlayerAttack>();
        initialScale = gameObject.transform.localScale;

        rend = GetComponent<Renderer>();
        notReadyColor = rend.material.color;
        ColorUtility.TryParseHtmlString("#3F51B5", out readyColor);
    }

    private void LateUpdate()
    {
        if (playerAttack.cooldown <= 0)
        {
            gameObject.transform.localScale = initialScale;
        }
        else
        {
            gameObject.transform.localScale = initialScale * Mathf.Clamp(playerAttack.CooldownTimer / playerAttack.cooldown, 0.2f, 1f);
        }

        if (!ready && playerAttack.CooldownTimer >= playerAttack.cooldown)
        {
            Ready();
        }
        else if (ready && playerAttack.CooldownTimer < playerAttack.cooldown)
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
