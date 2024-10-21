using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UiManager uiManager;
    [SerializeField] private RecipeLabController recipeLabController;
    [SerializeField] private InputController inputController;
    [SerializeField] private GameDataManager gameData = new GameDataManager();

    public GameDataManager Data { get { return gameData; } }
    public UiManager UI { get { return uiManager; } }

    public bool IsCanSwap = true;

    #region Money
    public int GameMoney
    {
        get => gameData.wealthSaveData.haveMoney;
    }

    public void EarnMoney(int money)
    {
        gameData.wealthSaveData.haveMoney += money;
        ChangeMoney();
    }

    public void SpendMoney(int money)
    {
        gameData.wealthSaveData.haveMoney -= money;
        ChangeMoney();
    }

    public void ChangeMoney()
    {
        MoneyEvent?.Invoke(gameData.wealthSaveData.haveMoney);
        gameData.SaveWealthSaveDataData();
    }
    #endregion

    #region Item
    public void UseItem(EItemType eItemType)
    {
        int remainItemCount = 0;
        switch (eItemType)
        {
            case EItemType.SortItem:
                gameData.wealthSaveData.sortItemCount--;
                remainItemCount = gameData.wealthSaveData.sortItemCount;
                break;
            case EItemType.ThrowOutItem:
                gameData.wealthSaveData.throwOutItemCount--;
                remainItemCount = gameData.wealthSaveData.throwOutItemCount;
                break;
            case EItemType.UpgradeItem:
                gameData.wealthSaveData.upgradeItemCount--;
                remainItemCount = gameData.wealthSaveData.upgradeItemCount;
                break;
        }
        gameData.SaveWealthSaveDataData();
        ItemEvent?.Invoke(eItemType, remainItemCount);
    }
    public void GetItem(EItemType eItemType, int itemCount)
    {
        int remainItemCount = 0;
        switch (eItemType)
        {
            case EItemType.SortItem:
                gameData.wealthSaveData.sortItemCount += itemCount;
                remainItemCount = gameData.wealthSaveData.sortItemCount;
                break;
            case EItemType.ThrowOutItem:
                gameData.wealthSaveData.throwOutItemCount += itemCount;
                remainItemCount = gameData.wealthSaveData.throwOutItemCount;
                break;
            case EItemType.UpgradeItem:
                gameData.wealthSaveData.upgradeItemCount += itemCount;
                remainItemCount = gameData.wealthSaveData.upgradeItemCount;
                break;
        }
        gameData.SaveWealthSaveDataData();
        ItemEvent?.Invoke(eItemType, remainItemCount);
    }
    #endregion

    #region Global Event

    //돈을 쓴다 or 돈을 번다 -> ui표시, 데이터 저장
    public Action<int> MoneyEvent; 
    public Action<EItemType,int> ItemEvent;
    public Action<ERecipeLabType, int> levelUpEvent;

    #endregion

    public async void Awake()
    {
        Instance = this;
        await gameData.LoadAllData();

        BindEvent();
        Init();
    }

    public void Init()
    {
        uiManager.Init();
        recipeLabController.Init();
    }

    public void BindEvent()
    {
        uiManager.ClickRecipeLabEvent += recipeLabController.SwitchRecipeLab;
        uiManager.ClickItemEvent += recipeLabController.UseItem;

        inputController.swapEvent = recipeLabController.SwapPuzzle;
        inputController.clickTileEvent = recipeLabController.ClickTileEvent;
    }

    public RecipeItemData[] GetRecipeItemData(ERecipeLabType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeLabType.Beverage:
                return gameData.beverageSaveData.recipeItemDatas;
            case ERecipeLabType.Bakery:
                return gameData.bakerySaveData.recipeItemDatas;
            case ERecipeLabType.Desert:
                return gameData.desertSaveData.recipeItemDatas;
        }

        Debug.Log("Null Exception");
        return null;
    }
    public List<int> GetSpawnValue(ERecipeLabType eRecipeType)
    {
        List<int> list = new List<int>();
        switch (eRecipeType)
        {
            case ERecipeLabType.Beverage:
                for (int i = 0; i < gameData.beverageSaveData.recipeItemDatas.Length; i++)
                {
                    if(gameData.beverageSaveData.recipeItemDatas[i].isSpawnMaterial == true)
                    {
                        list.Add(i);
                    }
                }
                break;
            case ERecipeLabType.Bakery:
                for (int i = 0; i < gameData.bakerySaveData.recipeItemDatas.Length; i++)
                {
                    if (gameData.bakerySaveData.recipeItemDatas[i].isSpawnMaterial == true)
                    {
                        list.Add(i);
                    }
                }
                break;
            case ERecipeLabType.Desert:
                for (int i = 0; i < gameData.desertSaveData.recipeItemDatas.Length; i++)
                {
                    if (gameData.desertSaveData.recipeItemDatas[i].isSpawnMaterial == true)
                    {
                        list.Add(i);
                    }
                }
                break;
        }
        return list;
    }
    public RecipeLabSaveData GetRecipeLabData(ERecipeLabType eRecipeType)
    {
        switch (eRecipeType)
        {
            case ERecipeLabType.Beverage:
                return gameData.beverageSaveData;
            case ERecipeLabType.Bakery:
                return gameData.bakerySaveData;
            case ERecipeLabType.Desert:
                return gameData.desertSaveData;
        }
        Debug.Log("Null Exception");
        return null;
    }
    public void LevelUpRecipe(ERecipeLabType eRecipeType, int index)
    {
        GetRecipeItemData(eRecipeType)[index].level++;
        if(GetRecipeItemData(eRecipeType)[index].level == 5)
        {
            GetRecipeItemData(eRecipeType)[index].isSpawnMaterial = true;
        }

        levelUpEvent?.Invoke(eRecipeType, index);
        SaveRecipeLabData(eRecipeType);
    }
    public void SaveRecipeLabData(ERecipeLabType eRecipeType)
    {
        gameData.SaveRecipeLabSaveData(eRecipeType);
    }
}
