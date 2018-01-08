using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdriveIndicator : MonoBehaviour {

    public GameObject player;

    Transform playerTransform;
    PlayerOverdrive playerOverdrive;
    Vector3 initialScale;
    Vector3 offset;

    Renderer rend;
    bool ready = false;
    Color notReadyColor;
    Color readyColor;

    private void Awake()
    {
        playerTransform = player.GetComponent<Transform>();
        playerOverdrive = player.GetComponent<PlayerOverdrive>();
        initialScale = gameObject.transform.localScale;

        rend = GetComponent<Renderer>();
        notReadyColor = rend.material.color;
        ColorUtility.TryParseHtmlString("#FF5722", out readyColor);
    }

    private void Start()
    {
        offset = gameObject.transform.position - playerTransform.position;
    }

    private void LateUpdate()
    {
        gameObject.transform.localScale = initialScale * Mathf.Clamp(playerOverdrive.Charge, 0.2f, 1.0f);
        //gameObject.transform.position = playerTransform.position + offset;
        if (!ready && playerOverdrive.Charge >= 1f)
        {
            Ready();
        }
        else if (ready && playerOverdrive.Charge < 1f)
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
