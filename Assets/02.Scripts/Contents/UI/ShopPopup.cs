using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : MonoBehaviour
{
    public Button cancelButton;
    public Button watchButton;

    public Image buyItemImage;

    public Action watchAdEvent;
    public Action cancelEvent;
    public void Init()
    {
        cancelButton.onClick.AddListener(()=>cancelEvent?.Invoke());
        watchButton.onClick.AddListener(()=>watchAdEvent?.Invoke());
    }

    public void SetAdEventPopup()
    {

    }
}
