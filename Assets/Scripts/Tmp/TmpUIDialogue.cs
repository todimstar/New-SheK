using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TmpUIDialogue : MonoBehaviour
{
    /**     没有仔细做对话ui，这只是一个临时的
    *       用到的文件
    *       /Assets/Scripts/Dialogue/*  该目录下的全部,对话系统文件可以说
    *       /Assets/Resources/Dialogue/* 该目录下的全部，对话内容的json文件
    *       对话UI被打开，应该在show那里直接调用 GameEventBus.Instance.Trigger(new NextDialogueProgress()); 或者是用NextBtn_Event函数，设置第一句台词
    *       
    *       中间层GameBus就调用对应的函数，调用到DialoguePlayer里面的nextDialogue
    *
    *       nextDialogue里面又通过中间层GameBus调用UpdateDialogueProgressHandle函数，就是把UpdateDialogueProgress封装的内容拿出来，在调用SetDialogueLine函数
    *
    */
    public GameObject speakerObj;
    public GameObject textObj;
    public GameObject optionBtn_Copy;
    public GameObject nextBtn;

    private List<GameObject> optionsBtn= new();

    private void Awake()
    {
        // 用事件总线，加个中间层调用
        GameEventBus.Instance.Subscribe<UpdateDialogueProgress>(
            UpdateDialogueProgressHandle
        );
    }
    /// <summary>
    /// 设置对话内容
    /// </summary>
    /// <param name="e"></param>
    private void UpdateDialogueProgressHandle(UpdateDialogueProgress e){
        SetDialogueLine(e.line);
        //设置选项还没有做
    }

    private void Start()
    {
        nextBtn.GetComponent<Button>().onClick.AddListener(NextBtn_Event);
    }

    // 事件的接口，方便我设置那个什么UpdateDialogueProgressHandle这个事件，
    public void SetDialogueLine(DialogueNode.DialogueLine line)
    {
        speakerObj.GetComponent<TextMeshProUGUI>().text = line.speaker;
        textObj.GetComponent<TextMeshProUGUI>().text = line.text;
    }

    // 同SetDialogueLine上
    public void SetDialogueOption(List<DialogueNode.DialogueOption> options)
    {
        Debug.Log("还没有实现");
    }

    public void NextBtn_Event(){
        GameEventBus.Instance.Trigger(new NextDialogueProgress());
    }
}
