using UnityEngine;
using System.Collections;

public class FrameChecker : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int width = Screen.width;
        int height = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, height/4, width, height * 2 / 100);  // 화면의 상단에 표시
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 100;  // 화면 크기에 비례한 폰트 크기
        style.normal.textColor = Color.red;

        float fps = 1.0f / deltaTime;
        string text = $"FPS: {Mathf.Ceil(fps)}";
        GUI.Label(rect, text, style);
    }
}