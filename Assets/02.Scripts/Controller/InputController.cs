using System;
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
    public RectTransform canvas;
    private float swapMinArea;
    private float swapMaxArea;

    public TileController currentTileController;

    public Action<EMoveDirType> swapEvent;

    public Action<Tile> clickTileEvent;

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    private void Start()
    {
        Camera cam = Camera.main;
        InitSwapArea();
        //orthographic ��忡���� orthographicSize�� ���� ũ��� ���.
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }
    
    private void Update()
    {
        if(GameManager.Instance.IsCanSwap == false)
        {
            return;
        }

        SwipeEditor();
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && IsCheckInSwapArea())
        {
            endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //endPosition = Input.mousePosition;
            if (endPosition.y < swapMinArea && startPosition.y < swapMinArea)
            {
                Swipe(startPosition, endPosition);
            }
        }

        ClickTile();
    }

    public void ClickTile()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            // Raycast ����� ������ ����Ʈ
            List<RaycastResult> results = new List<RaycastResult>();

            // Raycast ����
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.layer == TileLayer)
                {
                    Debug.Log("Hit UI: " + result.gameObject.name);
                    clickTileEvent.Invoke(result.gameObject.GetComponent<Tile>());
                }
            }
        }
    }

    public bool IsCheckInSwapArea()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Input.mousePosition;

        // Raycast ����� ������ ����Ʈ
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast ����
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == SwapAreaLayer)
            {
                Debug.Log("Hit UI: " + result.gameObject.name);
                return true;
            }
        }

        return false;
    }

    private void SwipeEditor() 
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            swapEvent?.Invoke(EMoveDirType.Up);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            swapEvent?.Invoke(EMoveDirType.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            swapEvent?.Invoke(EMoveDirType.Left);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            swapEvent?.Invoke(EMoveDirType.Down);
        }
    }


    private void Swipe(Vector2 startPosition, Vector2 endPosition)
    {
        if (startPosition != endPosition/* && startPosition != Vector2.zero && endPosition != Vector2.zero*/)
        {
            float deltaX = endPosition.x - startPosition.x;
            float deltaY = endPosition.y - startPosition.y;

            //X�� �̵��� Y�� �̵����� Ŭ ��
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                //���콺 Ȥ�� ���� ����� �Ÿ��� �������ִ�.
                if (deltaX >= width / 6 || deltaX <= -width / 6)
                {
                    //���������� �� �۴� -> ���ʿ��� ���������� ��������.
                    if (startPosition.x < endPosition.x)
                    {
                        swapEvent?.Invoke(EMoveDirType.Right);
                    }
                    else
                    {
                        swapEvent?.Invoke(EMoveDirType.Left);
                    }
                }
            }
            else
            {
                if (deltaY >= height / 6 || deltaY <= -height / 6)
                {
                    if (startPosition.y < endPosition.y)
                    {
                        swapEvent?.Invoke(EMoveDirType.Up);
                    }
                    else
                    {
                        swapEvent?.Invoke(EMoveDirType.Down);
                    }
                }
            }
        }
    }

    public void InitSwapArea()
    {
        swapMinArea = (canvas.sizeDelta.y * 0.5f);
        swapMaxArea = 0;
    }
}
