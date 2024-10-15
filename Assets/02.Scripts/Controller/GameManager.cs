using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UiManager UiManager;
    [SerializeField] private RecipeLabController recipeLabController;
    [SerializeField] private InputController inputController;

    private GameDataManager dataManager = new GameDataManager();

    public GameDataManager Data { get { return dataManager; } }
    public UiManager UI { get { return UiManager; } }

    public WealthSaveData wealthSaveData;
    public RecipeLabSaveData beverageSaveData;
    public RecipeLabSaveData bakerySaveData;
    public RecipeLabSaveData desertSaveData; 

    public int GameMoney
    {
        get => wealthSaveData.haveMoney;
        set
        {
            ManageWealthData(EWealthType.Money,value);
        }
    }

    #region Global Event

    public Action<int> MoneyEvent; 
    public Action ItemEvent;

    #endregion

    public async void Awake()
    {
        Instance = this;
        wealthSaveData = await dataManager.LoadWealthSaveDataData();
        beverageSaveData = await dataManager.LoadRecipeLabSaveData("BeverageSaveData");
        bakerySaveData = await dataManager.LoadRecipeLabSaveData("BakerySaveData");
        desertSaveData = await dataManager.LoadRecipeLabSaveData("DesertSaveData");
        await dataManager.LoadAllData();
        Init();
    }

    public void Init()
    {
        UiManager.Init();
        recipeLabController.Init();
    }

    public void BindEvent()
    {
        inputController.swapEvent = recipeLabController.SwapPuzzle;
        inputController.clickTileEvent = recipeLabController.RemoveTile;
    }

    public void ManageWealthData(EWealthType eWealthType, int value)
    {
        switch (eWealthType)
        {
            case EWealthType.Money:
                wealthSaveData.haveMoney += value;
                MoneyEvent?.Invoke(wealthSaveData.haveMoney);
                break;
            case EWealthType.Expand:
                wealthSaveData.expandItemCount += value;
                break;
            case EWealthType.Sort:
                wealthSaveData.sortItemCount += value;
                break;
            case EWealthType.Needle:
                wealthSaveData.needleItemCount += value;
                break;
            case EWealthType.ThrowOut:
                wealthSaveData.throwOutItemCount += value;
                break;
        }
        Debug.Log(eWealthType);
        dataManager.SaveWealthSaveDataData(wealthSaveData);
    }
    public RecipeItemData[] GetRecipeItemData(ERecipeType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                return beverageSaveData.recipeItemDatas;
            case ERecipeType.Bakery:
                return bakerySaveData.recipeItemDatas;
            case ERecipeType.Desert:
                return desertSaveData.recipeItemDatas;
        }

        Debug.Log("Null Exception");
        return null;
    }

    public List<int> GetSpawnValue(ERecipeType eRecipeType)
    {
        List<int> list = new List<int>();
        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                for (int i = 0; i < beverageSaveData.recipeItemDatas.Length; i++)
                {
                    if(beverageSaveData.recipeItemDatas[i].isSpawnMaterial == true)
                    {
                        list.Add(i);
                    }
                }
                break;
            case ERecipeType.Bakery:
                for (int i = 0; i < bakerySaveData.recipeItemDatas.Length; i++)
                {
                    if (bakerySaveData.recipeItemDatas[i].isSpawnMaterial == true)
                    {
                        list.Add(i);
                    }
                }
                break;
            case ERecipeType.Desert:
                for (int i = 0; i < desertSaveData.recipeItemDatas.Length; i++)
                {
                    if (desertSaveData.recipeItemDatas[i].isSpawnMaterial == true)
                    {
                        list.Add(i);
                    }
                }
                break;
        }
        return list;
    }
    public RecipeLabSaveData GetRecipeLabData(ERecipeType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                return beverageSaveData;
            case ERecipeType.Bakery:
                return bakerySaveData;
            case ERecipeType.Desert:
                return desertSaveData;
        }
        Debug.Log("Null Exception");
        return null;
    }
    public Action<ERecipeType, int> levelUpEvent;
    public void LevelUpRecipe(ERecipeType eRecipeType, int index)
    {
        GetRecipeItemData(eRecipeType)[index].level++;
        if(GetRecipeItemData(eRecipeType)[index].level == 5)
        {
            GetRecipeItemData(eRecipeType)[index].isSpawnMaterial = true;
        }

        levelUpEvent?.Invoke(eRecipeType, index);

        SaveRecipeLabData(eRecipeType);
    }
    public void SaveRecipeLabData(ERecipeType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                dataManager.SaveRecipeLabSaveData(beverageSaveData, "BeverageSaveData");
                break;
            case ERecipeType.Bakery:
                dataManager.SaveRecipeLabSaveData(beverageSaveData, "BakerySaveData");
                break;
            case ERecipeType.Desert:
                dataManager.SaveRecipeLabSaveData(beverageSaveData, "DesertSaveData");
                break;
        }
    }
}
