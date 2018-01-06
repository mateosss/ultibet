using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public float impactTime = 0.2f; // The time after the attack during which it destroys enemies
    public float cooldown = 1f; // Cooldown between attacks

    PlayerAnimation playerAnimation;
    PlayerMovement playerMovement;
    BoxCollider attackCollider;
    float impactTimer = 0f;
    public float CooldownTimer { get; private set; }
    int groundLayer;

    void Awake () {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
        attackCollider = GetComponents<BoxCollider>()[1];
        groundLayer = LayerMask.GetMask("Ground");

        CooldownTimer = cooldown;
    }

    void Update()
    {
        if (CooldownTimer > cooldown && ((playerMovement.IsRunning() && (Input.GetButtonDown("Fire1")) && Input.mousePosition.x > Screen.width / 2) || Input.GetButtonDown("Fire3"))) // Attack
        {
            Attack();
        }

        CooldownTimer += Time.deltaTime;
        
        if (impactTimer >= impactTime) // attackCollider time of presence
        {
            impactTimer = 0f;
            attackCollider.enabled = false;
        }

        if (attackCollider.enabled)
        {
            impactTimer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (impactTimer > 0)
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null) enemy.Death();
        }
    }

    private void Attack()
    {
        CooldownTimer = 0f;
        playerAnimation.Attack();

        if (playerMovement.CurrentJump > 0 && playerMovement.CurrentJump < playerMovement.maxJumps)
        {
            playerMovement.PauseJump();
        }

        attackCollider.enabled = true;

        Ray topdown = new Ray(gameObject.transform.position, Vector3.down);
        RaycastHit tileHit;
        Physics.Raycast(topdown, out tileHit, 100f, groundLayer);
        Debug.Log(tileHit.collider);
        FloorTileController floorTile = tileHit.collider.GetComponent<FloorTileController>();
        if (floorTile != null)
        {
            floorTile.Heal();
        }
    }

}
