using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 大屏设置面板
/// </summary>
public class SettingUI : BasePanel
{
    // 音效开关状态    
    private bool soundOn = true;

    private void Start()
    {
        Init();
        btn_back.GetComponent<Button>().onClick.AddListener(btn_backEvent_closeSettingUI);//注册返回按钮事件
        btn_sound.GetComponent<Button>().onClick.AddListener(btn_sound_clickEvent);//注册声音按钮事件
        btn_scene.GetComponent<Button>().onClick.AddListener(btn_scene_clickEvent);//注册场景按钮事件
        btn_exit.GetComponent<Button>().onClick.AddListener(btn_exitEvent);//注册退出按钮事件
    }
    // UI组件
    [SerializeField] private GameObject btn_back;
    [SerializeField] private GameObject btn_sound;
    [SerializeField] private GameObject btn_scene;
    [SerializeField] private GameObject btn_exit;
    [SerializeField] private GameObject soundButtonText;

    private void btn_backEvent_closeSettingUI(){
        PanelManager.Instance.ClosePanel(typeof(SettingUI));
    }
    
    /// <summary>
    /// 更新音效按钮文本
    /// </summary>
    private void UpdateSoundButtonText()
    {
        if (soundButtonText)
        {   
            // 获取Text上的文本组件
            Text textComponent = soundButtonText.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = soundOn ? "音效: 开" : "音效: 关";
            }
        }
    }

    /// <summary>
    /// 音效按钮点击事件
    /// </summary>
    private void btn_sound_clickEvent()
    {
        soundOn = !soundOn;
        Debug.Log("声音状态: " + (soundOn ? "开启" : "关闭"));
        
        // 这里可以添加音效控制代码
        // 例如: AudioListener.volume = soundOn ? 1 : 0;
        
        UpdateSoundButtonText();
    }

    /// <summary>
    /// 场景按钮点击事件
    /// </summary>
    private void btn_scene_clickEvent()
    {
        Debug.Log("场景按钮点击");
    }


    /// <summary>
    /// 退出按钮点击事件
    /// </summary>
    private void btn_exitEvent(){
        Debug.Log("退出按钮点击事件");
        Application.Quit();
    }


}