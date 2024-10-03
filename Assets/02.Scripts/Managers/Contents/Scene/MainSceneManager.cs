using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainSceneManager : SceneBase
{
    public TMP_Text goldText;

    public Action<int> earnMoney;
    public Action<int> useMoney;
}
