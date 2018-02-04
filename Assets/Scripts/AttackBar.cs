using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBar : MonoBehaviour {

    public PlayerAttack playerAttack;
    public Image fill;

    Slider slider;

    bool ready = false;
    Color notReadyColor;
    Color readyColor;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        notReadyColor = fill.color;
        ColorUtility.TryParseHtmlString("#80D8FF", out readyColor);
    }

    private void LateUpdate()
    {
        if (playerAttack.cooldown <= 0)
        {
            slider.value = 0;
        }
        else
        {
            slider.value = Mathf.Clamp(playerAttack.CooldownTimer / playerAttack.cooldown, 0f, 1f);
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
        fill.color = readyColor;
    }

    void NotReady()
    {
        ready = false;
        fill.color = notReadyColor;
    }
}
