using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 更新任务进度
/// </summary>
public struct UpdateDialogueProgress : IEvent
{
    public void OnEvent(){
        Debug.Log("更新当前对话");
    }
    public DialogueNode.DialogueLine line;
    public List<DialogueNode.DialogueOption> option;

    public UpdateDialogueProgress(DialogueNode.DialogueLine line,List<DialogueNode.DialogueOption> option=null)
    {
        this.line = line;
        this.option = option;
    }
}