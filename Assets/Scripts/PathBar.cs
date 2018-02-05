using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathBar : MonoBehaviour {

    public PlayerMovement playerMovement;
    public Image fill;

    Slider slider;
    RectTransform sliderTransform;

    bool ready = false;
    Color notReadyColor;
    Color readyColor;
    float bonusMultiplier = 1f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        sliderTransform = GetComponent<RectTransform>();
        notReadyColor = fill.color;
        ColorUtility.TryParseHtmlString("#E91E63", out readyColor);
    }

    private void LateUpdate()
    {
        if (bonusMultiplier != playerMovement.maxPathDistanceMultiplier)
        {
            sliderTransform.sizeDelta = new Vector2((sliderTransform.rect.width / bonusMultiplier) * playerMovement.maxPathDistanceMultiplier, sliderTransform.sizeDelta.y);
            bonusMultiplier = playerMovement.maxPathDistanceMultiplier;
        }

        slider.value = Mathf.Clamp(1 - playerMovement.pathDistanceDrawn / (playerMovement.maxPathDistance * playerMovement.maxPathDistanceMultiplier), 0f, 1f);
        
        if (!ready && playerMovement.pathDistanceDrawn == 0)
        {
            Ready();
        }
        else if (ready && playerMovement.pathDistanceDrawn > 0)
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
