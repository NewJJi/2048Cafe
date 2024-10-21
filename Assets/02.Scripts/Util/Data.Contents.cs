using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


[System.Serializable]
public class FoodDataInfo
{
    public int uid;
    public string name;
    public string description;
    public float sellPrice;
    public int starCost;
}
[System.Serializable]
public class FoodDataInfoBundle
{
    public List<FoodDataInfo> beverageDataList;
}

public class HaveRecipe
{
    int startCount;
}

#region Save Data

[System.Serializable]
public class ListBundle
{
    public List<Tile> tileRow = new List<Tile>();
}

[System.Serializable]
public class PuzzleData
{
    public List<ListBundle> tileColumn = new List<ListBundle>();
}

[System.Serializable]
public class WealthSaveData : ILoader
{
    public int haveMoney;
    public int upgradeItemCount;
    public int throwOutItemCount;
    public int sortItemCount;

    public WealthSaveData()
    {
        haveMoney = 0;
        upgradeItemCount = defaultChargedItemCount;
        throwOutItemCount = defaultChargedItemCount;
        sortItemCount = defaultChargedItemCount;
    }
}
public class BeverageSaveData : RecipeLabSaveData { }
public class BakerySaveData : RecipeLabSaveData { }
public class DesertSaveData : RecipeLabSaveData { }

public class SaveData : ILoader
{
    public int haveMoney;

    public int expandItemCount;
    public int needleItemCount;
    public int throwOutItemCount;
    public int sortItemCount;

    public RecipeLabSaveData beverageRecipeData;
    public RecipeLabSaveData bakeryRecipeData;
    public RecipeLabSaveData desertRecipeData;

    public SaveData()
    {
        InitData();
        Debug.Log("Data Init");
    }
    public void InitData()
    {
        haveMoney = 0;

        expandItemCount = defaultChargedItemCount;
        needleItemCount = defaultChargedItemCount;
        throwOutItemCount = defaultChargedItemCount;
        sortItemCount = defaultChargedItemCount;

        beverageRecipeData = new RecipeLabSaveData();
        bakeryRecipeData = new RecipeLabSaveData();
        desertRecipeData = new RecipeLabSaveData();
    }

    public RecipeItemData[] GetRecipeItemData(ERecipeLabType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeLabType.Beverage:
                return beverageRecipeData.recipeItemDatas;
            case ERecipeLabType.Bakery:
                return bakeryRecipeData.recipeItemDatas;
            case ERecipeLabType.Desert:
                return desertRecipeData.recipeItemDatas;
        }

        Debug.Log("Null Exception");
        return null;
    }
    public RecipeLabSaveData GetRecipeLabData(ERecipeLabType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeLabType.Beverage:
                return beverageRecipeData;
            case ERecipeLabType.Bakery:
                return bakeryRecipeData;
            case ERecipeLabType.Desert:
                return desertRecipeData;
        }
        Debug.Log("Null Exception");
        return null;
    }
}

[System.Serializable]
public class RecipeLabSaveData : ILoader
{
    public int maxValue;

    public int expandLevel;

    public List<List<int>> gridList = new List<List<int>>();
    public RecipeItemData[] recipeItemDatas = new RecipeItemData[recipeItemTotalCount];

    public RecipeLabSaveData()
    {
        expandLevel = defaultRecipeLabGridSize;
        recipeItemDatas = new RecipeItemData[recipeItemTotalCount];
        for (int i = 0; i < recipeItemTotalCount; i++)
        {
            recipeItemDatas[i] = new RecipeItemData();
        }
        recipeItemDatas[0].isUnlock = true;
        recipeItemDatas[0].isSpawnMaterial = true;
    }
}

[System.Serializable]
public class RecipeItemData
{
    public bool isUnlock = false;
    public bool isSpawnMaterial = false;
    public int level = 0;
}
#endregion
