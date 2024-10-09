using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwapMoney : MonoBehaviour
{
    public TMP_Text moneyText;
    public AudioSource audioSource;
    public Action<SwapMoney> returnEvent;
    public float moneyMoveSpeed;
    public float destoryTime;

    RectTransform rectTransform;

    public void Init(int money, float size)
    {
        SetMoneyImage(money);
        //audioSource.Play();
        StartCoroutine(MoveMoney());
        Invoke(nameof(DestroyMoney), destoryTime);

        if (rectTransform == null)
        {
            rectTransform = this.GetComponent<RectTransform>();
        }
        rectTransform.sizeDelta = new Vector2(size, size);
        moneyText.text = $"+${money}";
    }
    
    public void SetMoneyImage(int money)
    {

    }

    private IEnumerator MoveMoney()
    {
        while (true)
        {
            yield return null;
            this.transform.localPosition += Vector3.up * Time.deltaTime * moneyMoveSpeed;
        }
    }

    private void DestroyMoney()
    {
        StopCoroutine(MoveMoney());
        returnEvent?.Invoke(this);
    }
}
