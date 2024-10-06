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

    public RecipeItemData[] GetRecipeItemData(ERecipeType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                return beverageRecipeData.recipeItemDatas;
            case ERecipeType.Bakery:
                return bakeryRecipeData.recipeItemDatas;
            case ERecipeType.Desert:
                return desertRecipeData.recipeItemDatas;
        }

        Debug.Log("Null Exception");
        return null;
    }
    public RecipeLabSaveData GetRecipeLabData(ERecipeType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                return beverageRecipeData;
            case ERecipeType.Bakery:
                return bakeryRecipeData;
            case ERecipeType.Desert:
                return desertRecipeData;
        }
        Debug.Log("Null Exception");
        return null;
    }
}

public class RecipeLabSaveData
{
    public int expandLevel;

    public List<List<int>> gridList;
    public RecipeItemData[] recipeItemDatas = new RecipeItemData[recipeItemTotalCount];

    public RecipeLabSaveData()
    {
        expandLevel = defaultRecipeLabGridSize;
        recipeItemDatas = new RecipeItemData[recipeItemTotalCount];
        for (int i = 0; i < recipeItemTotalCount; i++)
        {
            recipeItemDatas[i] = new RecipeItemData();
        }
    }
}

[System.Serializable]
public class RecipeItemData
{
    public bool isUnlock = false;
    public int level = 0;
}
#endregion
