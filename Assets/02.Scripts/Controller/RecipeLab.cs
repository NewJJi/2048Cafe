using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class RecipeLab : MonoBehaviour
{
    public Button expandButton;
    public TMP_Text expandButtonText;
    private int expandLevel;

    public ERecipeLabType eRecipeLabType;
    public ERecipeType eRecipeType;
    public List<Sprite> spriteList;

    public RecipeViewer recipeViewer;
    public TileController tileController;

    public Action advertiseEvent;

    RecipeLabSaveData recipeLabSaveData;
    public void Init()
    {
        RecipeItemData[] recipeItems = InGameSystem.Instance.saveData.GetRecipeItemData(eRecipeType);
        recipeViewer.Init(recipeItems, eRecipeType);
        recipeViewer.showRecipeEvent = ShowInfoPopup;

        recipeLabSaveData = InGameSystem.Instance.saveData.GetRecipeLabData(eRecipeType);
        tileController.Init(recipeLabSaveData,eRecipeType);

        InGameSystem.Instance.MoneyEvent += ActiveExpandButton;

        expandButton.onClick.AddListener(OnClickExpandButton);

        int costIndex = recipeLabSaveData.expandLevel - defaultRecipeLabGridSize;
        expandButtonText.text = $"{DataManager.Instance.expandUpgradeCost[costIndex]} ¿ø";

        ActiveExpandButton(InGameSystem.Instance.GameMoney);
    }

    public void ShowInfoPopup(int recipeIndex)
    {
        InGameUiController.ShowRecipeInfoPopupEvent(eRecipeType, recipeIndex);
    }

    public void GetNewRecipe() { }

    public void ActiveExpandButton(int money)
    {
        int costIndex = recipeLabSaveData.expandLevel - defaultRecipeLabGridSize;

        if (DataManager.Instance.expandUpgradeCost[costIndex] > money)
        {
            expandButton.enabled = false;
        }
        else
        {
            expandButton.enabled = true;
        }

        expandButtonText.text = $"{DataManager.Instance.expandUpgradeCost[costIndex]} ¿ø";
    }

    public void OnClickExpandButton()
    {
        int costIndex = recipeLabSaveData.expandLevel - defaultRecipeLabGridSize;
        recipeLabSaveData.expandLevel++;
        tileController.ExpandLaboratory();

        InGameSystem.Instance.GameMoney = -DataManager.Instance.expandUpgradeCost[costIndex];
    }
}
