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

    public ERecipeLabType eRecipeType;

    public RecipeViewer recipeViewer;
    public TileController tileController;

    RecipeLabSaveData recipeLabSaveData;

    public void Init()
    {
        RecipeItemData[] recipeItems = GameManager.Instance.GetRecipeItemData(eRecipeType);
        recipeViewer.showRecipeEvent = ShowInfoPopup;
        recipeViewer.Init(recipeItems, eRecipeType);

        recipeLabSaveData = GameManager.Instance.GetRecipeLabData(eRecipeType);
        tileController.Init(recipeLabSaveData,eRecipeType);
        tileController.GetNewRecipeEvent = EnrollNewRecipe;
        GameManager.Instance.MoneyEvent += ActiveExpandButton;

        expandButton.onClick.AddListener(OnClickExpandButton);

        int costIndex = recipeLabSaveData.expandLevel - defaultRecipeLabGridSize;
        expandButtonText.text = $"{GameManager.Instance.Data.expandUpgradeCost[costIndex]} ¿ø";

        ActiveExpandButton(GameManager.Instance.GameMoney);
    }

    public void EnrollNewRecipe(int index)
    {
        recipeViewer.EnrollNewRecipe(eRecipeType, index);
    }

    public void ShowInfoPopup(int recipeIndex)
    {
        GameManager.Instance.UI.ShowRecipeInfoPopupEvent(eRecipeType, recipeIndex);
    }

    public void ActiveExpandButton(int money)
    {
        int costIndex = recipeLabSaveData.expandLevel - defaultRecipeLabGridSize;

        if (GameManager.Instance.Data.expandUpgradeCost[costIndex] > money)
        {
            expandButton.enabled = false;
        }
        else
        {
            expandButton.enabled = true;
        }

        expandButtonText.text = $"{GameManager.Instance.Data.expandUpgradeCost[costIndex]} ¿ø";
    }

    public void OnClickExpandButton()
    {
        int costIndex = recipeLabSaveData.expandLevel - defaultRecipeLabGridSize;
        recipeLabSaveData.expandLevel++;
        tileController.ExpandLaboratory();

        GameManager.Instance.SpendMoney(GameManager.Instance.Data.expandUpgradeCost[costIndex]);
    }
}
