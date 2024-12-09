using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class TMPTextReveal : MonoBehaviour
{
    public RectTransform textRectTransform;
    public TMP_Text tmpText;
    public float revealSpeed;
    public string text;

    public void StartShowText(Action endRevealTextEvent)
    {
        if(tmpText == null)
        {
            tmpText = this.GetComponent<TMP_Text>();
        }
        if (textRectTransform == null)
        {
            textRectTransform = this.GetComponent<RectTransform>();
        }
        tmpText.text = "";
        LayoutRebuilder.ForceRebuildLayoutImmediate(textRectTransform);

        StartCoroutine(CoStartShowText(endRevealTextEvent));
    }
    private IEnumerator CoStartShowText(Action endRevealTextEvent)
    {
        foreach (var letter in text.ToCharArray())
        {
            tmpText.text += letter;
            LayoutRebuilder.ForceRebuildLayoutImmediate(textRectTransform);
            yield return new WaitForSeconds(revealSpeed);
        }

        endRevealTextEvent?.Invoke();
    }
}
