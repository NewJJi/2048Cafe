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

    //�밢�� ���� ����
    public bool isCanDiagonal = true;

    public TileController tileController;

    void Start()
    {
        Camera cam = Camera.main;
        
        //orthographic ��忡���� orthographicSize�� ���� ũ��� ���.
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

            //X�� �̵��� Y�� �̵����� Ŭ ��
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                //���콺 Ȥ�� ���� ����� �Ÿ��� �������ִ�.
                if (deltaX >= width / 4 || deltaX <= -width / 4)
                {
                    //���������� �� �۴� -> ���ʿ��� ���������� ��������.
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
