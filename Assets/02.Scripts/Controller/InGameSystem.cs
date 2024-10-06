using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class InGameSystem : MonoBehaviour
{
    public static InGameSystem Instance;

    public InGameUiController inGameUiController;
    public RecipeLabController recipeLabController;
    public InputController inputController;

    public Action<int> MoneyEvent;

    public Action ItemEvent;

    public SaveData saveData;

    public int GameMoney
    {
        get => saveData.haveMoney;
        set
        {
            saveData.haveMoney += value;
            MoneyEvent?.Invoke(saveData.haveMoney);
        }
    }

    #region Global Event


    #endregion

    public void Awake()
    {
        Instance = this;
        DataManager.Instance.InitSaveData();
        saveData = DataManager.Instance.saveData;
        Init();
    }

    public void Init()
    {
        inGameUiController.Init();
        recipeLabController.Init();
        inputController.swapEvent = recipeLabController.SwapPuzzle;
    }
}
