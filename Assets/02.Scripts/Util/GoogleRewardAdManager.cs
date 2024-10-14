using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoogleRewardAdManager : MonoBehaviour
{
    public GoogleMobileAdsController googleMobileAdsDemo;
    public Button adButton;
    public GameObject rewardObj;

    public TMP_Text goldText;
    public int gold;
    public void Start()
    {
        //closeButton.onClick.AddListener(googleMobileAdsDemo.ShowRewardedAd((value)=> 
        //{
        //    gold += 100;
        //    goldText.text = gold.ToString();
        //}));


        adButton.onClick.AddListener(()=> { googleMobileAdsDemo.ShowRewardedAd((value) =>{ gold += value; goldText.text = gold.ToString(); }); });
    }
}