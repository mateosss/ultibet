using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h);
        style.alignment = TextAnchor.LowerLeft;
        style.fontSize = 24;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{1:0.} fps", msec, fps);
        GUI.Label(rect, text, style);
    }
}