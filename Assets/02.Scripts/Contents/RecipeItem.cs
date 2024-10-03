using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class RecipeItem : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] recipeStar = new GameObject[5];
    public Image image;
    public GameObject defaultMaterialText;

    public Action<int> showRecipeEvent;
    public int recipeIndex;

    public void SetInfo(int recipeIndex, int level, ERecipeType recipeType, Action<int> showRecipeEvent)
    {
        this.recipeIndex = recipeIndex;
        this.GetComponent<Image>().sprite = DataManager.Instance.GetFoodSprite(recipeType,recipeIndex);
        this.showRecipeEvent = showRecipeEvent;

        for (int i = 0; i < recipeStar.Length; i++)
        {
            recipeStar[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < level; i++)
        {
            recipeStar[i].gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        showRecipeEvent?.Invoke(recipeIndex);
        Debug.Log("레시피 클릭");
    }

    
}
