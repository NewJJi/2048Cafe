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

    //대각선 가능 여부
    public bool isCanDiagonal = true;

    //where is the position init?
    private float swapMinArea;
    private float swapMaxArea;

    public TileController currentTileController;

    private void Start()
    {
        Camera cam = Camera.main;
        
        //orthographic 모드에서는 orthographicSize가 절반 크기로 축소.
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

            //X축 이동이 Y축 이동보다 클 때
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                //마우스 혹은 손이 충분히 거리가 벌어져있다.
                if (deltaX >= width / 4 || deltaX <= -width / 4)
                {
                    //시작지점이 더 작다 -> 왼쪽에서 오른쪽으로 움직였다.
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

    public GraphicRaycaster raycaster; // Canvas에 부착된 GraphicRaycaster
    public EventSystem eventSystem;    // EventSystem 오브젝트
    void GetTopUIElementUnderMouse()
    {
        // 1. PointerEventData 생성: 현재 마우스 포지션을 설정
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition // 마우스 포지션을 입력
        };

        // 2. Raycast 결과를 저장할 리스트 생성
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // 3. GraphicRaycaster를 사용해 UI에 Raycast 수행
        raycaster.Raycast(pointerData, raycastResults);

        // 4. Raycast 결과 처리 (가장 위의 UI 요소 가져오기)
        if (raycastResults.Count > 0)
        {
            // 가장 위에 있는 첫 번째 UI 요소
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
