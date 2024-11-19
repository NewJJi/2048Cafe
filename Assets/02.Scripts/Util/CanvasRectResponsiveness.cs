using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRectResponsiveness : MonoBehaviour
{
    int screenWidth = Screen.width;
    int screenHeight = Screen.height;

    [ContextMenu("Show Screen")]
    public void SetCanvasRect()
    {
        Debug.Log(screenWidth + " : " + screenHeight);
    }
}
