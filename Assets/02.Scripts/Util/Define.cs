using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    //총 레시피 갯수
    public const int recipeItemTotalCount = 26;

    //기본 연구소 그리드 사이즈
    public const int defaultRecipeLabGridSize = 2;

    //기본 아이템 갯수
    public const int defaultChargedItemCount = 2;

    public const float moveSpeed = 0.5f;

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
        MulledWine, //뱅쇼
        BlackSesameSeedsLatte,
        CherryBlossom,
    }

    public enum EItemType
    {
        SortEvent,
        ThrowOutEvent,
        UpgradeEvent,
    }
}
