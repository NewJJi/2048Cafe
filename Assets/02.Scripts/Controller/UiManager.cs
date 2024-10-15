using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UiManager : MonoBehaviour
{
    public Action<ERecipeType,int> ShowRecipeInfoPopupEvent;
    public PopupController popupController;
    public Shop shopPanel;

    public TMP_Text moneyText;
    public Button shopButton;

    public void Init()
    {
        popupController.Init();
        ShowHaveMoney(GameManager.Instance.GameMoney);
        GameManager.Instance.MoneyEvent += ShowHaveMoney;
        shopPanel.Init();

        BindEvent();
    }

    public void BindEvent()
    {
        shopButton.onClick.AddListener(() => { shopPanel.ShowShopPanel(); });
    }

    public void ShowHaveMoney(int haveMoney)
    {
        moneyText.text = $"{haveMoney} ¿ø";
    }
}
