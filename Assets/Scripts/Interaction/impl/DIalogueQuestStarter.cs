using UnityEngine;

public class DialogueQuestStarter : MonoBehaviour, IInteractable
{
    public bool IsInteractable { get; set; } = true;

    public QuestData quest;

    public void OnDetectionEnter()
    {
        if (!IsInteractable)
        {
            return;
        }
    }

    public void OnDetectionExit()
    {
    }

    public void OnInteract(Interactor interactor)
    {
        if (!(IsInteractable && Input.GetKeyDown(KeyCode.E)))
        {
            return;
        }

        if (QuestManager.Instance.AcceptQuest(quest))
        {
            IsInteractable = false;
        }
    }
}