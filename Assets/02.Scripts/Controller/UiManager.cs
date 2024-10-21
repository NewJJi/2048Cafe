using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UiManager : MonoBehaviour
{
    public PopupController popupController;

    public TMP_Text moneyText;

    [Header("Shop")]
    public Shop shopPanel;
    public Button shopOpenButton;

    [Header("RecipeLabButton")]
    public Button beverageRecipeLabButton;
    public Button bakeryRecipeLabButton;
    public Button desertRecipeLabButton;

    [Header("Use Item")]
    public Button throwOutButton;
    public TMP_Text throwOutItemCountText;

    public Button upgradeButton;
    public TMP_Text upgradeItemCountText;

    public Button sortButton;
    public TMP_Text sortItemCountText;

    public SwapMoney moneyPrefab;
    public Transform moneyParent;
    private Stack<SwapMoney> moneyPool = new Stack<SwapMoney>();

    #region Event
    //info �г� �̺�Ʈ
    public Action<ERecipeLabType, int> ShowRecipeInfoPopupEvent;

    //������ ������ ���� �̺�Ʈ
    public Action<ERecipeLabType> ClickRecipeLabEvent;

    //������ Ŭ�� �̺�Ʈ
    public Action<EItemType> ClickItemEvent;
    #endregion

    public void Init()
    {
        BindEvent();

        popupController.Init();
        shopPanel.Init();

        ShowHaveMoney(GameManager.Instance.GameMoney);

        ShowItemData(EItemType.ThrowOutItem, GameManager.Instance.Data.GetItemCount(EItemType.ThrowOutItem));
        ShowItemData(EItemType.UpgradeItem, GameManager.Instance.Data.GetItemCount(EItemType.UpgradeItem));
        ShowItemData(EItemType.SortItem, GameManager.Instance.Data.GetItemCount(EItemType.SortItem));
    }

    public void BindEvent()
    {
        GameManager.Instance.MoneyEvent += ShowHaveMoney;
        GameManager.Instance.ItemEvent += ShowItemData;

        shopOpenButton.onClick.AddListener(() => shopPanel.ShowShopPanel());

        throwOutButton.onClick.AddListener(()=>ClickItemEvent?.Invoke(EItemType.ThrowOutItem));
        upgradeButton.onClick.AddListener(() => ClickItemEvent?.Invoke(EItemType.UpgradeItem));
        sortButton.onClick.AddListener(() => ClickItemEvent?.Invoke(EItemType.SortItem));

        beverageRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabEvent?.Invoke(ERecipeLabType.Beverage); });
        bakeryRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabEvent?.Invoke(ERecipeLabType.Bakery); });
        desertRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabEvent?.Invoke(ERecipeLabType.Desert); });
    }

    public void ShowHaveMoney(int haveMoney)
    {
        moneyText.text = $"{haveMoney} ��";
    }

    public void ShowItemData(EItemType eItemType, int count)
    {
        bool isRemainItem = count == 0 ? false : true;
        switch (eItemType)
        {
            case EItemType.SortItem:
                sortItemCountText.text = $"{count} ��";
                sortButton.interactable = isRemainItem;
                break;
            case EItemType.ThrowOutItem:
                throwOutItemCountText.text = $"{count} ��";
                throwOutButton.interactable = isRemainItem;
                break;
            case EItemType.UpgradeItem:
                upgradeItemCountText.text = $"{count} ��";
                upgradeButton.interactable = isRemainItem;
                break;
        }
    }

    public void OnClickThrowAwayItem()
    {
        Debug.Log("������!");
    }
    public void OnClickSortItem()
    {
        Debug.Log("�����ϱ�!");
    }
    public void OnClickUpgradeItem()
    {
        Debug.Log("���׷��̵��ϱ�!");
    }

    public void OnClickItemButton(EItemType eItemType)
    {
        switch (eItemType)
        {
            case EItemType.SortItem:

                break;
            case EItemType.ThrowOutItem:

                break;
            case EItemType.UpgradeItem:

                break;
        }
    }

    public SwapMoney PopMoney()
    {
        if (moneyPool.Count == 0)
        {
            SwapMoney money = Instantiate(moneyPrefab, moneyParent);
            money.returnEvent = PushMoney;
            moneyPool.Push(money);
        }

        SwapMoney swapMoney = moneyPool.Pop();
        swapMoney.gameObject.SetActive(true);
        return swapMoney;
    }
    public void PushMoney(SwapMoney money)
    {
        money.gameObject.SetActive(false);
        moneyPool.Push(money);
    }
}
