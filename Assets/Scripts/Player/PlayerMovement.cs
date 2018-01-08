using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float speed = 4f;
    public float jump = 25f;
    public float jumpAttackAirPause = 0.2f;

    [Header("Long Path Bonus (BPS = Bonus per second)")]
    public float isLongPathFrom = 15f;
    public float cooldownBPS = 0.1f;
    public float rangeBPS = 0.5f;
    public float overdriveBPS = 0.033f;

    [Header("Advanced")]
    public float nodeContactThreshold = 0.5f;
    public GameObject pathNodeDisplay;
    public int maxJumps = 2;

    // Initialization ad hoc
    const float camRayLength = 100f;
    bool onGround = false;
    bool running = false;
    List<GameObject> path = new List<GameObject>();
    bool pauseJump = false;

    // Initialized on awake
    Transform player;
    Rigidbody rb;
    PlayerAnimation playerAnimation;
    PlayerAttack playerAttack;
    PlayerOverdrive playerOverdrive;
    Camera cam;
    int nodeLayer;
    int platformLayer;
    public int CurrentJump { get; private set; }
    public float DistanceTravelled { get; private set; }
    public int NodesTravelled { get; private set; }
    public int EnemiesKilled { get; private set; }
    public float PathDistance { get; private set; }
    public int PathStep { get; private set; }
    public int PathKilled { get; private set; }

    // Initialized on start
    float defaultY;
    Vector3 targetLocation;

    void Awake()
    {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAttack = GetComponent<PlayerAttack>();
        playerOverdrive = GetComponent<PlayerOverdrive>();
        cam = Camera.main;
        nodeLayer = LayerMask.GetMask("Node");
        platformLayer = LayerMask.GetMask("Platform");

        CurrentJump = 0;
        DistanceTravelled = 0f;
        NodesTravelled = 0;
        EnemiesKilled = 0;
        PathDistance = 0f;
        PathStep = 0;
        PathKilled = 0;
    }

    private void Start()
    {
        defaultY = player.position.y;
        ResetPath();
        StartCoroutine(BPS());
    }

    void Update()
    {

        if (!pauseJump && Input.GetButton("Fire1") && !running) // Add node to path
        {
            GameObject node = NodeTouchedByMouse();
            if (node)
            {
                AddToPath(node.transform.position);
            }
        }

        if (!pauseJump  && Input.GetButtonUp("Fire1") && !running) // Start path
        {
            StartPath();
        }

        if (!pauseJump && running && ((Input.GetButtonDown("Fire1") && Input.mousePosition.x < Screen.width / 2) || Input.GetButtonDown("Jump")) && CurrentJump < maxJumps) // Jump
        {
            Jump();
        }
        else if (CurrentJump > 0)
        {
            CheckJumpFinish();
        }

        if (DistanceFromTop(targetLocation, player.position) > nodeContactThreshold) // Move, change node or stop running
        {
            if (!pauseJump)
            {
                Move(Time.deltaTime);
            }
        }
        else if (PathStep <= path.Count && running)
        {
            StartPath();
        }
        else
        {
            running = false;
        }
    }

    IEnumerator BPS() // Long path bonuses
    {
        bool withBonus = false;
        float initialCooldown = playerAttack.cooldown;
        float initialRange = playerAttack.range;
        while (true)
        {
            if (running && PathDistance > isLongPathFrom)
            {
                if (!withBonus)
                {
                    withBonus = true;
                }
                if (playerAttack.cooldown > 0) // Cooldown
                {
                    playerAttack.cooldown -= cooldownBPS * Time.deltaTime;
                }
                playerAttack.SetRange(playerAttack.range + rangeBPS * Time.deltaTime); // Range
                if (playerOverdrive.Charge < 1f) // Overdrive
                {
                    playerOverdrive.Charge += overdriveBPS * Time.deltaTime;
                }
                else
                {
                    playerOverdrive.Charge = 1f;
                }
            }
            else if (withBonus)
            {
                withBonus = false;
                playerAttack.cooldown = initialCooldown;
                playerAttack.SetRange(initialRange);
            }
            yield return null;
        }
    }

    void Move(float dt)
    {
        float move = speed * dt;
        player.Translate(Vector3.forward * speed * dt);
        PathDistance += move;
        DistanceTravelled += move;
    }

    void Jump()
    {
        rb.velocity = Vector3.up * jump;
        playerAnimation.Jump();
        CurrentJump++;
    }

    void CheckJumpFinish()
    {
        if (IsOnPlatform())
        {
            CurrentJump = 0;
        }
    }

    // Path section

    void StartPath()
    {
        if (PathStep == 0)
        {
            PathStep++;
            running = true;
        }
        else if (PathStep < path.Count) // Change to new node in the same path
        {
            targetLocation = GetWithDefaultY(path[PathStep].transform.position);
            targetLocation.y = player.position.y;
            player.LookAt(targetLocation);
            PathStep++;
            NodesTravelled++;
        }
        else // Stop path
        {
            ResetPath();
        }
    }

    void ResetPath()
    {
        path.ForEach(Destroy);
        path.Clear();
        targetLocation = GetWithDefaultY(player.position);
        AddToPath(targetLocation);
        PathDistance = 0f;
        PathStep = 0;
        PathKilled = 0;
        running = false;
    }

    void AddToPath(Vector3 point)
    {
        point.y += 0.1f;
        if (path.Count > 0 && point == path[path.Count - 1].transform.position) return;

        GameObject pathNode = Instantiate(pathNodeDisplay, point, pathNodeDisplay.GetComponent<Transform>().rotation);
        if (path.Count > 0)
        {
            LineRenderer lr = path[path.Count - 1].GetComponent<LineRenderer>();
            lr.SetPosition(0, path[path.Count - 1].transform.position);
            lr.SetPosition(1, point);
        }
        path.Add(pathNode);
    }

    public void OutOfFloor()
    {
        onGround = false;
    }

    public void Fell()
    {
        onGround = true;
        ResetPath();
        CurrentJump = 0;
    }

    public bool IsOnGround()
    {
        return onGround;
    }

    public bool IsRunning()
    {
        return running;
    }

    public bool IsJumping()
    {
        return CurrentJump > 0;
    }

    public void PauseJump()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        pauseJump = true;
        Invoke("RestoreJump", jumpAttackAirPause); // TODO: Learn how to use and implemente coroutines
    }
    
    public void AddEnemyKilled()
    {
        if (running)
        {
            PathKilled++;
        }
        EnemiesKilled++;
    }

    public void RestoreJump()
    {
        rb.useGravity = true;
        pauseJump = false;
    }

    Vector3 GetWithDefaultY(Vector3 t)
    {
        return new Vector3(t.x, defaultY, t.z);
    }

    Vector3 MouseToPoint()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Physics.Raycast(camRay, out floorHit, camRayLength, nodeLayer);
        return floorHit.point;
    }

    GameObject NodeTouchedByMouse()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Physics.Raycast(camRay, out floorHit, camRayLength, nodeLayer);
        if (!floorHit.collider) return null;
        return floorHit.collider.gameObject;
    }

    bool IsOnPlatform()
    {
        if (rb.velocity.y > 0)
        {
            return false;
        }
        Ray topdown = new Ray(player.position, Vector3.down);
        RaycastHit platformHit;
        Physics.Raycast(topdown, out platformHit, player.localScale.z, platformLayer);
        return platformHit.collider != null;
    }

    float DistanceFromTop(Vector3 p0, Vector3 p1)
    {
        return Vector2.Distance(new Vector2(p0.x, p0.z), new Vector2(p1.x, p1.z));
    }
}
