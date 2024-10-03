using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class InputController : MonoBehaviour
{
    private Vector2 startPosition = Vector2.zero;
    private Vector2 endPosition = Vector2.zero;

    private float height;
    private float width;

    //대각선 가능 여부
    public bool isCanDiagonal = true;

    public TileController tileController;

    void Start()
    {
        Camera cam = Camera.main;
        
        //orthographic 모드에서는 orthographicSize가 절반 크기로 축소.
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Swipe(startPosition, endPosition, isCanDiagonal);
        }
    }
   
    void Swipe(Vector2 startPosition, Vector2 endPosition, bool diagonal)
    {
        if (startPosition != endPosition/* && startPosition != Vector2.zero && endPosition != Vector2.zero*/)
        {
            float deltaX = endPosition.x - startPosition.x;
            float deltaY = endPosition.y - startPosition.y;

            //X축 이동이 Y축 이동보다 클 때
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                //마우스 혹은 손이 충분히 거리가 벌어져있다.
                if (deltaX >= width / 4 || deltaX <= -width / 4)
                {
                    //시작지점이 더 작다 -> 왼쪽에서 오른쪽으로 움직였다.
                    if (startPosition.x < endPosition.x)
                    {
                        tileController.Move(EMoveDirType.Right);
                    }
                    else
                    {
                        tileController.Move(EMoveDirType.Left);
                    }
                }
            }
            else
            {
                if (deltaY >= height / 3 || deltaY <= -height / 3)
                {
                    if (startPosition.y < endPosition.y)
                    {
                        tileController.Move(EMoveDirType.Up);
                    }
                    else
                    {
                        tileController.Move(EMoveDirType.Down);
                    }
                }
            }
        }
    }
}
