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
    public GameObject defaultMaterialText;
    public Image image;
    public Action<int> showRecipeEvent;
    public int recipeIndex;

    private void Start()
    {
        GameManager.Instance.levelUpEvent += ShowRecipeItem;
    }

    public void SetInfo(int recipeIndex, int level, ERecipeLabType recipeType, Action<int> showRecipeEvent, bool isSpawnMaterial)
    {
        this.recipeIndex = recipeIndex;
        image.sprite = GameManager.Instance.Data.GetFoodSprite(recipeType,recipeIndex);
        this.showRecipeEvent = showRecipeEvent;
        defaultMaterialText.SetActive(isSpawnMaterial == true);

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

    public void ShowRecipeItem(ERecipeLabType recipeType, int index)
    {
        if (index == recipeIndex)
        {
            int level = GameManager.Instance.GetRecipeLabData(recipeType).recipeItemDatas[index].level;

            for (int i = 0; i < recipeStar.Length; i++)
            {
                recipeStar[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < level; i++)
            {
                recipeStar[i].gameObject.SetActive(true);
            }
        }
    }
}
