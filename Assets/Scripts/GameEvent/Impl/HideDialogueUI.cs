using UnityEngine;
public struct HideDialogueUI : IEvent
{
    public void OnEvent()
    {
        Debug.Log("隐藏对话UI");
    }
}