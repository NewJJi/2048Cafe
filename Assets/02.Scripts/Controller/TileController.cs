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

    public Tile tilePrefab;
    private Stack<Tile> tilePool = new Stack<Tile>();
    private Transform poolParent;

    public SwapMoney moneyPrefab;
    private Stack<SwapMoney> moneyPool = new Stack<SwapMoney>();
    private Transform moneyParent;

    public PuzzleData puzzleData;

    private List<Vector2> emptyGrid = new List<Vector2>();

    public RectTransform contentTransform;
    public int padding;
    private int expandGridCount;
    private int wholeSize;
    private float gridSize;

    public void Init(RecipeLabSaveData recipeLabSaveData, ERecipeType eRecipeType)
    {
        this.eRecipeType = eRecipeType;
        expandGridCount = recipeLabSaveData.expandLevel;

        wholeSize = (int)contentTransform.rect.width;

        poolParent = new GameObject().transform;
        poolParent.parent = this.transform;
        poolParent.transform.localPosition = Vector2.zero;
        poolParent.name = "PoolParent";

        for (int i = 0; i < expandGridCount; i++)
        {
            for (int j = 0; j < expandGridCount; j++)
            {
                emptyGrid.Add(new Vector2(j, i));
            }
        }

        puzzleData = new PuzzleData();

        for (int y = 0; y < expandGridCount; y++)
        {
            puzzleData.gridList.Add(new ListBundle());

            for (int x = 0; x < expandGridCount; x++)
            {
                puzzleData.gridList[y].tiles.Add(null);
            }
        }

        CalculateGridSize();

        SpawnTile();
        SpawnTile();
    }

    public void Move(Define.EMoveDirType dir)
    {
        bool isMoved = false;

        Vector2 vector = Vector2.zero;

        int[] xArrayTemp = new int[expandGridCount];
        int[] yArrayTemp = new int[expandGridCount];

        for (int i = 0; i < xArrayTemp.Length; i++)
        {
            xArrayTemp[i] = i;
        }

        for (int i = 0; i < yArrayTemp.Length; i++)
        {
            yArrayTemp[i] = i;
        }

        //������ ���⺤��
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

        //�׸��� x,y �ݺ�
        foreach (int x in xArrayTemp)
        {
            foreach (int y in yArrayTemp)
            {
                //�׸��尡 ������� ������ ����
                //��� grid�� ��ȸ�� �ʿ䰡 �ֳ�?
                if (puzzleData.gridList[x].tiles[y] != null)
                {
                    //tile combined false�� ����
                    //�ʱ�ȭ
                    puzzleData.gridList[x].tiles[y].combined = false;
                    //current cell
                    Vector2 cell;

                    //next cell
                    Vector2 next = new Vector2(x, y);

                    do
                    {
                        //���� cell�� next�� ����
                        cell = next;

                        //next�� ���� �� + ���⺤��
                        next = new Vector2(cell.x + vector.x, cell.y + vector.y);

                        //�����ȿ� �ְ�, �������� null�� �ƴҶ����� �ݺ�
                        //do while�̱� ������ ������ ����ų� ���� �־ �ϴ� next���� ������ �Ѿ
                    } while (isInArea(next) && puzzleData.gridList[Mathf.RoundToInt(next.x)].tiles[Mathf.RoundToInt(next.y)] == null);

                    //vector2���̾ float���̱� ������ RoundToInt�� ����ȯ�ؼ� �־��ֱ�
                    //

                    int nx = Mathf.RoundToInt(next.x);
                    int ny = Mathf.RoundToInt(next.y);

                    int cx = Mathf.RoundToInt(cell.x);
                    int cy = Mathf.RoundToInt(cell.y);

                    //nx,ny�� cx,cy���� ���⺤�͸� ���� ��

                    //next�� �����ȿ� �ְ�, next�� ���� Ÿ���� value���� ������ �̵�
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
            Debug.Log("����!");
        }

    }
    public bool IsCanSwap()
    {
        //������ ���� �� �̵��� �� �Ҽ� �ִ� ���� �ִ��� Ȯ��

        for (int y = 0; y < expandGridCount; y++)
        {
            for (int x = 0; x < expandGridCount; x++)
            {
                //�� ���� �ϳ��� ������ swap����
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

    public void SpawnTile()
    {
        if (emptyGrid.Count == 0)
        {
            return;
        }

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
            Debug.Log("����!");
        }
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

        SwapMoney swapMoney = PopMoney();
        swapMoney.transform.localPosition = newMovedTile.transform.localPosition;
        swapMoney.Init(newMovedTile.tileValue * 2);
    }

    #region Func

    #region Expand Grid
    public void ExpandLaboratory()
    {
        expandGridCount++;
        puzzleData.gridList.Add(new ListBundle());

        for (int i = 0;i<puzzleData.gridList.Count; i++)
        {
            do
            {
                puzzleData.gridList[i].tiles.Insert(0,null);

            }while(puzzleData.gridList[i].tiles.Count<expandGridCount);
        }
        CalculateGridSize();
        SetTileSize();
        SetPositionAllTile();
    }
    #endregion

    #region Sort Tile
    public void SortAllTile()
    {

    }

    private void MergeSort(List<Tile> tileList)
    {
        if(tileList.Count<=1)
        {
            return;
        }

        int midPoint = Mathf.RoundToInt(tileList.Count*0.5f);

        List<Tile> leftList = new List<Tile>(tileList.GetRange(0,midPoint));
        List<Tile> rightList = new List<Tile>(tileList.GetRange(midPoint,tileList.Count-midPoint));

        MergeSort(leftList);
        MergeSort(rightList);

        Merge(tileList, leftList,rightList);
    }

    public void Merge(List<Tile> resultList, List<Tile> leftList, List<Tile> rightList)
    {
        int leftIndex = 0;
        int rightIndex = 0;
        int resultIndex = 0;

        while(leftList.Count>leftIndex && rightList.Count>rightIndex)
        {
            if(leftList[leftIndex].tileValue >= rightList[rightIndex].tileValue)
            {
                resultList[resultIndex++] = leftList[leftIndex++];
            }
            else
            {
                resultList[resultIndex++] = rightList[rightIndex++];
            }
        }        

        //Guarantee that one side is zero
        //Not Guarantee that both list size is same size

        while(leftIndex < leftList.Count)
        {
            resultList[resultIndex++] = leftList[leftIndex++];
        }

        while(rightIndex < rightList.Count)
        {
            resultList[resultIndex++] = rightList[rightIndex++];
        }
    }

    #endregion

    #region Remove Tile

    public void RemoveTile(Vector2 tileIndex)
    {

    }

    #endregion

    #region Util Func
    private bool isInArea(Vector2 vec)
    {
        return 0 <= vec.x && vec.x < expandGridCount && 0 <= vec.y && vec.y < expandGridCount;
    }
    public Vector2 GetWorldPositionFromGrid(Vector2 grid)
    {
        float pivotPoint = (gridSize / 2) + 10 - (wholeSize / 2);

        float xPosition = pivotPoint + ((gridSize + padding) * grid.x);
        float yPosition = pivotPoint + ((gridSize + padding) * grid.y);

        return new Vector2(xPosition, yPosition);
    }
    public void SetTileSize()
    {
        for (int i = 0; i < expandGridCount; i++)
        {
            for (int j = 0; j < expandGridCount; j++)
            {
                if (puzzleData.gridList[i].tiles[j] != null)
                {
                    puzzleData.gridList[i].tiles[j].rectTransform.sizeDelta = new Vector2(gridSize, gridSize);
                }
            }
        }
    }
    public void SetPositionAllTile()
    {
        for (int x = 0; x < puzzleData.gridList.Count; x++)
        {
            for (int y = 0; y < puzzleData.gridList[x].tiles.Count; y++)
            {
                if (puzzleData.gridList[x].tiles[y] != null)
                {
                    puzzleData.gridList[x].tiles[y].transform.localPosition = GetWorldPositionFromGrid(new Vector2(x,y));
                }
            }
        }
    }
    public void CalculateGridSize()
    {
        gridSize = ((float)wholeSize - (padding * 2) - ((expandGridCount - 1) * padding)) / expandGridCount;
    }

    #endregion

    #endregion

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

    public SwapMoney PopMoney()
    {
        if (moneyPool.Count == 0)
        {
            SwapMoney money = Instantiate(moneyPrefab, moneyParent);
            moneyPool.Push(money);
        }

        SwapMoney swapMoney = moneyPool.Pop();
        swapMoney.gameObject.SetActive(true);
        return swapMoney;
    }
    public void PushMoney(SwapMoney money)
    {
        money.gameObject.SetActive(false);
        moneyPool.Push(money);
    }

    #endregion
}
