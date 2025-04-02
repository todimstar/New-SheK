using UnityEngine;
public class Pickable_questobject : QuestObjective, IInteractable
{
    public bool IsInteractable { get; set; } = true;

    public void OnDetectionEnter()
    {
        IsInteractable = isActive;
        Debug.Log("按 E 可以捡起");
    }
    public void OnDetectionExit() { }

    private void Start()
    {
        RegisterQuestObjective();
    }

    public void OnInteract(Interactor interactor)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameEventBus.Instance.Trigger(new UpdateQuestProgress(this.questID));
            IsInteractable=false;
            Destroy(gameObject);
        }
    }
}