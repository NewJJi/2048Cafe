using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;
using UnityEngine.Pool;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Newtonsoft.Json;

[System.Serializable]
public class ListBundle
{
    public List<Tile> tiles = new List<Tile>();
}

[System.Serializable]
public class PuzzleData
{
    public List<ListBundle> gridList = new List<ListBundle>();
}

public class TileController : MonoBehaviour
{
    private ERecipeType eRecipeType;

    Stack<Tile> tilePool = new Stack<Tile>();
    Transform poolParent;

    public Tile tilePrefab;

    public TileBundle tileBundle;

    public Transform gridPanel;

    int gridCount;

    public PuzzleData puzzleData;

    public List<Vector2> emptyGrid;

    public GridLayoutGroup gridLayoutGroup;
    public RectTransform contentTransform;
    public int padding;
    public float gridSize;
    public int wholeSize;

    public void Init(RecipeLabSaveData recipeLabSaveData, ERecipeType eRecipeType)
    {
        this.eRecipeType = eRecipeType;
        gridCount = recipeLabSaveData.expandLevel;

        wholeSize = (int)contentTransform.rect.width;

        poolParent = new GameObject().transform;
        poolParent.parent = this.transform;
        poolParent.transform.localPosition = Vector2.zero;
        poolParent.name = "PoolParent";

        for (int i = 0; i < gridCount; i++)
        {
            for (int j = 0; j < gridCount; j++)
            {
                emptyGrid.Add(new Vector2(j, i));
            }
        }

        puzzleData = new PuzzleData();

        for (int y = 0; y < gridCount; y++)
        {
            puzzleData.gridList.Add(new ListBundle());

            for (int x = 0; x < gridCount; x++)
            {
                puzzleData.gridList[y].tiles.Add(null);
            }
        }

        CalculateGridSize();

        SpawnTile();
        SpawnTile();
    }

