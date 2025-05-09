using UnityEngine;

public class DialogueQuesObj : QuestObjective, IInteractable
{
    void Awake()
    {
        RegisterQuestObjective();
    }
    public bool IsInteractable { get; set; } = true;

    public void OnDetectionEnter()
    {
        if(!isActive){
            return;
        }
        if (!IsInteractable)
        {
            return;
        }

        dialogueData ??= DialogueLoader.LoadDialogueData(dialogueName);

        if(dialogueData == null){
            Debug.LogWarning("找不到对话数据");
        }
    }

    public void OnDetectionExit()
    {
        this.interactor=null;
    }
    private DialogueData dialogueData=null;
    public string dialogueName;

    private Interactor interactor;
    public void OnInteract(Interactor interactor)
    {
        if (!(isActive && IsInteractable && Input.GetKeyDown(KeyCode.E)))
        {
            return;
        }
        this.interactor=interactor;

        DialoguePlayer.Instance.LoadDialogueData(dialogueData);

        IsInteractable = false;
        GameEventBus.Instance.Trigger(new UpdateQuestProgress(questID));
    }
}