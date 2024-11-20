using UnityEngine;
using UnityEngine.UI;

public class CanvasRectResponsiveness : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    private void Awake()
    {
        // ȭ���� ���� ���� ���
        float screenAspectRatio = (float)Screen.width / Screen.height;

        // ���� ���� 9:19 ���
        float referenceAspectRatio = 9f / 19f;

        // �� �� Match ����
        if (screenAspectRatio < referenceAspectRatio) // ȭ�� ������ 9:19���� ���ΰ� �� ��ٸ�
        {
            canvasScaler.matchWidthOrHeight = 0f; // Match�� 0���� ���� (���� �켱)
        }
        else // ȭ�� ������ 9:19���� ���ΰ� �� ��ų� ���ٸ�
        {
            canvasScaler.matchWidthOrHeight = 1f; // Match�� 1�� ���� (���� �켱)
        }
    }
}
