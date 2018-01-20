using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public float impactTime = 0.2f; // The time after the attack during which it destroys enemies
    public float cooldown = 1f; // Cooldown between attacks
    public float range = 4f;
    public Animator attackRangeDisplayAnimator;
    public Animator healLineDisplayAnimator;

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

        SetRange(range);
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
            if (enemy != null)
            {
                enemy.Death();
                playerMovement.AddEnemyKilled();
            }
        }
    }

    public void Attack()
    {
        CooldownTimer = 0f;
        playerAnimation.Attack();
        attackRangeDisplayAnimator.SetTrigger("Attack");
        healLineDisplayAnimator.SetTrigger("Attack");

        if (playerMovement.CurrentJump > 0 && playerMovement.CurrentJump < playerMovement.maxJumps)
        {
            playerMovement.PauseJump();
        }

        attackCollider.enabled = true;

        Ray topdown = new Ray(gameObject.transform.position, Vector3.down);
        RaycastHit tileHit;
        Physics.Raycast(topdown, out tileHit, 100f, groundLayer);

        FloorTileController floorTile = tileHit.collider.GetComponent<FloorTileController>();
        if (floorTile != null)
        {
            floorTile.Heal();
        }
    }

    public void SetContinuousAttack(bool value)
    {
        attackCollider.enabled = value;
    }

    public void SetRange(float newRange)
    {
        range = newRange;
        attackCollider.size = new Vector3(range, 10f, range);
        attackRangeDisplayAnimator.transform.parent.transform.localScale = new Vector3(1, 0, 1) * (range / 4f) + Vector3.up;
    }

}
