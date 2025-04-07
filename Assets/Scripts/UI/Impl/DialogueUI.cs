using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 对话系统UI面板
/// </summary>
public class DialogueUI : BasePanel
{
    public GameObject speakerObj;
    public GameObject textObj;
    public GameObject optionBtn_Copy;
    public GameObject nextBtn;
    
    private List<GameObject> optionsBtn = new();
    
    private void Awake()
    {
        // 初始化并注册到面板管理器
        Init();
        
        // 订阅对话事件
        GameEventBus.Instance.Subscribe<UpdateDialogueProgress>(UpdateDialogueProgressHandle);
        GameEventBus.Instance.Subscribe<ShowDialogueUI>(ShowDialogueUIHandle);
        GameEventBus.Instance.Subscribe<HideDialogueUI>(HideDialogueUIHandle);
        
        // 默认隐藏
        Hide();
    }
    
    private void OnDestroy()
    {
        // 取消事件订阅
        GameEventBus.Instance.Unsubscribe<UpdateDialogueProgress>(UpdateDialogueProgressHandle);
        GameEventBus.Instance.Unsubscribe<ShowDialogueUI>(ShowDialogueUIHandle);
        GameEventBus.Instance.Unsubscribe<HideDialogueUI>(HideDialogueUIHandle);
    }
    
    private void Start()
    {
        // 注册Next按钮事件
        nextBtn.GetComponent<Button>().onClick.AddListener(NextBtn_Event);
    }
    
    // 显示对话UI的事件处理
    private void ShowDialogueUIHandle(ShowDialogueUI e)
    {
        Show();
    }
    
    // 隐藏对话UI的事件处理
    private void HideDialogueUIHandle(HideDialogueUI e)
    {
        Hide();
    }
    
    // 更新对话内容的事件处理
    private void UpdateDialogueProgressHandle(UpdateDialogueProgress e)
    {
        SetDialogueLine(e.line);
        
        // 处理对话选项
        if(e.option != null && e.option.Count > 0)
        {
            nextBtn.SetActive(false);
            SetDialogueOption(e.option);
        }
        else
        {
            nextBtn.SetActive(true);
            ClearOptions();
        }

        if(e.option==null && e.line==null){}
    }
    
    // 设置对话文本
    public void SetDialogueLine(DialogueNode.DialogueLine line)
    {
        speakerObj.GetComponent<TextMeshProUGUI>().text = line.speaker;
        textObj.GetComponent<TextMeshProUGUI>().text = line.text;
    }
    
    // 设置对话选项
    public void SetDialogueOption(List<DialogueNode.DialogueOption> options)
    {
        // 清除旧选项
        ClearOptions();
        Vector3 position= optionBtn_Copy.transform.localPosition;
        // 创建新选项
        for(int i = 0; i < options.Count; i++)
        {
            GameObject newBtn = Instantiate(optionBtn_Copy, optionBtn_Copy.transform.parent);
            newBtn.transform.localPosition = position+new Vector3(0,50*i,0);
            newBtn.SetActive(true);
            
            int optionIndex = i; // 避免闭包问题
            newBtn.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(optionIndex));
            newBtn.GetComponentInChildren<TextMeshProUGUI>().text = options[i].optionText;
            
            optionsBtn.Add(newBtn);
        }
    }
    
    // 隐藏所有选项
    // 2025-04-07 HideOptions()->ClearOptions() 我改了个名字
    private void ClearOptions()
    {
        foreach(var btn in optionsBtn)
        {
            Destroy(btn);
        }
        optionsBtn.Clear();
    }
    
    // 选项被选中时的回调
    private void OnOptionSelected(int optionIndex)
    {
        GameEventBus.Instance.Trigger(new ChooseDialogueOption(optionIndex));
    }
    
    // Next按钮点击事件
    public void NextBtn_Event()
    {
        Debug.Log("nextBtn_Event");
        GameEventBus.Instance.Trigger(new NextDialogueProgress());
    }
    
}