using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class InfoPopup : MonoBehaviour
{
    public GameObject starParent;
    public GameObject[] foodStarArray = new GameObject[5];

    public Image foodImage;

    public TMP_Text foodName;
    public TMP_Text foodDescription;
    public TMP_Text addMoneyValue;

    public TMP_Text costText;
    public Button upgradeButton;
    public CanvasGroup upgradeCanvasGroup;

    public Button backPanelButton;

    public Button preButton;
    public Button nextButton;

    public Image preButtonImage;
    public Image nextButtonImage;

    public Sprite preActiveSprite;
    public Sprite preDeActiveSprite;

    public Sprite nextActiveSprite;
    public Sprite nextDeActiveSprite;

    int upgradeCost = 0;

    ERecipeLabType recipeType;
    int recipeIndex;
    private void Start()
    {
        backPanelButton.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
            GameManager.Instance.isCanSwap = true;
            GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);
        });

        upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        GameManager.Instance.levelUpEvent += ShowRecipeItem;

        preButton.onClick.AddListener(OnClickPreButton);
        nextButton.onClick.AddListener(OnClickNextButton);
    }

    public void SetInfoPopup(ERecipeLabType eRecipeType, int index)
    {
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);

        GameManager.Instance.isCanSwap = false;

        recipeType = eRecipeType;
        this.recipeIndex = index;

        RecipeLabSaveData recipeLabSaveData = GameManager.Instance.GetRecipeLabData(eRecipeType);
        RecipeItemData recipeItemData = recipeLabSaveData.recipeItemDatas[index];
        recipeItemData.isUnlock = true;

        var data = GameManager.Instance.Data;

        foodImage.sprite = data.GetFoodSprite(eRecipeType, index);
        foodName.text = data.GetFoodInfo(eRecipeType, index).name;

        if(index == 26)
        {
            foodName.text = data.GetFoodInfo(eRecipeType, index).name + "(마지막)";
        }

        addMoneyValue.text = $"+{(recipeItemData.level * 0.2) * 100}%";
        foodDescription.text = data.GetFoodInfo(eRecipeType, index).description;

        int level = recipeItemData.level + 1;
        upgradeCost = (int)(tileDefaultUpgradeCost * Mathf.Pow(index + 1, 2) * level * level);

        costText.text = $"$ {NumberTranslator.FormatNumber(upgradeCost)}";

        upgradeButton.interactable = true;
        upgradeCanvasGroup.alpha = 1;

        if (upgradeCost > GameManager.Instance.GameMoney)
        {
            upgradeButton.interactable = false;
            upgradeCanvasGroup.alpha = 0.8f;
        }

        if (recipeItemData.level == 5)
        {
            costText.text = "Max";
            upgradeButton.interactable = false;
            upgradeCanvasGroup.alpha = 0.8f;
        }

        ShowRecipeItem(eRecipeType, index);

        ShowActiveButton();
    }

    public void OnClickUpgradeButton()
    {
        Debug.Log("업그레이드!!");
        GameManager.Instance.SpendMoney(upgradeCost);
        GameManager.Instance.LevelUpRecipe(recipeType, recipeIndex);
        GameManager.Instance.Sound.PlayEffectSound(EEffectSoundType.Button);
        RecipeLabSaveData recipeLabSaveData = GameManager.Instance.GetRecipeLabData(recipeType);
        RecipeItemData recipeItemData = recipeLabSaveData.recipeItemDatas[recipeIndex];

        // 50  2*2 3*3 4*4 5*5  4 9 16 25
        int index = recipeIndex + 1;
        int level = recipeItemData.level + 1;

        upgradeCost = (int)(tileDefaultUpgradeCost * Mathf.Pow(index + 1, 2) * level * level);

        //upgradeCost = GameManager.Instance.Data.GetFoodInfo(recipeType, recipeIndex).starCost * (recipeItemData.level + 1);
        costText.text = $"$ {NumberTranslator.FormatNumber(upgradeCost)}";
        addMoneyValue.text = $"+{(recipeItemData.level * 0.2) * 100}%";
        if (upgradeCost > GameManager.Instance.GameMoney)
        {
            upgradeButton.interactable = false;
            upgradeCanvasGroup.alpha = 0.8f;
        }
        if (recipeItemData.level == 5)
        {
            costText.text = "Max";
            upgradeButton.interactable = false;
            upgradeCanvasGroup.alpha = 0.8f;
        }
    }

    public void ShowRecipeItem(ERecipeLabType recipeType, int index)
    {
        int level = GameManager.Instance.GetRecipeLabData(recipeType).recipeItemDatas[index].level;

        if(level == 0)
        {
            starParent.SetActive(false);
        }
        else
        {
            starParent.SetActive(true);
        }

        for (int i = 0; i < foodStarArray.Length; i++)
        {
            foodStarArray[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < level; i++)
        {
            foodStarArray[i].gameObject.SetActive(true);
        }
    }

    public void OnClickNextButton()
    {
        recipeIndex++;
        SetInfoPopup(recipeType,recipeIndex);
        ShowActiveButton();
    }
    public void OnClickPreButton()
    {
        recipeIndex--;
        SetInfoPopup(recipeType, recipeIndex);
        ShowActiveButton();
    }

    public void ShowActiveButton()
    {
        RecipeLabSaveData recipeLabSaveData = GameManager.Instance.GetRecipeLabData(recipeType);

        if (recipeIndex == 0)
        {
            preButton.enabled = false;
            preButtonImage.sprite = preDeActiveSprite;

            nextButton.enabled = true;
            nextButtonImage.sprite = nextActiveSprite;

            if (recipeIndex == Mathf.Log(recipeLabSaveData.maxValue,2)-1)
            {
                nextButton.enabled = false;
                nextButtonImage.sprite = nextDeActiveSprite;
            }
        }
        else if(recipeIndex == Mathf.Log(recipeLabSaveData.maxValue,2)-1)
        {
            nextButton.enabled = false;
            nextButtonImage.sprite = nextDeActiveSprite;

            preButton.enabled = true;
            preButtonImage.sprite = preActiveSprite;
        }
        else
        {
            nextButton.enabled = true;
            nextButtonImage.sprite = nextActiveSprite;

            preButton.enabled = true;
            preButtonImage.sprite = preActiveSprite;
        }
    }
}
