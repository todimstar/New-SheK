using System.Collections.Generic;
using System.IO;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    private static DialoguePlayer _instance;
    public static DialoguePlayer Instance => _instance ??= FindObjectOfType<DialoguePlayer>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            // 切换场景时，不用删除
            DontDestroyOnLoad(gameObject);
        }
        GameEventBus.Instance.Subscribe<NextDialogueProgress>(
            NextDialogueProgressHandler
        );
        GameEventBus.Instance.Subscribe<ChooseDialogueOption>(
            ChooseDialogueOptionHandle
        );
    }

    private void NextDialogueProgressHandler(NextDialogueProgress e)
    {
        NextDialogue();
    }
    private void ChooseDialogueOptionHandle(ChooseDialogueOption e)
    {
        UpdateDialogue(e.optionIndex);
    }

    private DialogueData currentDialogueData;

    public void LoadDialogueData(DialogueData dialogueData)
    {
        currentDialogueData = dialogueData;
        // 下面的NextDialogue应该在ui的show函数调用一次的，这里只是用来测试展示
        StartDialogueNode(dialogueData.entryNode);
    }

    private DialogueNode currentNode;
    private int currentLineIndex = -1;

    public DialogueNode.DialogueLine line => currentNode.lines[currentLineIndex];
    public DialogueNode.DialogueOption option => currentNode.options[currentLineIndex];

    private void StartDialogueNode(DialogueNode node)
    {
        if (node == null)
        {
            GameEventBus.Instance.Trigger(new HideDialogueUI());
        }
        else
        {
            //----此处修改过源代码
            // 显示对话UI
            GameEventBus.Instance.Trigger(new ShowDialogueUI());

            currentNode = node;
            NextDialogue();
        }

    }

    public void NextDialogue()
    {
        if (currentNode == null)
        {
            Debug.LogError("没有加载对话文件");
            return;
        }
        if (currentLineIndex < currentNode.lines.Count - 1)
        {
            currentLineIndex++;
            //TODO 通过事件处理，跟新对话ui显示文本，无选项更新
            GameEventBus.Instance.Trigger(
                new UpdateDialogueProgress(
                    currentNode.lines[currentLineIndex])
                );
        }
        else
        {
            if (currentNode.options == null || currentNode.options.Count == 0)
            {
                EndAllDialogue();
                return;
            }
            else
            {
                //TODO 通过事件处理，跟新对话ui显示文本，有选项更新
                GameEventBus.Instance.Trigger(
                    new UpdateDialogueProgress(
                        currentNode.lines[currentLineIndex],
                        currentNode.options
                    )
                );
            }
        }

        Debug.Log($"{currentLineIndex} +{currentLineIndex < currentNode.lines.Count - 1}");
    }

    public void UpdateDialogue(int option)
    {
        DialogueNode nextNode = currentDialogueData.nodes?[
                    currentNode.options[option].nextNodeId
                ];
        EndDialogueNode();
        currentNode = nextNode;
        StartDialogueNode(currentNode);
    }

    public void EndDialogueNode()
    {
        currentNode = null;
        currentLineIndex = -1;
    }

    public void EndAllDialogue()
    {
        EndDialogueNode();
        GameEventBus.Instance.Trigger(new HideDialogueUI());
    }
}