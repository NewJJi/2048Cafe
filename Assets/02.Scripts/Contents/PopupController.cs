using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PopupController : MonoBehaviour
{
    public ShopPopup shopPopup;
    public InfoPopup infoPopup;

    public void Init()
    {
        InGameUiController.ShowRecipeInfoPopupEvent = ShowInfoPopup;
    }

    public void ShowInfoPopup(ERecipeType eRecipeType, int index)
    {
        infoPopup.gameObject.SetActive(true);

        Sprite sprite = DataManager.Instance.GetFoodSprite(eRecipeType, index);
        FoodDataInfo foodDataInfo = DataManager.Instance.GetFoodInfo(eRecipeType, index);
        //int startCount = DataManager.Instance.saveData.haveBakeryRecipeStar[index];

        //infoPopup.SetInfoPopup(sprite, foodDataInfo.name, foodDataInfo.description, startCount);
    }
}
