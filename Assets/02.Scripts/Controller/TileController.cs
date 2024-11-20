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

public class TileController : MonoBehaviour
{
    [Header("Data")]
    public PuzzleData puzzleData = new PuzzleData();
    public List<Vector2> emptyGrid = new List<Vector2>();
    private ERecipeLabType eRecipeType;

    [Header("Tile")]
    public Tile tilePrefab;
    public Transform poolParent;
    private Stack<Tile> tilePool = new Stack<Tile>();

    [Header("Size")]
    public RectTransform contentTransform;
    public int padding;
    private int expandGridCount;
    private int wholeSize;
    private float gridSize;

    [Header("Event")]
    public Action<int> GetNewRecipeEvent;
    private Func<SwapMoney> ShowSwapMoneyEvnet;

    public Image tileBackgroundImage;
    public Transform parentTransform;

    public void Init(int expandLevel, List<List<int>> tileValueList, ERecipeLabType eRecipeType)
    {
        this.eRecipeType = eRecipeType;
        expandGridCount = expandLevel;

        tileBackgroundImage.pixelsPerUnitMultiplier = 1 + (0.5f * (expandGridCount - 2));

        wholeSize = (int)contentTransform.rect.width;
        gridSize = GetGridSize();

        ShowSwapMoneyEvnet = GameManager.Instance.UI.PopMoney;

        for (int y = 0; y < expandGridCount; y++)
        {
            puzzleData.tileColumn.Add(new ListBundle());

            for (int x = 0; x < expandGridCount; x++)
            {
                emptyGrid.Add(new Vector2(y, x));
                puzzleData.tileColumn[y].tileRow.Add(null);
            }
        }

        InitTile(tileValueList);

        if(tileValueList.Count == 0)
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
                    SpawnInitTile(new Vector2(x, y), grid[x][y]);
                    emptyGrid.Remove(new Vector2(x, y));
                }
            }
        }
    }

    #region Act
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
                if (puzzleData.tileColumn[x].tileRow[y] != null)
                {
                    puzzleData.tileColumn[x].tileRow[y].combined = false;
                    //current cell
                    Vector2 cell;

                    //next cell
                    Vector2 next = new Vector2(x, y);

                    do
                    {
                        cell = next;

                        next = new Vector2(cell.x + vector.x, cell.y + vector.y);

                    } while (isInArea(next) && puzzleData.tileColumn[Mathf.RoundToInt(next.x)].tileRow[Mathf.RoundToInt(next.y)] == null);

                    int nx = Mathf.RoundToInt(next.x);
                    int ny = Mathf.RoundToInt(next.y);

                    int cx = Mathf.RoundToInt(cell.x);
                    int cy = Mathf.RoundToInt(cell.y);

                    if (isInArea(next) && !puzzleData.tileColumn[nx].tileRow[ny].combined && puzzleData.tileColumn[nx].tileRow[ny].tileValue == puzzleData.tileColumn[x].tileRow[y].tileValue && puzzleData.tileColumn[x].tileRow[y].tileValue != 134217728)
                    {
                        StartCoroutine(MergeTile(puzzleData.tileColumn[nx].tileRow[ny], puzzleData.tileColumn[x].tileRow[y]));
                        MoveTile(new Vector2(nx, ny), puzzleData.tileColumn[x].tileRow[y]);
                        puzzleData.tileColumn[nx].tileRow[ny] = puzzleData.tileColumn[x].tileRow[y];
                        puzzleData.tileColumn[nx].tileRow[ny].combined = true;
                        puzzleData.tileColumn[x].tileRow[y] = null;

                        emptyGrid.Remove(new Vector2(nx, ny));
                        emptyGrid.Add(new Vector2(x, y));
                        isMoved = true;
                    }
                    else
                    {
                        if (x != cx || y != cy)
                        {
                            MoveTile(new Vector2(cx, cy), puzzleData.tileColumn[x].tileRow[y]);
                            puzzleData.tileColumn[cx].tileRow[cy] = puzzleData.tileColumn[x].tileRow[y];
                            puzzleData.tileColumn[x].tileRow[y] = null;

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
            //Debug.Log("Can't Move");
        }
        else
        {
            GameManager.Instance.NPC.CountUpSwap();
        }

        GameManager.Instance.Sound.PlaySweepSound();
    }
    public void MoveTile(Vector2 destinationGrid, Tile tile)
    {
        Vector2 destinationPosition = GetWorldPositionFromGrid(destinationGrid);
        tile.transform.DOLocalMove(destinationPosition, moveSpeed);
        tile.SetGrid((int)destinationGrid.x, (int)destinationGrid.y);

        puzzleData.tileColumn[(int)destinationGrid.x].tileRow[(int)destinationGrid.y] = tile;
    }
    
    public void SpawnInitTile(Vector2 pos, int num)
    {
        Vector2 spawnPosition = GetWorldPositionFromGrid(pos);

        Tile tile = PopTile();
        tile.transform.localPosition = spawnPosition;

        tile.GrowTile(new Vector2(gridSize, gridSize));

        int x = (int)pos.x;
        int y = (int)pos.y;

        puzzleData.tileColumn[x].tileRow[y] = tile;
        tile.SetGrid(x, y);


        int value = num == 0 ? 0 : (int)Mathf.Log(num, 2);
        Debug.Log($"num : {num}");
        Debug.Log($"value : {value}");
        tile.Change(num, GameManager.Instance.Data.GetFoodSprite(eRecipeType, value-1));

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

        puzzleData.tileColumn[x].tileRow[y] = tile;
        tile.SetGrid(x, y);

        List<int> spawnValue = GameManager.Instance.GetSpawnValue(eRecipeType);

        int ranSapwnNum = UnityEngine.Random.Range(0, spawnValue.Count);
        int value = spawnValue[ranSapwnNum] == 0 ? 1 : spawnValue[ranSapwnNum];
        int logValue = Mathf.RoundToInt(Mathf.Pow(2, spawnValue[ranSapwnNum]+1));
        tile.Change(logValue, GameManager.Instance.Data.GetFoodSprite(eRecipeType, spawnValue[ranSapwnNum]));
        
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


    public IEnumerator MergeTile(Tile tile, Tile newMovedTile)
    {
        yield return new WaitForSeconds(moveSpeed);

        GameManager.Instance.Sound.PlayMoneySound();

        int newValue = newMovedTile.tileValue * 2;

        if(GameManager.Instance.GetRecipeLabData(eRecipeType).maxValue < newValue)
        {
            GameManager.Instance.GetRecipeLabData(eRecipeType).maxValue = newValue;
            GameManager.Instance.UI.popupController.ShowInfoPopup(eRecipeType, (int)Mathf.Log(newValue, 2)-1);
            GetNewRecipeEvent?.Invoke((int)Mathf.Log(newValue, 2)-1);
        }

        newMovedTile.GrowTile(new Vector2(gridSize, gridSize));
        newMovedTile.Change(newValue, GameManager.Instance.Data.GetFoodSprite(eRecipeType,(int)Mathf.Log(newValue, 2)-1));
        PushTile(tile);

        SwapMoney swapMoney = ShowSwapMoneyEvnet?.Invoke();
        swapMoney.transform.position = newMovedTile.transform.position;
        
        float multiple = GameManager.Instance.GetRecipeItemData(eRecipeType)[(int)Mathf.Log(newValue, 2) - 1].level * 0.2f;
        int getCoin = newValue + Mathf.RoundToInt(newValue * multiple);

        swapMoney.Init(getCoin, gridSize);
        GameManager.Instance.EarnMoney(getCoin);
    }
    
    public bool IsCanSwap()
    {
        for (int y = 0; y < expandGridCount; y++)
        {
            for (int x = 0; x < expandGridCount; x++)
            {
                if (puzzleData.tileColumn[x].tileRow[y] == null)
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
                            puzzleData.tileColumn[x + Mathf.RoundToInt(Vtor.x)].tileRow[y + Mathf.RoundToInt(Vtor.y)] != null &&
                            puzzleData.tileColumn[x].tileRow[y].tileValue == puzzleData.tileColumn[x + Mathf.RoundToInt(Vtor.x)].tileRow[y + Mathf.RoundToInt(Vtor.y)].tileValue)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
    public void ConvertSaveData()
    {
        List<List<int>> gridList = new List<List<int>>();
        
        for (int y = 0; y < puzzleData.tileColumn.Count; y++)
        {
            gridList.Add(new List<int>());
            for (int x = 0; x < puzzleData.tileColumn[y].tileRow.Count; x++)
            {
                gridList[y].Add(puzzleData.tileColumn[y].tileRow[x] == null ? 0 : puzzleData.tileColumn[y].tileRow[x].tileValue);
            }
        }

        GameManager.Instance.GetRecipeLabData(eRecipeType).gridList = gridList;
        GameManager.Instance.SaveRecipeLabData(eRecipeType);
    }
    #endregion

    #region Func

    [ContextMenu("SetEmptyTile")]
    public void SetEmptyTile()
    {
        emptyGrid.Clear();

        for (int i = 0; i < puzzleData.tileColumn.Count; i++)
        {
            for (int j = 0; j < puzzleData.tileColumn[i].tileRow.Count; j++)
            {
                if(puzzleData.tileColumn[i].tileRow[j] == null)
                {
                    emptyGrid.Add(new Vector2(i,j));
                }
            }
        }
    }

    #region Expand Grid
    public void ExpandLaboratory()
    {
        expandGridCount++;
        puzzleData.tileColumn.Add(new ListBundle());
        for (int i = 0;i<puzzleData.tileColumn.Count; i++)
        {
            do
            {
                puzzleData.tileColumn[i].tileRow.Insert(0,null);
            } while(puzzleData.tileColumn[i].tileRow.Count<expandGridCount);
        }
        gridSize = GetGridSize();
        SetTileSize();
        SetPositionAllTile();
        SetEmptyTile();
        tileBackgroundImage.pixelsPerUnitMultiplier += 0.5f;
    }
    #endregion

    #region Sort Tile
    [ContextMenu("Sort")]
    public void SortAllTile()
    {
        List<Tile> activeTile = new List<Tile>();

        for (int i = 0; i < puzzleData.tileColumn.Count; i++)
        {
            for (int j = 0; j < puzzleData.tileColumn[i].tileRow.Count; j++)
            {
                if(puzzleData.tileColumn[i].tileRow[j] != null)
                {
                    activeTile.Add(puzzleData.tileColumn[i].tileRow[j]);
                }
            }
        }

        MergeSort(activeTile);

        for (int y = expandGridCount-1; y >= 0; y--)
        {
            for (int x = 0; x < expandGridCount; x++)
            {
                if(activeTile.Count != 0) 
                {
                    puzzleData.tileColumn[x].tileRow[y] = activeTile[0];
                    puzzleData.tileColumn[x].tileRow[y].gameObject.transform.localPosition = GetWorldPositionFromGrid(new Vector2(x, y));
                    activeTile.RemoveAt(0);
                }
                else
                {
                    puzzleData.tileColumn[x].tileRow[y] = null;
                }
            }
        }

        SetEmptyTile();
        ConvertSaveData();
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
        Tile removedTile = puzzleData.tileColumn[tile.gridX].tileRow[tile.gridY];
        puzzleData.tileColumn[tile.gridX].tileRow[tile.gridY] = null;
        PushTile(removedTile);
        ConvertSaveData();
    }

    #region Upgrade Tile
    internal bool UpgradeTile(Tile tile)
    {
        Tile upgradeTile = puzzleData.tileColumn[tile.gridX].tileRow[tile.gridY];
        if(upgradeTile.tileValue >= GameManager.Instance.GetRecipeLabData(eRecipeType).maxValue)
        {
            return false;
        }
        else
        {
            int newValue = upgradeTile.tileValue * 2;
            upgradeTile.Change(newValue, GameManager.Instance.Data.GetFoodSprite(eRecipeType, (int)Mathf.Log(newValue, 2) - 1));
            ConvertSaveData();
            return true;
        }
    }
    #endregion

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
                if (puzzleData.tileColumn[i].tileRow[j] != null)
                {
                    puzzleData.tileColumn[i].tileRow[j].rectTransform.sizeDelta = new Vector2(gridSize, gridSize);
                }
            }
        }
    }
    public void SetPositionAllTile()
    {
        for (int x = 0; x < puzzleData.tileColumn.Count; x++)
        {
            for (int y = 0; y < puzzleData.tileColumn[x].tileRow.Count; y++)
            {
                if (puzzleData.tileColumn[x].tileRow[y] != null)
                {
                    puzzleData.tileColumn[x].tileRow[y].transform.localPosition = GetWorldPositionFromGrid(new Vector2(x,y));
                }
            }
        }
    }
    public float GetGridSize()
    {
        return ((float)wholeSize - (padding * 2) - ((expandGridCount - 1) * padding)) / expandGridCount;
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
    #endregion
}
