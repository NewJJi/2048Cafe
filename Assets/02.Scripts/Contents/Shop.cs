using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;
using static Define;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;

    public Button adButton;

    public CodelessIAPButton buyUpgradeItemButton;
    public CodelessIAPButton buyThrowOutButton;
    public CodelessIAPButton buySortItemButton;
    public CodelessIAPButton buyItemPackageButton;
    public CodelessIAPButton removeAdButton;
    public GameObject removeButtonObject;

    public Button closeButton;

    public void Init()
    {
        BindEvnet();

        if (PlayerPrefs.HasKey(removeAdValue))
        {
            removeButtonObject.SetActive(false);
        }
    }

    public void BindEvnet()
    {
        closeButton.onClick.AddListener(() => { CloseShopPanel(); });

        buyUpgradeItemButton.onPurchaseComplete.AddListener(OnClickBuyUpgradeItem);
        buyThrowOutButton.onPurchaseComplete.AddListener(OnClickBuyThrowOutItem);
        buySortItemButton.onPurchaseComplete.AddListener(OnClickBuySortItemItem);
        buyItemPackageButton.onPurchaseComplete.AddListener(OnClickBuyPackageItem);
        removeAdButton.onPurchaseComplete.AddListener(OnClickRemoveAdItem);
    }

    private void OnClickRemoveAdItem(Product product)
    {
        PlayerPrefs.SetString(removeAdValue, "Buy");
        removeButtonObject.SetActive(false);
    }

    internal void ShowShopPanel()
    {
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);
        GameManager.Instance.isCanSwap = false;
        shopPanel.SetActive(true);
    }

    private void CloseShopPanel()
    {
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);
        GameManager.Instance.isCanSwap = true;
        shopPanel.SetActive(false);
    }

    public void OnClickAdButton()
    {
        GameManager.Instance.EarnMoney(100);
    }

    public void OnClickBuyThrowOutItem(Product product)
    {
        GameManager.Instance.GetItem(Define.EItemType.ThrowOutItem, 3);
    }

    public void OnClickBuySortItemItem(Product product)
    {
        GameManager.Instance.GetItem(Define.EItemType.SortItem, 2);
    }

    public void OnClickBuyUpgradeItem(Product product)
    {
        GameManager.Instance.GetItem(Define.EItemType.UpgradeItem, 2);
    }

    public void OnClickBuyPackageItem(Product product)
    {
        GameManager.Instance.GetItem(Define.EItemType.UpgradeItem, 3);
        GameManager.Instance.GetItem(Define.EItemType.SortItem, 3);
        GameManager.Instance.GetItem(Define.EItemType.ThrowOutItem, 3);
    }
}
