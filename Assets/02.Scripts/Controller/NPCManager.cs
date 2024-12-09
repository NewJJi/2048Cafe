using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class NPCData
{
    public string npcName;
    public Sprite npcSprite;
    public List<string> conversationList;
}

public class NPCManager : MonoBehaviour
{
    public NPC npc;
    public List<NPCData> npcDataList;

    private float visitRanTime = 0;
    public void Init()
    {
        npc.leaveEvent = SetNextVisitTime;
        StartCoroutine(CoStartVisitCustomerCount());
    }

    public void SetNextVisitTime()
    {
        visitRanTime = UnityEngine.Random.Range(visitMinTime, visitMaxTime);
    }

    private IEnumerator CoStartVisitCustomerCount()
    {
        SetNextVisitTime();
        while (true)
        {
            yield return new WaitForSeconds(visitRanTime);

            GameManager.Instance.Sound.PlayDoorBellSound();

            yield return new WaitForSeconds(1.0f);
            ShowNpc();
        }
    }
    public void ShowNpc()
    {
        int npcRandomNum = UnityEngine.Random.Range(0,Enum.GetValues(typeof(ENpcType)).Length);
        NPCData npcData = npcDataList[npcRandomNum];

        int conversationRanNum = UnityEngine.Random.Range(0,npcData.conversationList.Count);

        npc.gameObject.SetActive(true);
        npc.SetNpcInfo(npcData.npcSprite, npcData.conversationList[conversationRanNum]);
    }
}
