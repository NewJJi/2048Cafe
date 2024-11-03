using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NPCManager : MonoBehaviour
{
    public NPC npc;
    public int randomNextCustomerVisitSwap;

    public Action visitEvent;

    public void Init()
    {
        SetRandomNextSwap();
    }

    private void SetRandomNextSwap()
    {
        randomNextCustomerVisitSwap = UnityEngine.Random.Range(nextCustomerMinSwapCount, nextCustomerMaxSwapCount+1);
    }
    public void CountUpSwap()
    {
        randomNextCustomerVisitSwap--;

        if (randomNextCustomerVisitSwap <= 0)
        {
            SetRandomNextSwap();
            GameManager.Instance.Sound.PlayDoorBellSound();
            visitEvent?.Invoke();
        }
    }
}
