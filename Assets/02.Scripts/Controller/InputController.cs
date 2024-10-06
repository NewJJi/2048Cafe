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
        SwipeEditor();
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //endPosition = Input.mousePosition;
            if (endPosition.y < swapMinArea && startPosition.y < swapMinArea)
            {
                Swipe(startPosition, endPosition, isCanDiagonal);
            }
        }
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
                if (deltaY >= height / 3 || deltaY <= -height / 3)
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

    public void SwapRecipeLab()
    {

    }

    public void InitSwapArea()
    {
        swapMinArea = (canvas.sizeDelta.y * 0.5f);
        swapMaxArea = 0;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, swapMinArea), new Vector3(1080, swapMinArea));
        Gizmos.DrawLine(new Vector3(0, swapMaxArea), new Vector3(1080, swapMaxArea));
    }
}
