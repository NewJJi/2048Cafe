using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class NPC : MonoBehaviour
{
    public GameObject textBox;
    public TMPTextReveal tmpTextReveal;

    public Image npcImage;

    public Action leaveEvent;

    public void SetNpcInfo(Sprite npcSprite, string conversation)
    {
        npcImage.sprite = npcSprite;
        tmpTextReveal.text = conversation;
        npcImage.SetNativeSize();
        StartCoroutine(CoShowTextBox());
    }

    IEnumerator CoShowTextBox()
    {
        yield return new WaitForSeconds(2.0f);
        textBox.gameObject.SetActive(true);
        tmpTextReveal.StartShowText(LeaveCustomer);
    }

    private void LeaveCustomer()
    {
        StartCoroutine(CoLeave());
    }
    IEnumerator CoLeave()
    {
        yield return new WaitForSeconds(7f);
        HideTextBox();
        this.gameObject.SetActive(false);
        leaveEvent?.Invoke();
    }
    public void HideTextBox()
    {
        textBox.gameObject.SetActive(false);
    }
}
