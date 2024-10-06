using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class InGameUiController : MonoBehaviour
{
    public static Action<ERecipeType,int> ShowRecipeInfoPopupEvent;
    public PopupController popupController;
    public TMP_Text moneyText;
    
    public void Init()
    {
        popupController.Init();
        ShowHaveMoney(InGameSystem.Instance.GameMoney);
        InGameSystem.Instance.MoneyEvent += ShowHaveMoney;
    }

    public void ShowHaveMoney(int haveMoney)
    {
        moneyText.text = $"{haveMoney} ¿ø";
    }
}
