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

    public Action<int> MoneyEvent;

    public Action ItemEvent;

    public SaveData saveData;

    #region Global Event
    public void GetMoney(int earnMoney)
    {
        saveData.haveMoney += earnMoney;
        MoneyEvent?.Invoke(saveData.haveMoney);
    }
    public void UseMoney(int useMoney)
    {
        saveData.haveMoney -= useMoney;
        if(saveData.haveMoney <= 0)
        {
            saveData.haveMoney = 0;
        }
        MoneyEvent?.Invoke(saveData.haveMoney);
    }

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
    }
}
