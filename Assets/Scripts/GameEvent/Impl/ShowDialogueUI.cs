using UnityEngine;
public struct ShowDialogueUI : IEvent
{
    public void OnEvent()
    {
        Debug.Log("显示对话UI");
    }
}