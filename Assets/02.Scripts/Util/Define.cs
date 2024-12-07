using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    //�� ������ ����
    public const int recipeItemTotalCount = 27;

    //�⺻ ������ �׸��� ������
    public const int defaultRecipeLabGridSize = 2;

    //�⺻ ������ ����
    public const int defaultChargedItemCount = 2;

    public const float moveSpeed = 0.3f;

    public const int SwapAreaLayer = 7;
    public const int TileLayer = 8;

    public const string soundSaveVolumn = "SoundVolumn";
    public const string soundOnOffKey = "soundOnOffKey";

    public const string removeAdValue = "RemoveAd";

    public const string finishTutorialBoolKey = "isFinishTutorial";

    public const int nextCustomerMinSwapCount = 40;
    public const int nextCustomerMaxSwapCount = 60;

    public const float visitMinTime = 3;
    public const float visitMaxTime = 5;

    public const int tileDefaultUpgradeCost = 50;
    public const int expandDefaultUpgradeCost = 20;

    public enum EMoveDirType
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum ERecipeLabType
    {
        Beverage,
        Bakery,
        Desert,
    }

    public enum EBeverageData
    {
        Espresso,
        Americano,
        CaffeLatte,
        CaffeMocha,
        CaramelMacchiato,
        ColdBrew,

        GrapefruitAde,
        LemonAde,
        BlueberryAde,
        TropicalAde,
        BlueLemonAde,
        MelonAde,

        JavaChipFrappuccino,
        StrawberryFrappuccino,
        MangoFrappuccino,
        GreenTeaFrappuccino,
        OreoFrappuccino,

        TomatoJuice,
        WaterMelonJuice,
        KiwifruitJuice,
        BlueberryJuice,
        BananaChocolateJuice,

        TaroMilkTea,
        Mojito,
        MulledWine, //���
        BlackSesameSeedsLatte,
        CherryBlossom,
    }

    public enum EItemType
    {
        SortItem,
        ThrowOutItem,
        UpgradeItem,
    }

    public enum ETileClickEventType
    {
        None,
        Remove,
        Upgrade
    }

    public enum EPopupType
    {
        InfoPopup,
        StuckPopup
    }

    public enum ENpcType
    {
        Woman,
        Worker,
        ManStudent,
        WomanStudent,
        GrandFather
    }
}
