using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;

    public Button adButton;

    public CodelessIAPButton buyUpgradeItemButton;
    public CodelessIAPButton buyThrowOutButton;
    public CodelessIAPButton buySortItemButton;
    public CodelessIAPButton buyItemPackageButton;

    public Button closeButton;

    public void Init()
    {
        BindEvnet();
    }

    public void BindEvnet()
    {
        closeButton.onClick.AddListener(() => { CloseShopPanel(); });

        buyUpgradeItemButton.onPurchaseComplete.AddListener(OnClickBuyUpgradeItem);
        buyThrowOutButton.onPurchaseComplete.AddListener(OnClickBuyThrowOutItem);
        buySortItemButton.onPurchaseComplete.AddListener(OnClickBuySortItemItem);
        buyItemPackageButton.onPurchaseComplete.AddListener(OnClickBuyPackageItem);
    }

    internal void ShowShopPanel()
    {
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);
        GameManager.Instance.IsCanSwap = false;
        shopPanel.SetActive(true);
    }

    private void CloseShopPanel()
    {
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);
        GameManager.Instance.IsCanSwap = true;
        shopPanel.SetActive(false);
    }

    public void OnClickAdButton()
    {
        Debug.Log("����� �� �޾Ҵ�!!");
        GameManager.Instance.EarnMoney(100);
    }

    public void OnClickBuyThrowOutItem(Product product)
    {
        Debug.Log("������ ������ ���!!");
        GameManager.Instance.GetItem(Define.EItemType.ThrowOutItem, 3);
    }

    public void OnClickBuySortItemItem(Product product)
    {
        Debug.Log("���� ������ ���!!");
        GameManager.Instance.GetItem(Define.EItemType.SortItem, 2);
    }

    public void OnClickBuyUpgradeItem(Product product)
    {
        Debug.Log("���׷��̵� ������ ���!!");
        GameManager.Instance.GetItem(Define.EItemType.UpgradeItem, 2);
    }

    public void OnClickBuyPackageItem(Product product)
    {
        Debug.Log("��Ű�� �÷��� �غη���!!");
        GameManager.Instance.GetItem(Define.EItemType.UpgradeItem, 2);
        GameManager.Instance.GetItem(Define.EItemType.SortItem, 2);
        GameManager.Instance.GetItem(Define.EItemType.ThrowOutItem, 3);
    }
}
