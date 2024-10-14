using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;

    public Button adButton;

    public Button buyExpandItemButton;
    public Button buyThrowOutButton;
    public Button buySortItemButton;

    public Button buyItemPackageButton;

    public Button closeButton;

    GoogleMobileAdsController googleMobileAdsController;
    public void Init()
    {
        googleMobileAdsController = new GoogleMobileAdsController();
        googleMobileAdsController.Init();

        BindEvnet();
    }

    public void BindEvnet()
    {
        adButton.onClick.AddListener(() =>
        {
            googleMobileAdsController.ShowRewardedAd(() =>
            {
                OnClickAdButton();
            });
        });

        closeButton.onClick.AddListener(() => { CloseShopPanel(); });
    }

    internal void ShowShopPanel()
    {
        shopPanel.SetActive(true);
    }

    private void CloseShopPanel()
    {
        shopPanel.SetActive(false);
    }

    public void OnClickAdButton()
    {
        Debug.Log("µ· ¹Þ¾Ò´Ù!!");
    }

    public void OnClickBuyExpandItem()
    {

    }

    public void OnClickBuyThrowOutItem()
    {

    }

    public void OnClickBuySortItemItem()
    {

    }

    public void OnClickBuyPackageItem()
    {

    }
}
