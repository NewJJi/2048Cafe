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
    public Image beverageRecipeLabButtonImage;
    public Sprite beverageButtonActiveSprite;
    public Sprite beverageButtonDeActiveSprite;

    public Button bakeryRecipeLabButton;
    public Image bakeryRecipeLabButtonImage;
    public Sprite bakeryButtonActiveSprite;
    public Sprite bakeryButtonDeActiveSprite;

    public Button desertRecipeLabButton;
    public Image desertRecipeLabButtonImage;
    public Sprite desertButtonActiveSprite;
    public Sprite desertButtonDeActiveSprite;

    public Image buttonBottomLineImage;
    public List<Sprite> labButtonBottomAreaSprite;

    public Canvas beverageButtonCanvas;
    public Canvas bakeryButtonCanvas;
    public Canvas desertButtonCanvas;
    public Canvas bottomLineCanvas;

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

        ShowItemData(EItemType.ThrowOutItem, GameManager.Instance.Data.GetItemCount(EItemType.ThrowOutItem));
        ShowItemData(EItemType.UpgradeItem, GameManager.Instance.Data.GetItemCount(EItemType.UpgradeItem));
        ShowItemData(EItemType.SortItem, GameManager.Instance.Data.GetItemCount(EItemType.SortItem));

        ClickRecipeLabButton(ERecipeLabType.Beverage);
    }

    public void BindEvent()
    {
        GameManager.Instance.MoneyEvent += ShowHaveMoney;
        GameManager.Instance.ItemEvent += ShowItemData;

        shopOpenButton.onClick.AddListener(() => shopPanel.ShowShopPanel());

        throwOutButton.onClick.AddListener(()=>ClickItemEvent?.Invoke(EItemType.ThrowOutItem));
        upgradeButton.onClick.AddListener(() => ClickItemEvent?.Invoke(EItemType.UpgradeItem));
        sortButton.onClick.AddListener(() => ClickItemEvent?.Invoke(EItemType.SortItem));

        beverageRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabButton(ERecipeLabType.Beverage); });
        bakeryRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabButton(ERecipeLabType.Bakery); });
        desertRecipeLabButton.onClick.AddListener(() => { ClickRecipeLabButton(ERecipeLabType.Desert); });
    }

    public void ClickRecipeLabButton(ERecipeLabType eRecipeLabType)
    {
        DeSelectAllRecipeLabButton();
        switch (eRecipeLabType)
        {
            case ERecipeLabType.Beverage:
                //beverageRecipeLabButtonImage.sprite = beverageButtonActiveSprite;
                beverageButtonCanvas.sortingOrder = 4;
                bakeryButtonCanvas.sortingOrder = 2;
                desertButtonCanvas.sortingOrder = 1;

                bottomLineCanvas.sortingOrder = 3;

                beverageRecipeLabButtonImage.sprite = beverageButtonActiveSprite;

                beverageRecipeLabButtonImage.SetNativeSize();
                break;
            case ERecipeLabType.Bakery:
                //bakeryRecipeLabButtonImage.sprite = bakeryButtonActiveSprite;

                beverageButtonCanvas.sortingOrder = 1;
                bakeryButtonCanvas.sortingOrder = 3;
                desertButtonCanvas.sortingOrder = 1;
                bottomLineCanvas.sortingOrder = 2;

                bakeryRecipeLabButtonImage.sprite = bakeryButtonActiveSprite;


                bakeryRecipeLabButtonImage.SetNativeSize();
                break;
            case ERecipeLabType.Desert:
                //desertRecipeLabButtonImage.sprite = desertButtonActiveSprite;

                beverageButtonCanvas.sortingOrder = 1;
                bakeryButtonCanvas.sortingOrder = 2;
                desertButtonCanvas.sortingOrder = 4;

                bottomLineCanvas.sortingOrder = 3;

                desertRecipeLabButtonImage.sprite = desertButtonActiveSprite;
                desertRecipeLabButtonImage.SetNativeSize();
                break;
        }
        //bakeryRecipeLabButtonImage.sprite = labButtonBottomAreaSprite[(int)eRecipeLabType];
        buttonBottomLineImage.sprite = labButtonBottomAreaSprite[(int)eRecipeLabType];
        ClickRecipeLabEvent?.Invoke(eRecipeLabType);
    }

    public void DeSelectAllRecipeLabButton()
    {
        beverageRecipeLabButtonImage.sprite = beverageButtonDeActiveSprite;
        bakeryRecipeLabButtonImage.sprite = bakeryButtonDeActiveSprite;
        desertRecipeLabButtonImage.sprite = desertButtonDeActiveSprite;

        beverageRecipeLabButtonImage.SetNativeSize();
        bakeryRecipeLabButtonImage.SetNativeSize();
        desertRecipeLabButtonImage.SetNativeSize();

        beverageRecipeLabButtonImage.rectTransform.sizeDelta = GetDeActiveButtonImageSize(beverageRecipeLabButtonImage.rectTransform.sizeDelta);
        bakeryRecipeLabButtonImage.rectTransform.sizeDelta = GetDeActiveButtonImageSize(bakeryRecipeLabButtonImage.rectTransform.sizeDelta);
        desertRecipeLabButtonImage.rectTransform.sizeDelta = GetDeActiveButtonImageSize(desertRecipeLabButtonImage.rectTransform.sizeDelta);

        //beverageRecipeLabButtonImage.SetNativeSize();
        //bakeryRecipeLabButtonImage.SetNativeSize();
        //desertRecipeLabButtonImage.SetNativeSize();
    }

    public Vector2 GetDeActiveButtonImageSize(Vector2 originSize)
    {
        return new Vector2(originSize.x * 0.8f, originSize.y * 0.8f);
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
            case EItemType.SortItem:
                sortItemCountText.text = $"{count} 개";
                sortButton.interactable = isRemainItem;
                break;
            case EItemType.ThrowOutItem:
                throwOutItemCountText.text = $"{count} 개";
                throwOutButton.interactable = isRemainItem;
                break;
            case EItemType.UpgradeItem:
                upgradeItemCountText.text = $"{count} 개";
                upgradeButton.interactable = isRemainItem;
                break;
        }
    }

    public void OnClickThrowAwayItem()
    {
        Debug.Log("버리기!");
    }
    public void OnClickSortItem()
    {
        Debug.Log("정렬하기!");
    }
    public void OnClickUpgradeItem()
    {
        Debug.Log("업그레이드하기!");
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
