using UnityEngine;
using UnityEngine.UI;

public class CanvasRectResponsiveness : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    private void Awake()
    {
        // 화면의 실제 비율 계산
        float screenAspectRatio = (float)Screen.width / Screen.height;

        // 기준 비율 9:19 계산
        float referenceAspectRatio = 9f / 19f;

        // 비교 후 Match 설정
        if (screenAspectRatio < referenceAspectRatio) // 화면 비율이 9:19보다 가로가 더 길다면
        {
            canvasScaler.matchWidthOrHeight = 0f; // Match를 0으로 설정 (가로 우선)
        }
        else // 화면 비율이 9:19보다 세로가 더 길거나 같다면
        {
            canvasScaler.matchWidthOrHeight = 1f; // Match를 1로 설정 (세로 우선)
        }
    }
}
