using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UiManager : MonoBehaviour
{
    public PopupController popupController;

    public TMP_Text moneyText;

    [Header("Shop")]
    public Button shopButton;
    public Shop shopPanel;

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

    #region Event
    //info 패널 이벤트
    public Action<ERecipeLabType, int> ShowRecipeInfoPopupEvent;

    //레시피 연구실 변경 이벤트
    public Action<ERecipeLabType> ClickRecipeLabEvent;

    //아이템 클릭 이벤트
    public Action<EItemType> ClickItemEvent;
    #endregion

    public void Init()
    {
        BindEvent();

        popupController.Init();
        shopPanel.Init();

        ShowHaveMoney(GameManager.Instance.GameMoney);

        ShowItemData(EItemType.ThrowOutEvent, GameManager.Instance.Data.GetItemCount(EItemType.ThrowOutEvent));
        ShowItemData(EItemType.UpgradeEvent, GameManager.Instance.Data.GetItemCount(EItemType.UpgradeEvent));
        ShowItemData(EItemType.SortEvent, GameManager.Instance.Data.GetItemCount(EItemType.SortEvent));
    }

    public void BindEvent()
    {
        GameManager.Instance.MoneyEvent += ShowHaveMoney;
        GameManager.Instance.ItemEvent += ShowItemData;

        shopButton.onClick.AddListener(() => { shopPanel.ShowShopPanel(); });
        beverageRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabEvent?.Invoke(ERecipeLabType.Beverage); });
        bakeryRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabEvent?.Invoke(ERecipeLabType.Bakery); });
        desertRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabEvent?.Invoke(ERecipeLabType.Desert); });
    }

    public void ShowHaveMoney(int haveMoney)
    {
        moneyText.text = $"{haveMoney} 원";
    }

    public void ShowItemData(EItemType eItemType, int count)
    {
        bool isRemainItem = count == 0 ? false : true;
        switch (eItemType)
        {
            case EItemType.SortEvent:
                sortItemCountText.text = $"X {count}";
                sortButton.gameObject.SetActive(isRemainItem);
                break;
            case EItemType.ThrowOutEvent:
                throwOutItemCountText.text = $"X {count}";
                throwOutButton.gameObject.SetActive(isRemainItem);
                break;
            case EItemType.UpgradeEvent:
                upgradeItemCountText.text = $"{count}";
                upgradeButton.gameObject.SetActive(isRemainItem);
                break;
        }
    }
}
