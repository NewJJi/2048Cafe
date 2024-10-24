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

    ERecipeLabType recipeType;
    int index;
    private void Start()
    {
        backPanelButton.onClick.AddListener(()=> 
        { 
            this.gameObject.SetActive(false);
            GameManager.Instance.IsCanSwap = true;
        });

        upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        GameManager.Instance.levelUpEvent += ShowRecipeItem;
    }

    public void SetInfoPopup(ERecipeLabType eRecipeType, int index/* Sprite foodSprite, string name, string description, int startCount*/)
    {
        GameManager.Instance.IsCanSwap = false;

        recipeType = eRecipeType;
        this.index = index;

        RecipeLabSaveData recipeLabSaveData = GameManager.Instance.GetRecipeLabData(eRecipeType);
        RecipeItemData recipeItemData = recipeLabSaveData.recipeItemDatas[index];
        recipeItemData.isUnlock = true;

        foodImage.sprite = GameManager.Instance.Data.GetFoodSprite(eRecipeType,index);
        foodName.text = GameManager.Instance.Data.GetFoodInfo(eRecipeType,index).name + $"Lv{recipeItemData.level}\n+{recipeItemData.level*0.2}%";
        foodDescription.text = GameManager.Instance.Data.GetFoodInfo(eRecipeType, index).description;

        upgradeCost = GameManager.Instance.Data.GetFoodInfo(eRecipeType, index).starCost * (recipeItemData.level + 1);
        costText.text = $"$ {upgradeCost}";

        upgradeButton.interactable = true;

        if (upgradeCost > GameManager.Instance.GameMoney)
        {
            upgradeButton.interactable = false;
        }

        if(recipeItemData.level == 5)
        {
            costText.text = "Max";
            upgradeButton.interactable = false;
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
        GameManager.Instance.SpendMoney(upgradeCost);
        GameManager.Instance.LevelUpRecipe(recipeType, index);
        if (upgradeCost > GameManager.Instance.GameMoney)
        {
            upgradeButton.enabled = false;
        }
        RecipeLabSaveData recipeLabSaveData = GameManager.Instance.GetRecipeLabData(recipeType);
        RecipeItemData recipeItemData = recipeLabSaveData.recipeItemDatas[index];
        upgradeCost = GameManager.Instance.Data.GetFoodInfo(recipeType, index).starCost * (recipeItemData.level + 1);
        costText.text = $"$ {upgradeCost}";
        if (recipeItemData.level == 5)
        {
            costText.text = "Max";
            upgradeButton.interactable = false;
        }
    }

    public void ShowRecipeItem(ERecipeLabType recipeType, int index)
    {
            int level = GameManager.Instance.GetRecipeLabData(recipeType).recipeItemDatas[index].level;

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
