using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class RecipeLabController : MonoBehaviour
{
    public Button beverageRecipeLabButton;
    public Button bakeryRecipeLabButton;
    public Button desertRecipeLabButton;

    public Button throwOutButton;
    public TMP_Text throwOutItemCountText;

    public Button expandButton;
    public TMP_Text expandItemCountText;

    public Button needleButton;
    public TMP_Text needleItemCountText;

    public Button sortButton;
    public TMP_Text sortItemCountText;

    public RecipeLab beverageRecipeLab;
    public RecipeLab bakeryRecipeLab;
    public RecipeLab desertRecipeLab;

    public RecipeLab currentActiveRecipeLab;

    public bool isRemoveMode = false;
    public void Init()
    {
        BindEvent();
        InitData();
        OnClickSwitchRecipeLab(ERecipeType.Beverage);
    }

    private void InitData()
    {
        ReInitItemUi();

        beverageRecipeLab.Init();
        bakeryRecipeLab.Init();
        desertRecipeLab.Init();
    }

    public void BindEvent()
    {
        InGameSystem.Instance.ItemEvent = ReInitItemUi;

        beverageRecipeLabButton.onClick.AddListener(() => OnClickSwitchRecipeLab(ERecipeType.Beverage));
        bakeryRecipeLabButton.onClick.AddListener(() => OnClickSwitchRecipeLab(ERecipeType.Bakery));
        desertRecipeLabButton.onClick.AddListener(() => OnClickSwitchRecipeLab(ERecipeType.Desert));

        throwOutButton.onClick.AddListener(() => OnClickThrowOutButton());
        needleButton.onClick.AddListener(() => OnClickUseNeedleButton());
        expandButton.onClick.AddListener(() => OnClickExpandButton());
        sortButton.onClick.AddListener(() => OnClickSortButton());
    }

    public void OnClickSwitchRecipeLab(ERecipeType eRecipeType)
    {
        if(currentActiveRecipeLab != null)
        {
            currentActiveRecipeLab.gameObject.SetActive(false);
        }

        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                currentActiveRecipeLab = beverageRecipeLab;
                break;
            case ERecipeType.Bakery:
                currentActiveRecipeLab = bakeryRecipeLab;
                break;
            case ERecipeType.Desert:
                currentActiveRecipeLab = desertRecipeLab;
                break;
        }
        currentActiveRecipeLab.gameObject.SetActive(true);
    }

    public void SwapPuzzle(EMoveDirType eMoveDirType)
    {
        Debug.Log(eMoveDirType);
        currentActiveRecipeLab.tileController.Move(eMoveDirType);
    }

    public void RemoveTile(Tile tile)
    {
        if(isRemoveMode == true)
        {
            currentActiveRecipeLab.tileController.RemoveTile(tile);
            isRemoveMode = false;
            InGameSystem.Instance.ManageWealthData(EWealthType.ThrowOut,-1);
        }
    }

    public void OnClickExpandButton()
    {
        Debug.Log("확장!");
    }

    public void OnClickUseNeedleButton()
    {
        Debug.Log("바늘!");
    }

    public void OnClickThrowOutButton()
    {
        Debug.Log("버리기!");
        isRemoveMode = true;
    }

    public void OnClickSortButton()
    {
        Debug.Log("정렬!");
    }
    public void ReInitItemUi()
    {
        int throwOutItem = InGameSystem.Instance.wealthSaveData.throwOutItemCount;
        int expandItem = InGameSystem.Instance.wealthSaveData.expandItemCount;
        int neeldeItem = InGameSystem.Instance.wealthSaveData.needleItemCount;
        int sortItem = InGameSystem.Instance.wealthSaveData.sortItemCount;

        throwOutItemCountText.text = $"X {throwOutItem}";
        expandItemCountText.text = $"X {expandItem}";
        needleItemCountText.text = $"X {neeldeItem}";
        sortItemCountText.text = $"X {sortItem}";

        throwOutButton.gameObject.SetActive(throwOutItem==0 ? false : true);
        needleButton.gameObject.SetActive(neeldeItem == 0 ? false : true);
        expandButton.gameObject.SetActive(expandItem == 0 ? false : true);
        sortButton.gameObject.SetActive(sortItem == 0 ? false : true);
    }

}
