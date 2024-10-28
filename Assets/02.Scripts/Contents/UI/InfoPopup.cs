using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class InfoPopup : MonoBehaviour
{
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
            GameManager.Instance.IsCanSwap = true;
        });

        upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        GameManager.Instance.levelUpEvent += ShowRecipeItem;

        preButton.onClick.AddListener(OnClickPreButton);
        nextButton.onClick.AddListener(OnClickNextButton);
    }

    public void SetInfoPopup(ERecipeLabType eRecipeType, int index)
    {
        GameManager.Instance.IsCanSwap = false;

        recipeType = eRecipeType;
        this.recipeIndex = index;

        RecipeLabSaveData recipeLabSaveData = GameManager.Instance.GetRecipeLabData(eRecipeType);
        RecipeItemData recipeItemData = recipeLabSaveData.recipeItemDatas[index];
        recipeItemData.isUnlock = true;

        var data = GameManager.Instance.Data;

        foodImage.sprite = data.GetFoodSprite(eRecipeType, index);
        foodName.text = data.GetFoodInfo(eRecipeType, index).name;
        addMoneyValue.text = $"+{(recipeItemData.level * 0.2) * 100}%";
        foodDescription.text = data.GetFoodInfo(eRecipeType, index).description;

        upgradeCost = GameManager.Instance.Data.GetFoodInfo(eRecipeType, index).starCost * (recipeItemData.level + 1);
        costText.text = $"$ {upgradeCost}";

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

        for (int i = 0; i < foodStarArray.Length; i++)
        {
            foodStarArray[i].SetActive(false);
        }
        for (int i = 0; i < recipeItemData.level; i++)
        {
            foodStarArray[i].SetActive(true);
        }

        ShowActiveButton();
    }

    public void OnClickUpgradeButton()
    {
        Debug.Log("업그레이드!!");
        GameManager.Instance.SpendMoney(upgradeCost);
        GameManager.Instance.LevelUpRecipe(recipeType, recipeIndex);
        if (upgradeCost > GameManager.Instance.GameMoney)
        {
            upgradeButton.enabled = false;
            upgradeCanvasGroup.alpha = 0.8f;
        }
        RecipeLabSaveData recipeLabSaveData = GameManager.Instance.GetRecipeLabData(recipeType);
        RecipeItemData recipeItemData = recipeLabSaveData.recipeItemDatas[recipeIndex];
        upgradeCost = GameManager.Instance.Data.GetFoodInfo(recipeType, recipeIndex).starCost * (recipeItemData.level + 1);
        costText.text = $"$ {upgradeCost}";
        addMoneyValue.text = $"+{(recipeItemData.level * 0.2) * 100}%";
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

        //0일 때

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
