using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour
{
    public GameObject[] foodStarArray = new GameObject[5];

    public Image foodImage;
    public TMP_Text foodName;
    public TMP_Text foodDescription;
    public Button upgradeButton;

    public void SetInfoPopup(Sprite foodSprite, string name, string description, int startCount)
    {
        foodImage.sprite = foodSprite;
        foodName.text = name;
        foodDescription.text = description;

        for (int i = 0; i < foodStarArray.Length; i++)
        {
            foodStarArray[i].SetActive(false);
        }
        for (int i = 0; i < startCount; i++)
        {
            foodStarArray[i].SetActive(true);
        }
    }
}
