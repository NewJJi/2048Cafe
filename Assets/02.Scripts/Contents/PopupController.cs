using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PopupController : MonoBehaviour
{
    public InfoPopup infoPopup;

    public void Init()
    {
        GameManager.Instance.UI.ShowRecipeInfoPopupEvent = ShowInfoPopup;
    }

    public void ShowInfoPopup(ERecipeLabType eRecipeType, int index)
    {
        infoPopup.gameObject.SetActive(true);
        infoPopup.SetInfoPopup(eRecipeType, index);
    }
}
