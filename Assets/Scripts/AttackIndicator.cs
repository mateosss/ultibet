using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour {

    public GameObject player;

    Transform playerTransform;
    PlayerAttack playerAttack;
    Vector3 initialScale;
    Vector3 offset;

    private void Awake()
    {
        playerTransform = player.GetComponent<Transform>();
        playerAttack = player.GetComponent<PlayerAttack>();
        initialScale = gameObject.transform.localScale;
    }

    private void Start()
    {
        offset = gameObject.transform.position - playerTransform.position;
    }

    private void LateUpdate()
    {
        gameObject.transform.localScale = initialScale * Mathf.Clamp(playerAttack.CooldownTimer, 0.2f, 1.0f);
        gameObject.transform.position = playerTransform.position + offset;
    }

}
