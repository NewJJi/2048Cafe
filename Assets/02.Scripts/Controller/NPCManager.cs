using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NPCManager : MonoBehaviour
{
    public NPC npc;

    public Action visitEvent;
    public float visitRanTime = 0;

    public void Init()
    {
        StartCoroutine(CoStartVisitCustomerCount());
    }

    public void ShowNpc()
    {
        int npcRandomNum = UnityEngine.Random.Range(0,Enum.GetValues(typeof(ENpcType)).Length);
        npc.gameObject.SetActive(true);
        npc.SetNpcInfo(npcRandomNum);
    }
    private IEnumerator CoStartVisitCustomerCount()
    {
        visitRanTime = UnityEngine.Random.Range(visitMinTime, visitMaxTime);
        while (true)
        {
            yield return new WaitForSeconds(visitRanTime);

            GameManager.Instance.Sound.PlayDoorBellSound();

            yield return new WaitForSeconds(1.0f);
            ShowNpc();
        }
    }
}
