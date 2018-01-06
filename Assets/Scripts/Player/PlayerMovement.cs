using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float speed = 4f;
    public float jump = 25f;
    public float jumpAttackAirPause = 0.2f;

    [Header("Advanced")]
    public float nodeContactThreshold = 0.5f;
    public GameObject pathNodeDisplay;
    public int maxJumps = 2;

    // Initialization ad hoc
    const float camRayLength = 100f;
    public int CurrentJump { get; private set; }
    bool onGround = false;
    bool running = false;
    int pathStep = 0;
    List<GameObject> path = new List<GameObject>();
    bool pauseJump = false;

    // Initialized on awake
    Transform player;
    Rigidbody rb;
    PlayerAnimation playerAnimation;
    Camera cam;
    int nodeLayer;
    int platformLayer;

    // Initialized on start
    float defaultY;
    Vector3 targetLocation;

    void Awake()
    {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        cam = Camera.main;
        nodeLayer = LayerMask.GetMask("Node");
        platformLayer = LayerMask.GetMask("Platform");

        CurrentJump = 0;
    }

    private void Start()
    {
        defaultY = player.position.y;
        ResetPath();
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
        else if (pathStep <= path.Count && running)
        {
            StartPath();
        }
        else
        {
            running = false;
        }
    }

    void Move(float dt)
    {
        player.Translate(Vector3.forward * speed * dt);
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
        if (pathStep < path.Count)
        {
            targetLocation = GetWithDefaultY(path[pathStep].transform.position);
            targetLocation.y = player.position.y;
            player.LookAt(targetLocation);
            pathStep++;
            running = true;
        }
        else
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
        pathStep = 0;
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
