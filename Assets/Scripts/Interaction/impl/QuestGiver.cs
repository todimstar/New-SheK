using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteractable
{
    public bool IsInteractable { get; set; }=true;

    public void OnDetectionEnter(){
        Debug.Log("可以按 E 接受任务");
    }

    public void OnDetectionExit(){}

    public QuestData quest;

    public void OnInteract(Interactor interactor)
    {
        if(Input.GetKeyDown(KeyCode.E)){
            QuestManager.Instance.AcceptQuest(quest);
            IsInteractable=false;
        }
    }
}