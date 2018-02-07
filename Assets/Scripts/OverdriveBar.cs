using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverdriveBar : MonoBehaviour {

    public PlayerMovement playerMovement;
    public Image fill;

    Slider slider;

    bool ready = false;
    Color notReadyColor;
    Color readyColor;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        notReadyColor = fill.color;
        ColorUtility.TryParseHtmlString("#FF5722", out readyColor);
    }

    private void LateUpdate()
    {

        if (playerMovement.overdriving)
        {
            slider.value = Mathf.Clamp(1 - playerMovement.overdriveDistanceDrawn / playerMovement.maxOverdriveDistance, 0f, 1f);
        }
        else
        {
             slider.value = Mathf.Clamp(playerMovement.OverdriveCharge, 0f, 1f);
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
        fill.color = readyColor;
    }

    void NotReady()
    {
        ready = false;
        fill.color = notReadyColor;
    }
}
