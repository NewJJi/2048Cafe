using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class GameDataManager : MonoBehaviour
{
    DataManager dataManager = new DataManager();

    public WealthSaveData wealthSaveData;
    public RecipeLabSaveData beverageSaveData;
    public RecipeLabSaveData bakerySaveData;
    public RecipeLabSaveData desertSaveData;

    public List<Sprite> beverageSpriteList;
    public List<Sprite> bakerySpriteList;
    public List<Sprite> desertSpriteList;
    public List<Sprite> NPCSpriteList;

    public TextAsset beverageInfoData;
    public TextAsset bakeryInfoData;
    public TextAsset desertInfoData;

    public FoodDataInfoBundle beverageInfoBundle = new FoodDataInfoBundle();
    public FoodDataInfoBundle bakeryInfoBundle = new FoodDataInfoBundle();
    public FoodDataInfoBundle desertInfoBundle = new FoodDataInfoBundle();

    public int[] expandUpgradeCost;

    public async Task LoadAllData()
    {
        wealthSaveData = await LoadWealthSaveDataData();
        beverageSaveData = await LoadRecipeLabSaveData("BeverageSaveData");
        bakerySaveData = await LoadRecipeLabSaveData("BakerySaveData");
        desertSaveData = await LoadRecipeLabSaveData("DesertSaveData");
    }

    [ContextMenu("Food Data Init")]
    public void InitFoodData()
    {
        beverageInfoBundle = JsonConvert.DeserializeObject<FoodDataInfoBundle>(beverageInfoData.text);
        bakeryInfoBundle = JsonConvert.DeserializeObject<FoodDataInfoBundle>(bakeryInfoData.text);
        desertInfoBundle = JsonConvert.DeserializeObject<FoodDataInfoBundle>(desertInfoData.text);
    }

    [ContextMenu("Reset Save Data")]
    public void ResetSaveData()
    {
        wealthSaveData = new WealthSaveData();
        beverageSaveData = new RecipeLabSaveData();
        bakerySaveData = new RecipeLabSaveData();
        desertSaveData = new RecipeLabSaveData();
        SaveData();
    }

    [ContextMenu("Save Save Data")]
    public void SaveData()
    {
        SaveWealthSaveDataData();
        SaveRecipeLabSaveData(ERecipeLabType.Beverage);
        SaveRecipeLabSaveData(ERecipeLabType.Bakery);
        SaveRecipeLabSaveData(ERecipeLabType.Desert);
    }

    public FoodDataInfo GetFoodInfo(ERecipeLabType eRecipeType, int index)
    {
        switch (eRecipeType)
        {
            case ERecipeLabType.Beverage:
                return beverageInfoBundle.foodDataList[index];
            case ERecipeLabType.Bakery:
                return bakeryInfoBundle.foodDataList[index];
            case ERecipeLabType.Desert:
                return desertInfoBundle.foodDataList[index];
            default:
                Debug.LogError("Exception");
                return null;
        }
    }

    public Sprite GetFoodSprite(ERecipeLabType eRecipeType, int index)
    {
        switch (eRecipeType)
        {
            case ERecipeLabType.Beverage:
                return beverageSpriteList[index];
            case ERecipeLabType.Bakery:
                return bakerySpriteList[index];
            case ERecipeLabType.Desert:
                return desertSpriteList[index];
            default:
                Debug.LogError("Exception");
                return null;
        }
    }

    public Sprite GetRandomNPCSprite()
    {
        int ranIndex = Random.Range(0, NPCSpriteList.Count);
        return NPCSpriteList[ranIndex];
    }

    public void SaveWealthSaveDataData()
    {
        dataManager.SaveData<WealthSaveData>(wealthSaveData, "WealthSaveData");
    }

    public void SaveRecipeLabSaveData(ERecipeLabType eRecipeLabType)
    {
        switch (eRecipeLabType)
        {
            case ERecipeLabType.Beverage:
                dataManager.SaveData<RecipeLabSaveData>(beverageSaveData, "BeverageSaveData");
                break;
            case ERecipeLabType.Bakery:
                dataManager.SaveData<RecipeLabSaveData>(bakerySaveData, "BakerySaveData");
                break;
            case ERecipeLabType.Desert:
                dataManager.SaveData<RecipeLabSaveData>(desertSaveData, "DesertSaveData");
                break;
        }
    }

    public async Task<RecipeLabSaveData> LoadRecipeLabSaveData(string path)
    {
        return await dataManager.LoadDataAsync<RecipeLabSaveData>(path);
    }
    public async Task<WealthSaveData> LoadWealthSaveDataData()
    {
        return await dataManager.LoadDataAsync<WealthSaveData>("WealthSaveData");
    }

    public int GetItemCount(EItemType eItemType)
    {
        int count = 0;
        switch (eItemType)
        {
            case EItemType.SortItem:
                count = wealthSaveData.sortItemCount;
                break;
            case EItemType.ThrowOutItem:
                count = wealthSaveData.throwOutItemCount;
                break;
            case EItemType.UpgradeItem:
                count = wealthSaveData.upgradeItemCount;
                break;
        }
        return count;
    }
}
