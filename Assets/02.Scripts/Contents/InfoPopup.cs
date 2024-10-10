using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class InfoPopup : MonoBehaviour
{
    public GameObject[] foodStarArray = new GameObject[5];

    public Image foodImage;
   
    public TMP_Text foodName;
    public TMP_Text foodDescription;
    
    public TMP_Text costText;
    public Button upgradeButton;
    
    public Button backPanelButton;

    int upgradeCost = 0;

    ERecipeType recipeType;
    int index;
    private void Start()
    {
        backPanelButton.onClick.AddListener(()=> { this.gameObject.SetActive(false); });
        upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        InGameSystem.Instance.levelUpEvent += ShowRecipeItem;
    }

    public void SetInfoPopup(ERecipeType eRecipeType, int index/* Sprite foodSprite, string name, string description, int startCount*/)
    {
        recipeType = eRecipeType;
        this.index = index;

        RecipeLabSaveData recipeLabSaveData = InGameSystem.Instance.GetRecipeLabData(eRecipeType);
        RecipeItemData recipeItemData = recipeLabSaveData.recipeItemDatas[index];
        recipeItemData.isUnlock = true;

        foodImage.sprite = DataManager.Instance.GetFoodSprite(eRecipeType,index);
        foodName.text = DataManager.Instance.GetFoodInfo(eRecipeType,index).name + $"Lv{recipeItemData.level}\n+{recipeItemData.level*0.2}%";
        foodDescription.text = DataManager.Instance.GetFoodInfo(eRecipeType, index).description;

        upgradeCost = DataManager.Instance.GetFoodInfo(eRecipeType, index).starCost * (recipeItemData.level + 1);
        costText.text = $"$ {upgradeCost}";

        if(upgradeCost > InGameSystem.Instance.wealthSaveData.haveMoney)
        {
            upgradeButton.enabled = false;
        }

        for (int i = 0; i < foodStarArray.Length; i++)
        {
            foodStarArray[i].SetActive(false);
        }
        for (int i = 0; i < recipeItemData.level; i++)
        {
            foodStarArray[i].SetActive(true);
        }
    }

    public void OnClickUpgradeButton()
    {
        Debug.Log("업그레이드!!");
        InGameSystem.Instance.ManageWealthData(EWealthType.Money, -upgradeCost);
        InGameSystem.Instance.LevelUpRecipe(recipeType, index);
        if (upgradeCost > InGameSystem.Instance.wealthSaveData.haveMoney)
        {
            upgradeButton.enabled = false;
        }
        RecipeLabSaveData recipeLabSaveData = InGameSystem.Instance.GetRecipeLabData(recipeType);
        RecipeItemData recipeItemData = recipeLabSaveData.recipeItemDatas[index];
        upgradeCost = DataManager.Instance.GetFoodInfo(recipeType, index).starCost * (recipeItemData.level + 1);
        costText.text = $"$ {upgradeCost}";
    }

    public void ShowRecipeItem(ERecipeType recipeType, int index)
    {
            int level = InGameSystem.Instance.GetRecipeLabData(recipeType).recipeItemDatas[index].level;

            for (int i = 0; i < foodStarArray.Length; i++)
            {
                foodStarArray[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < level; i++)
            {
                foodStarArray[i].gameObject.SetActive(true);
            }
    }
}
