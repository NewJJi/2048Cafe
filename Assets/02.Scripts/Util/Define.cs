using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    //�� ������ ����
    public const int recipeItemTotalCount = 26;

    //�⺻ ������ �׸��� ������
    public const int defaultRecipeLabGridSize = 2;

    //�⺻ ������ ����
    public const int defaultChargedItemCount = 2;

    public const float moveSpeed = 0.5f;

    public enum EMoveDirType
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum ESaveType { }

    public enum EPuzzleType
    {
        Beverage,
        Caffee,
        Desert,
    }

    public enum ERecipeLabType
    {
        Beverage,
        Bakery,
        Desert,
    }

    public enum ESaveDataType
    {
        BeverageData,
        CaffeeData,
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

    public enum EEventType
    {
        SortEvent,
        RemoveEvent,
        ExpandEvent,
        UpgradeEvent,
    }

    public enum ERecipeType
    {
        Beverage,
        Bakery,
        Desert
    }
}