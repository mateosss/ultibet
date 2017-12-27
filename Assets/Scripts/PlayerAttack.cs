using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public float impactTime = 0.2f; // The time after the attack during which it destroys enemies

    PlayerAnimation playerAnimation;
    PlayerMovement playerMovement;
    BoxCollider attackCollider;
    float timer = 0f;

    void Awake () {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
        attackCollider = GetComponents<BoxCollider>()[1];
    }

    void Update()
    {
        if ((playerMovement.isRunning && Input.GetButtonDown("Fire1")) || Input.GetButtonDown("Fire3"))
        {
            playerAnimation.Attack();
            attackCollider.enabled = true;
        }

        if (timer >= impactTime)
        {
            timer = 0f;
            attackCollider.enabled = false;
        }

        if (attackCollider.enabled)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") Debug.Log("TRIGGEEEERED");
        if (timer > 0)
        {
            Debug.Log(">0");
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null) enemy.Death();
        }
    }
}
