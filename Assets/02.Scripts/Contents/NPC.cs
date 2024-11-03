using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject textBox;
    public TMP_Text tMP_Text;
    public int tipMoney;
    public void ShowTextBox()
    {
        StartCoroutine(CoShowTextBox());
    }
    public void HideTextBox()
    {
        textBox.gameObject.SetActive(false);
    }
    IEnumerator CoShowTextBox()
    {
        yield return new WaitForSeconds(2.0f);
        textBox.gameObject.SetActive(true);
    }
}
