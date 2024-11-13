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
        
        BindEvent();
        recipeViewer.Init(recipeItems, eRecipeType);

        recipeLabSaveData = GameManager.Instance.GetRecipeLabData(eRecipeType);
        //tileController.Init(recipeLabSaveData,eRecipeType);
        tileController.Init(recipeLabSaveData.expandLevel, recipeLabSaveData.gridList, eRecipeType);

        int costIndex = recipeLabSaveData.expandLevel - defaultRecipeLabGridSize;
        expandButtonText.text = $"{NumberTranslator.FormatNumber(GameManager.Instance.Data.expandUpgradeCost[costIndex])}";

        ActiveExpandButton(GameManager.Instance.GameMoney);
    }

    public void BindEvent()
    {
        recipeViewer.showRecipeEvent = ShowInfoPopup;
        tileController.GetNewRecipeEvent = EnrollNewRecipe;
        GameManager.Instance.MoneyEvent += ActiveExpandButton;
        expandButton.onClick.AddListener(OnClickExpandButton);
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
        expandButtonText.text = $"{NumberTranslator.FormatNumber(GameManager.Instance.Data.expandUpgradeCost[costIndex])}";

        //expandButtonText.text = $"{GameManager.Instance.Data.expandUpgradeCost[costIndex]} ¿ø";
    }

    public void OnClickExpandButton()
    {
        int costIndex = recipeLabSaveData.expandLevel - defaultRecipeLabGridSize;
        recipeLabSaveData.expandLevel++;
        tileController.ExpandLaboratory();

        GameManager.Instance.SpendMoney(GameManager.Instance.Data.expandUpgradeCost[costIndex]);
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.ItemButton);
    }

    public void SortPuzzle()
    {
        tileController.SortAllTile();
    }
}
