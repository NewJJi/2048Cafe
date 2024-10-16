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

    public bool isRemoveMode = false;

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

        switch (eRecipeLabType)
        {
            case ERecipeLabType.Beverage:
                currentActiveRecipeLab = beverageRecipeLab;
                break;
            case ERecipeLabType.Bakery:
                currentActiveRecipeLab = bakeryRecipeLab;
                break;
            case ERecipeLabType.Desert:
                currentActiveRecipeLab = desertRecipeLab;
                break;
        }
        currentActiveRecipeLab.gameObject.SetActive(true);
    }
    public void SwapPuzzle(EMoveDirType eMoveDirType)
    {
        currentActiveRecipeLab.tileController.Move(eMoveDirType);
    }

    #region Func
    public void RemoveTile(Tile tile)
    {
        if(isRemoveMode == true)
        {
            currentActiveRecipeLab.tileController.RemoveTile(tile);
            isRemoveMode = false;
            GameManager.Instance.UseItem(EItemType.ThrowOutEvent);
        }
    }
    public void ExpandRecipeLab()
    {
        Debug.Log("Ȯ��!");
    }
    public void ThrowOutTile()
    {
        Debug.Log("������!");
        isRemoveMode = true;
    }
    public void SortRecipeLab()
    {
        Debug.Log("����!");
    }
    public void UpgradeTile()
    {
        Debug.Log("����!");
    }
    #endregion

    #region Pool
    #endregion
}
