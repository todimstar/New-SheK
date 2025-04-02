using System;
using UnityEngine;

public class DialogueStarter : MonoBehaviour, IInteractable
{
    public bool IsInteractable { get; set; } = true;

    public void OnDetectionEnter()
    {
        if (!IsInteractable)
        {
            return;
        }
        dialogueData ??= DialogueLoader.LoadDialogueData(dialogueName);

        Debug.Log("按 E 可以开始对话");
    }

    public void OnDetectionExit()
    {
    }

    public void OnInteract(Interactor interactor)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("对话开始");
            DialoguePlayer.Instance.LoadDialogueData(dialogueData);
            IsInteractable=false;
            return;
        }
    }

    private DialogueData dialogueData;

    public string dialogueName;


}