    public void SetSize()
    {
        gridCount++;
        CalculateGridSize();

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < gridCount; j++)
            {
                emptyGrid.Add(new Vector2(j, i));
            }
        }

        puzzleData.gridList.Add(new ListBundle());
        for (int x = 0; x < gridCount; x++)
        {
            puzzleData.gridList[gridCount - 1].tiles.Add(null);
        }
    }

    public void SetTileSize()
    {
        for (int i = 0; i < gridCount; i++)
        {
            for (int j = 0; j < gridCount; j++)
            {
                if (puzzleData.gridList[i].tiles[j] != null)
                {
                    puzzleData.gridList[i].tiles[j].rectTransform.sizeDelta = new Vector2(gridSize, gridSize);
                }
            }
        }
    }

    public void Move(Define.EMoveDirType dir)
    {
        bool isMoved = false;

        Vector2 vector = Vector2.zero;

        int[] xArrayTemp = new int[gridCount];
        int[] yArrayTemp = new int[gridCount];

        for (int i = 0; i < xArrayTemp.Length; i++)
        {
            xArrayTemp[i] = i;
        }

        for (int i = 0; i < yArrayTemp.Length; i++)
        {
            yArrayTemp[i] = i;
        }

        //움직임 방향벡터
        switch (dir)
        {
            case EMoveDirType.Up:
                vector = Vector2.up;
                System.Array.Reverse(yArrayTemp);
                break;
            case EMoveDirType.Down:
                vector = -Vector2.up;
                break;
            case EMoveDirType.Right:
                vector = Vector2.right;
                System.Array.Reverse(xArrayTemp);
                break;
            case EMoveDirType.Left:
                vector = Vector2.left;
                break;
        }

        //그리드 x,y 반복
        foreach (int x in xArrayTemp)
        {
            foreach (int y in yArrayTemp)
            {
                //그리드가 비어있지 않으면 실행
                //모든 grid를 순회할 필요가 있나?
                if (puzzleData.gridList[x].tiles[y] != null)
                {
                    //tile combined false로 시작
                    //초기화
                    puzzleData.gridList[x].tiles[y].combined = false;
                    //current cell
                    Vector2 cell;

                    //next cell
                    Vector2 next = new Vector2(x, y);

                    do
                    {
                        //현재 cell은 next로 시작
                        cell = next;

                        //next는 현재 셀 + 방향벡터
                        next = new Vector2(cell.x + vector.x, cell.y + vector.y);

                        //범위안에 있고, 다음셀이 null이 아닐때까지 반복
                        //do while이기 때문에 범위에 벗어나거나 셀이 있어도 일단 next셀을 가지고 넘어감
                    } while (isInArea(next) && puzzleData.gridList[Mathf.RoundToInt(next.x)].tiles[Mathf.RoundToInt(next.y)] == null);

                    //vector2값이어서 float형이기 때문에 RoundToInt로 형변환해서 넣어주기
                    //

                    int nx = Mathf.RoundToInt(next.x);
                    int ny = Mathf.RoundToInt(next.y);

                    int cx = Mathf.RoundToInt(cell.x);
                    int cy = Mathf.RoundToInt(cell.y);

                    //nx,ny는 cx,cy에서 방향벡터를 더한 값

                    //next가 범위안에 있고, next와 시작 타일의 value값이 같으면 이동
                    if (isInArea(next) && !puzzleData.gridList[nx].tiles[ny].combined && puzzleData.gridList[nx].tiles[ny].tileValue == puzzleData.gridList[x].tiles[y].tileValue)
                    {
                        StartCoroutine(MergeTile(puzzleData.gridList[nx].tiles[ny], puzzleData.gridList[x].tiles[y]));
                        MoveTile(new Vector2(nx, ny), puzzleData.gridList[x].tiles[y]);
                        puzzleData.gridList[nx].tiles[ny] = puzzleData.gridList[x].tiles[y];
                        puzzleData.gridList[nx].tiles[ny].combined = true;
                        puzzleData.gridList[x].tiles[y] = null;

                        emptyGrid.Remove(new Vector2(nx, ny));
                        emptyGrid.Add(new Vector2(x, y));
                        isMoved = true;
                    }
                    else
                    {
                        if (x != cx || y != cy)
                        {
                            MoveTile(new Vector2(cx, cy), puzzleData.gridList[x].tiles[y]);
                            puzzleData.gridList[cx].tiles[cy] = puzzleData.gridList[x].tiles[y];
                            puzzleData.gridList[x].tiles[y] = null;

                            emptyGrid.Remove(new Vector2(cx, cy));
                            emptyGrid.Add(new Vector2(x, y));
                            isMoved = true;
                        }
                    }
                }
            }
        }

        if (isMoved == true)
        {
            StartCoroutine(CoSpawnTile());
        }


        if (!IsCanSwap())
        {
            Debug.Log("실패!");
        }

    }

    public bool IsCanSwap()
    {
        //스왑이 끝난 후 이동을 더 할수 있는 곳이 있는지 확인

        for (int y = 0; y < gridCount; y++)
        {
            for (int x = 0; x < gridCount; x++)
            {
                //빈 값이 하나라도 있으면 swap가능
                if (puzzleData.gridList[x].tiles[y] == null)
                {
                    return true;
                }
                else
                {
                    for (int z = 0; z <= 3; z++)
                    {
                        Vector2 Vtor = Vector2.zero;

                        switch (z)
                        { // keys
                            case 0:
                                Vtor = Vector2.up;
                                break;
                            case 1:
                                Vtor = -Vector2.up;
                                break;
                            case 2:
                                Vtor = Vector2.right;
                                break;
                            case 3:
                                Vtor = -Vector2.right;
                                break;
                        }

                        if (isInArea(Vtor + new Vector2(x, y)) &&
                            puzzleData.gridList[x + Mathf.RoundToInt(Vtor.x)].tiles[y + Mathf.RoundToInt(Vtor.y)] != null &&
                            puzzleData.gridList[x].tiles[y].tileValue == puzzleData.gridList[x + Mathf.RoundToInt(Vtor.x)].tiles[y + Mathf.RoundToInt(Vtor.y)].tileValue)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private bool isInArea(Vector2 vec)
    {
        return 0 <= vec.x && vec.x < gridCount && 0 <= vec.y && vec.y < gridCount;
    }

    //좌측 하단부터 시작
    public Vector2 GetWorldPositionFromGrid(Vector2 grid)
    {
        float pivotPoint = (gridSize / 2) + 10 - (wholeSize / 2);

        float xPosition = pivotPoint + ((gridSize + padding) * grid.x);
        float yPosition = pivotPoint + ((gridSize + padding) * grid.y);

        return new Vector2(xPosition, yPosition);
    }

    public void SpawnTile()
    {
        if (emptyGrid.Count == 0)
        {
            return;
        }

        //빈 공간
        int ranNum = UnityEngine.Random.Range(0, emptyGrid.Count);

        Vector2 spawnPosition = GetWorldPositionFromGrid(emptyGrid[ranNum]);

        Tile tile = PopTile();
        tile.transform.localPosition = spawnPosition;

        tile.GrowTile(new Vector2(gridSize, gridSize));

        int x = (int)emptyGrid[ranNum].x;
        int y = (int)emptyGrid[ranNum].y;

        emptyGrid.RemoveAt(ranNum);

        puzzleData.gridList[x].tiles[y] = tile;
        tile.SetGrid(x, y);
        tile.Change(2, DataManager.Instance.GetFoodSprite(eRecipeType,0));
        if (!IsCanSwap())
        {
            Debug.Log("실패!");
        }
    }

    [ContextMenu("Expand")]
    public void ExpandLaboratory()
    {

    }
    public IEnumerator CoSpawnTile()
    {
        yield return new WaitForSeconds(moveSpeed);
        SpawnTile();
    }
    public void MoveTile(Vector2 destinationGrid, Tile tile)
    {
        Vector2 destinationPosition = GetWorldPositionFromGrid(destinationGrid);
        tile.transform.DOLocalMove(destinationPosition, moveSpeed);
        tile.SetGrid((int)destinationGrid.x, (int)destinationGrid.y);

        puzzleData.gridList[(int)destinationGrid.x].tiles[(int)destinationGrid.y] = tile;
    }
    public IEnumerator MergeTile(Tile tile, Tile newMovedTile)
    {
        yield return new WaitForSeconds(moveSpeed);
        newMovedTile.GrowTile(new Vector2(gridSize, gridSize));
        newMovedTile.Change(newMovedTile.tileValue * 2, DataManager.Instance.GetFoodSprite(eRecipeType,(int)Mathf.Log(newMovedTile.tileValue * 2, 2)));
        PushTile(tile);
    }
    [ContextMenu("CalculateGridSize")]
    public void CalculateGridSize()
    {
        gridSize = ((float)wholeSize - (padding * 2) - ((gridCount - 1) * padding)) / gridCount;
        gridLayoutGroup.cellSize = new Vector3(gridSize, gridSize, 0);
    }

    #region pool
    public Tile PopTile()
    {
        if (tilePool.Count == 0)
        {
            Tile newTile = Instantiate(tilePrefab, poolParent);
            tilePool.Push(newTile);
        }

        Tile tile = tilePool.Pop();
        tile.gameObject.SetActive(true);
        return tile;
    }
    public void PushTile(Tile tile)
    {
        tile.gameObject.SetActive(false);
        tilePool.Push(tile);
    }
    #endregion
}
