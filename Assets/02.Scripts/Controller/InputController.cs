using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;
public class InputController : MonoBehaviour
{
    private Vector2 startPosition = Vector2.zero;
    private Vector2 endPosition = Vector2.zero;

    private float height;
    private float width;

    //�밢�� ���� ����
    public bool isCanDiagonal = true;

    //where is the position init?
    private float swapMinArea;
    private float swapMaxArea;

    public TileController currentTileController;

    private void Start()
    {
        Camera cam = Camera.main;
        
        //orthographic ��忡���� orthographicSize�� ���� ũ��� ���.
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetTopUIElementUnderMouse();
            //startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPosition = Input.mousePosition;

            Swipe(startPosition, endPosition, isCanDiagonal);
        }
    }

    private void Swipe(Vector2 startPosition, Vector2 endPosition, bool diagonal)
    {
        if (startPosition != endPosition/* && startPosition != Vector2.zero && endPosition != Vector2.zero*/)
        {
            float deltaX = endPosition.x - startPosition.x;
            float deltaY = endPosition.y - startPosition.y;

            //X�� �̵��� Y�� �̵����� Ŭ ��
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                //���콺 Ȥ�� ���� ����� �Ÿ��� �������ִ�.
                if (deltaX >= width / 4 || deltaX <= -width / 4)
                {
                    //���������� �� �۴� -> ���ʿ��� ���������� ��������.
                    if (startPosition.x < endPosition.x)
                    {
                        currentTileController.Move(EMoveDirType.Right);
                    }
                    else
                    {
                        currentTileController.Move(EMoveDirType.Left);
                    }
                }
            }
            else
            {
                if (deltaY >= height / 3 || deltaY <= -height / 3)
                {
                    if (startPosition.y < endPosition.y)
                    {
                        currentTileController.Move(EMoveDirType.Up);
                    }
                    else
                    {
                        currentTileController.Move(EMoveDirType.Down);
                    }
                }
            }
        }
    }

    public void SwapRecipeLab()
    {

    }

    public void InitSwapArea()
    {
        swapMinArea = 0;
        swapMaxArea = 0;
    }

    public GraphicRaycaster raycaster; // Canvas�� ������ GraphicRaycaster
    public EventSystem eventSystem;    // EventSystem ������Ʈ
    void GetTopUIElementUnderMouse()
    {
        // 1. PointerEventData ����: ���� ���콺 �������� ����
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition // ���콺 �������� �Է�
        };

        // 2. Raycast ����� ������ ����Ʈ ����
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // 3. GraphicRaycaster�� ����� UI�� Raycast ����
        raycaster.Raycast(pointerData, raycastResults);

        // 4. Raycast ��� ó�� (���� ���� UI ��� ��������)
        if (raycastResults.Count > 0)
        {
            // ���� ���� �ִ� ù ��° UI ���
            RaycastResult topResult = raycastResults[0];
            Debug.Log("Top UI Element under mouse: " + topResult.gameObject.name);
        }
        else
        {
            Debug.Log("No UI element under mouse.");
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, swapMinArea), new Vector3(1080, swapMinArea));
        Gizmos.DrawLine(new Vector3(0, swapMaxArea), new Vector3(1080, swapMaxArea));
    }
}
