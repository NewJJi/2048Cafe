using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class RecipeViewer : MonoBehaviour
{
    public Transform recipeParent;
    public RecipeItem recipeItemPrefab;
    public Action<int> showRecipeEvent;

    public void Init(RecipeItemData[] recipeItemDatas, ERecipeType eRecipeType)
    {
        for (int i = 0; i < recipeItemDatas.Length; i++)
        {
            if(recipeItemDatas[i].isUnlock == true)
            {
                RecipeItem recipeItem = Instantiate(recipeItemPrefab,recipeParent);
                recipeItem.SetInfo(i , recipeItemDatas[i].level, eRecipeType, showRecipeEvent);
            }
        }
    }
}
