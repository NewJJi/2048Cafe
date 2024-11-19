using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class RecipeLabController : MonoBehaviour
{
    public RecipeLab beverageRecipeLab;
    public RecipeLab bakeryRecipeLab;
    public RecipeLab desertRecipeLab;

    public RecipeLab currentActiveRecipeLab;

    public Action<bool,Transform,Transform> activeTileMaskUiEvent;

    public ETileClickEventType eTileEventType = ETileClickEventType.None;

    public SwapMoney moneyPrefab;
    private Stack<SwapMoney> moneyPool = new Stack<SwapMoney>();
    private Transform moneyParent;

    public Image recipeLabBackGround;
    public Color beverageColor;
    public Color bakeryColor;
    public Color desertColor;
    public void Init()
    {
        InitData();
        SwitchRecipeLab(ERecipeLabType.Beverage);
    }
    private void InitData()
    {
        beverageRecipeLab.Init();
        bakeryRecipeLab.Init();
        desertRecipeLab.Init();
    }

    public void SwitchRecipeLab(ERecipeLabType eRecipeLabType)
    {
        if(currentActiveRecipeLab != null)
        {
            currentActiveRecipeLab.gameObject.SetActive(false);
        }

        Color tempColor = Color.white;
        switch (eRecipeLabType)
        {
            case ERecipeLabType.Beverage:
                tempColor = beverageColor;
                currentActiveRecipeLab = beverageRecipeLab;
                break;
            case ERecipeLabType.Bakery:
                tempColor = bakeryColor;
                currentActiveRecipeLab = bakeryRecipeLab;
                break;
            case ERecipeLabType.Desert:
                tempColor = desertColor;
                currentActiveRecipeLab = desertRecipeLab;
                break;
        }

        recipeLabBackGround.color = tempColor;
        currentActiveRecipeLab.gameObject.SetActive(true);
    }
    public void SwapPuzzle(EMoveDirType eMoveDirType)
    {
        currentActiveRecipeLab.tileController.Move(eMoveDirType);
    }

    #region Func

    public void UseItem(EItemType eItemType)
    {
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.ItemButton);
        switch (eItemType)
        {
            case EItemType.SortItem:
                SortRecipeLab();
                break;
            case EItemType.ThrowOutItem:
                eTileEventType = ETileClickEventType.Remove;
                activeTileMaskUiEvent?.Invoke(true,currentActiveRecipeLab.tileController.poolParent.transform, currentActiveRecipeLab.tileController.parentTransform);
                break;
            case EItemType.UpgradeItem:
                eTileEventType = ETileClickEventType.Upgrade;
                activeTileMaskUiEvent?.Invoke(true, currentActiveRecipeLab.tileController.poolParent.transform, currentActiveRecipeLab.tileController.parentTransform);
                break;
        }
    }

    public void CancelItemButtonEvent()
    {
        eTileEventType = ETileClickEventType.None;
        activeTileMaskUiEvent?.Invoke(false, currentActiveRecipeLab.tileController.poolParent.transform, currentActiveRecipeLab.tileController.parentTransform);
    }

    public void ClickTileEvent(Tile tile)
    {
        if(tile == null)
        {
            if (eTileEventType!=ETileClickEventType.None)
            {
                CancelItemButtonEvent();
                return;
            }
        }

        switch (eTileEventType)
        {
            case ETileClickEventType.None:
                Debug.Log("아무일 없기!");
                break;
            case ETileClickEventType.Remove:
                RemoveTile(tile);
                Debug.Log("음식 갖다 버리기!");
                break;
            case ETileClickEventType.Upgrade:
                UpgradeTile(tile);
                Debug.Log("레시피 업그레이드 하기!");
                break;
        }
    }
    public void RemoveTile(Tile tile)
    {
        currentActiveRecipeLab.tileController.RemoveTile(tile);
        GameManager.Instance.UseItem(EItemType.ThrowOutItem);
        activeTileMaskUiEvent?.Invoke(false, currentActiveRecipeLab.tileController.poolParent.transform, currentActiveRecipeLab.tileController.parentTransform);
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);
        eTileEventType = ETileClickEventType.None;
    }
    public void UpgradeTile(Tile tile)
    {
        bool isFinishUpgrade = currentActiveRecipeLab.tileController.UpgradeTile(tile);

        if (isFinishUpgrade == true)
        {
            GameManager.Instance.UseItem(EItemType.UpgradeItem);
            GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);
            activeTileMaskUiEvent?.Invoke(false, currentActiveRecipeLab.tileController.poolParent.transform, currentActiveRecipeLab.tileController.parentTransform);
            eTileEventType = ETileClickEventType.None;
        }
    }
    public void SortRecipeLab()
    {
        currentActiveRecipeLab.SortPuzzle();
        GameManager.Instance.UseItem(EItemType.SortItem);
        Debug.Log("정렬!");
    }

    #endregion

    #region Pool
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

    #endregion
}
