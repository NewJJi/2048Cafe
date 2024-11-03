using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStepPanel : MonoBehaviour
{
    public Button nextTutorialButton;

    public Action<int> clickEvent;
    public int index;

    [ContextMenu("SetUI")]
    public void SetButton()
    {
        nextTutorialButton = this.GetComponent<Button>();
    }
}
