using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour {

    public GameObject player;

    Transform playerTransform;
    PlayerAttack playerAttack;
    Vector3 initialScale;
    Vector3 offset;

    Renderer rend;
    bool ready = false;
    Color notReadyColor;
    Color readyColor;

    private void Awake()
    {
        playerTransform = player.GetComponent<Transform>();
        playerAttack = player.GetComponent<PlayerAttack>();
        initialScale = gameObject.transform.localScale;

        rend = GetComponent<Renderer>();
        notReadyColor = rend.material.color;
        ColorUtility.TryParseHtmlString("#3F51B5", out readyColor);
    }

    private void Start()
    {
        offset = gameObject.transform.position - playerTransform.position;
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
            //gameObject.transform.position = playerTransform.position + offset;
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
