using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBundle : MonoBehaviour
{
    public bool init;

    public int xCount;
    public int yCount;

    public int count;

    public List<List<int>> tiles = new List<List<int>>();

    public void Init()
    {
        xCount = 2;
        yCount = 2;
        count = 2;

        List<int> firstTileList = new List<int>();
        firstTileList.Add(0);
        firstTileList.Add(0);

        List<int> secondTileList = new List<int>();
        secondTileList.Add(0);
        secondTileList.Add(0);

        tiles.Add(firstTileList);
        tiles.Add(secondTileList);
    }

    public void IncreaseTile()
    {
        xCount++;
        yCount++;
        count++;

        for (int i = 0; i < yCount; i++)
        {
            tiles[i].Add(0);
        }

        List<int> newTile = new List<int>();
        for (int i = 0; i < xCount; i++)
        {
            newTile.Add(0);
        }
        tiles.Add(newTile);
    }
}
