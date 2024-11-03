using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class TutorialController : MonoBehaviour
{
    public GameObject tutorialPanel;

    public List<TutorialStepPanel> tutorialStepPanelList;
    public TutorialStepPanel currentTutorialPanel;

    public void Init()
    {
        BindInit();

        tutorialPanel.SetActive(true);

        currentTutorialPanel = tutorialStepPanelList[0];
        currentTutorialPanel.gameObject.SetActive(true);
    }
    public void BindInit()
    {
        for (int i = 0; i < tutorialStepPanelList.Count; i++)
        {
            Debug.Log(i);
            int index = i;
            tutorialStepPanelList[i].nextTutorialButton.onClick.AddListener(()=>ShowTutorialPanel(index));
        }
    }
    public void ShowTutorialPanel(int index)
    {
        int nextIndex = index+1;
        if(nextIndex == tutorialStepPanelList.Count)
        {
            tutorialPanel.SetActive(false);
            return;
        }

        currentTutorialPanel.gameObject.SetActive(false);
        currentTutorialPanel = tutorialStepPanelList[nextIndex];
        currentTutorialPanel.gameObject.SetActive(true);
    }

    [ContextMenu("Delete Tutorial Save")]
    public void DeleteFinishTutorial()
    {
        PlayerPrefs.DeleteKey(finishTutorialBoolKey);
    }
}
