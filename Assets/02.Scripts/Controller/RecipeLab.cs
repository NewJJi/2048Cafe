using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class RecipeLab : MonoBehaviour
{
    public Button expandButton;

    public ERecipeLabType eRecipeLabType;
    public ERecipeType eRecipeType;
    public List<Sprite> spriteList;

    public RecipeViewer recipeViewer;
    public TileController tileController;

    public Action advertiseEvent;

    public void Init()
    {
        RecipeItemData[] recipeItems = InGameSystem.Instance.saveData.GetRecipeItemData(eRecipeType);
        recipeViewer.Init(recipeItems, eRecipeType);
        recipeViewer.showRecipeEvent = ShowInfoPopup;

        RecipeLabSaveData recipeLabSaveData = InGameSystem.Instance.saveData.GetRecipeLabData(eRecipeType);
        tileController.Init(recipeLabSaveData,eRecipeType);
    }

    public void ShowInfoPopup(int recipeIndex)
    {
        InGameUiController.ShowRecipeInfoPopupEvent(eRecipeType, recipeIndex);
    }

    public void GetNewRecipe() { }

    public void PlayAdvertise()
    {

    }
}
