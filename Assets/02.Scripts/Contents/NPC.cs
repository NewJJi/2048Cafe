using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

[System.Serializable]
public class NPCData
{
    public string npcName;
    public Sprite npcSprite;
    public List<string> conversationList;
}

public class NPC : MonoBehaviour
{
    public GameObject textBox;
    public RectTransform textRectTransform;
    public TMP_Text conversationText;

    public Image npcImage;
    public List<NPCData> npcData;

    Sprite currentSprite;
    string currentConversation;
    public void SetNpcInfo(int npcNum)
    {
        NPCData currentNPCData = npcData[npcNum];
        int ranConversationNum = Random.Range(0, currentNPCData.conversationList.Count);

        currentSprite = currentNPCData.npcSprite;
        currentConversation = currentNPCData.conversationList[ranConversationNum];

        ShowNpc();
    }

    public void ShowNpc()
    {
        npcImage.sprite = currentSprite;
        npcImage.SetNativeSize();
        conversationText.text = currentConversation;
        ShowTextBox();
    }


    public void ShowTextBox()
    {
        textBox.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(textRectTransform);
        StartCoroutine(CoShowTextBox());
    }
    public void HideTextBox()
    {
        textBox.gameObject.SetActive(false);
    }
    IEnumerator CoShowTextBox()
    {
        yield return new WaitForSeconds(2.0f);
        CoLeave();
    }

    IEnumerator CoLeave()
    {
        yield return new WaitForSeconds(10f);
        HideTextBox();
        this.gameObject.SetActive(false);
    }
}
