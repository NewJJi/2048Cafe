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
    private ERecipeLabType eRecipeType;

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

    public Action<int> GetNewRecipeEvent;

    public void Init(RecipeLabSaveData recipeLabSaveData, ERecipeLabType eRecipeType)
    {
        this.eRecipeType = eRecipeType;
        expandGridCount = recipeLabSaveData.expandLevel;

        wholeSize = (int)contentTransform.rect.width;

        poolParent = new GameObject().transform;
        poolParent.parent = this.transform;
        poolParent.transform.localPosition = Vector2.zero;
        poolParent.name = "PoolParent";

        moneyParent = new GameObject().transform;
        moneyParent.parent = this.transform;
        moneyParent.transform.localPosition = Vector2.zero;
        moneyParent.name = "moneyParent";


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
        InitTile(recipeLabSaveData.gridList);

        if (recipeLabSaveData.gridList.Count == 0)
        {
            SpawnTile();
            SpawnTile();
            ConvertSaveData();
        }
    }
    public void InitTile(List<List<int>> grid)
    {
        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[y].Count; x++)
            {
                if (grid[x][y] != 0)
                {
                    SpawnTile(new Vector2(x, y), grid[x][y]);
                }
            }
        }
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

        foreach (int x in xArrayTemp)
        {
            foreach (int y in yArrayTemp)
            {
                if (puzzleData.gridList[x].tiles[y] != null)
                {
                    puzzleData.gridList[x].tiles[y].combined = false;
                    //current cell
                    Vector2 cell;

                    //next cell
                    Vector2 next = new Vector2(x, y);

                    do
                    {
                        cell = next;

                        next = new Vector2(cell.x + vector.x, cell.y + vector.y);

                    } while (isInArea(next) && puzzleData.gridList[Mathf.RoundToInt(next.x)].tiles[Mathf.RoundToInt(next.y)] == null);

                    int nx = Mathf.RoundToInt(next.x);
                    int ny = Mathf.RoundToInt(next.y);

                    int cx = Mathf.RoundToInt(cell.x);
                    int cy = Mathf.RoundToInt(cell.y);

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
            Debug.Log("Can't Move");
        }
    }
    public void ConvertSaveData()
    {
        List<List<int>> gridList = new List<List<int>>();
        
        for (int y = 0; y < puzzleData.gridList.Count; y++)
        {
            gridList.Add(new List<int>());
            for (int x = 0; x < puzzleData.gridList[y].tiles.Count; x++)
            {
                gridList[y].Add(puzzleData.gridList[y].tiles[x] == null ? 0 : puzzleData.gridList[y].tiles[x].tileValue);
            }
        }

        GameManager.Instance.GetRecipeLabData(eRecipeType).gridList = gridList;
        GameManager.Instance.SaveRecipeLabData(eRecipeType);
    }
    public bool IsCanSwap()
    {
        for (int y = 0; y < expandGridCount; y++)
        {
            for (int x = 0; x < expandGridCount; x++)
            {
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
    public void SpawnTile(Vector2 pos, int num)
    {
        Vector2 spawnPosition = GetWorldPositionFromGrid(pos);

        Tile tile = PopTile();
        tile.transform.localPosition = spawnPosition;

        tile.GrowTile(new Vector2(gridSize, gridSize));

        int x = (int)pos.x;
        int y = (int)pos.y;

        puzzleData.gridList[x].tiles[y] = tile;
        tile.SetGrid(x, y);
        int value = num == 0 ? 0 : (int)Mathf.Log(num, 2);
        tile.Change(num, GameManager.Instance.Data.GetFoodSprite(eRecipeType, value-1));
        if (!IsCanSwap())
        {
            Debug.Log("Can't Move!");
        }

        ConvertSaveData();
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

        List<int> spawnValue = GameManager.Instance.GetSpawnValue(eRecipeType);

        int ranSapwnNum = UnityEngine.Random.Range(0, spawnValue.Count);
        int logValue = Mathf.RoundToInt(Mathf.Pow(2, ranSapwnNum+1));
        tile.Change(logValue, GameManager.Instance.Data.GetFoodSprite(eRecipeType, ranSapwnNum));
        
        if (!IsCanSwap())
        {
            Debug.Log("Can't Move!");
        }

        ConvertSaveData();
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
        int newValue = newMovedTile.tileValue * 2;

        if(GameManager.Instance.GetRecipeLabData(eRecipeType).maxValue < newValue)
        {
            GameManager.Instance.GetRecipeLabData(eRecipeType).maxValue = newValue;
            GameManager.Instance.UI.popupController.ShowInfoPopup(eRecipeType, (int)Mathf.Log(newValue, 2)-1);
            GetNewRecipeEvent?.Invoke((int)Mathf.Log(newValue, 2));
        }

        newMovedTile.GrowTile(new Vector2(gridSize, gridSize));
        newMovedTile.Change(newValue, GameManager.Instance.Data.GetFoodSprite(eRecipeType,(int)Mathf.Log(newValue, 2)-1));
        PushTile(tile);

        SwapMoney swapMoney = PopMoney();
        swapMoney.transform.localPosition = newMovedTile.transform.localPosition;

        float multiple = GameManager.Instance.GetRecipeItemData(eRecipeType)[(int)Mathf.Log(newValue, 2) - 1].level * 0.2f;
        int getCoin = newValue + Mathf.RoundToInt(newValue * multiple);

        swapMoney.Init(getCoin, gridSize);
        GameManager.Instance.EarnMoney(getCoin);
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
            } while(puzzleData.gridList[i].tiles.Count<expandGridCount);
        }
        CalculateGridSize();
        SetTileSize();
        SetPositionAllTile();
    }
    #endregion

    #region Sort Tile
    [ContextMenu("Sort")]
    public void SortAllTile()
    {
        List<Tile> activeTile = new List<Tile>();

        for (int i = 0; i < puzzleData.gridList.Count; i++)
        {
            for (int j = 0; j < puzzleData.gridList[i].tiles.Count; j++)
            {
                if(puzzleData.gridList[i].tiles[j] != null)
                {
                    activeTile.Add(puzzleData.gridList[i].tiles[j]);
                }
            }
        }
        MergeSort(activeTile);

        for (int y = expandGridCount-1; y > 0; y--)
        {
            for (int x = 0; x < expandGridCount; x++)
            {
                if(activeTile.Count != 0) 
                {
                    puzzleData.gridList[x].tiles[y] = activeTile[0];
                    puzzleData.gridList[x].tiles[y].gameObject.transform.localPosition = GetWorldPositionFromGrid(new Vector2(x, y));
                    activeTile.RemoveAt(0);
                }
            }
        }
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
    public void RemoveTile(Tile tile)
    {
        Tile removedTile = puzzleData.gridList[tile.gridX].tiles[tile.gridY];
        puzzleData.gridList[tile.gridX].tiles[tile.gridY] = null;
        PushTile(removedTile);
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
    public Vector2 GetGridFromWorldPosition(Vector2 position)
    {
        Vector2 grid = Vector2.zero;

        return grid;
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
        Debug.Log("Push Tile");
        tile.gameObject.SetActive(false);
        tilePool.Push(tile);
    }

    public SwapMoney PopMoney()
    {
        if (moneyPool.Count == 0)
        {
            SwapMoney money = Instantiate(moneyPrefab, moneyParent);
            money.returnEvent = PushMoney;
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
