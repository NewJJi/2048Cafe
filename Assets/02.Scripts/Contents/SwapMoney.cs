using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMoney : MonoBehaviour
{
    public AudioSource audioSource;
    public Action<SwapMoney> returnEvent;
    public float moneyMoveSpeed;
    public float destoryTime;
    public void Init(int money)
    {
        SetMoneyImage(money);
        //audioSource.Play();
        StartCoroutine(MoveMoney());
        Invoke(nameof(DestroyMoney), destoryTime);
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
