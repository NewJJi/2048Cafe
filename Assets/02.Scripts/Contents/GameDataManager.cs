using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class GameDataManager : MonoBehaviour
{
    DataManager dataManager = new DataManager();

    public SaveData saveData;

    public WealthSaveData wealthSaveData;
    public RecipeLabSaveData beverageSaveData;
    public RecipeLabSaveData bakerySaveData;
    public RecipeLabSaveData desertSaveData;

    public List<Sprite> beverageSpriteList;
    public List<Sprite> bakerySpriteList;
    public List<Sprite> desertSpriteList;

    public TextAsset beverageInfoData;
    public TextAsset bakeryInfoData;
    public TextAsset desertInfoData;

    public FoodDataInfoBundle beverageInfoBundle;
    public FoodDataInfoBundle bakeryInfoBundle;
    public FoodDataInfoBundle desertInfoBundle;

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

    public async Task InitSaveData()
    {
        saveData = await dataManager.LoadDataAsync<SaveData>("SaveData");
    }
    public void InitSaveData2()
    {
        saveData = dataManager.LoadData<SaveData>("SaveData");
    }

    [ContextMenu("Reset Save Data")]
    public void ResetSaveData()
    {
        saveData = new SaveData();
    }

    public FoodDataInfo GetFoodInfo(ERecipeLabType eRecipeType, int index)
    {
        FoodDataInfo foodDataInfo = new FoodDataInfo();
        switch (eRecipeType)
        {
            case ERecipeLabType.Beverage:
                return beverageInfoBundle.beverageDataList[index];
            case ERecipeLabType.Bakery:
                return bakeryInfoBundle.beverageDataList[index];
            case ERecipeLabType.Desert:
                return desertInfoBundle.beverageDataList[index];
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
            case EItemType.SortEvent:
                count = wealthSaveData.sortItemCount;
                break;
            case EItemType.ThrowOutEvent:
                count = wealthSaveData.throwOutItemCount;
                break;
            case EItemType.UpgradeEvent:
                break;
        }
        return count;
    }
}
