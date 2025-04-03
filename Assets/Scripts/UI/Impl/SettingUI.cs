using UnityEngine;
using UnityEngine.UI;
using TMPro; // 添加TextMeshPro命名空间

/// <summary>
/// 大屏设置面板
/// </summary>
public class SettingUI : BasePanel
{
    // 音效开关状态    
    private bool soundOn = true;

    private void Awake()
    {
        // // 添加调试日志，验证类型
        // Debug.Log("SettingUI类型： " + this.GetType().Name);
        // Debug.Log("SettingUI类型全名： " + this.GetType().FullName);
        // Debug.Log("是否继承自BasePanel: " + (this is BasePanel));

        // 确保正确注册到PanelManager
        Init();

        // 注册按钮事件
        btn_back.GetComponent<Button>().onClick.AddListener(btn_backEvent_closeSettingUI);//注册返回按钮事件
        btn_sound.GetComponent<Button>().onClick.AddListener(btn_sound_clickEvent);//注册声音按钮事件
        btn_scene.GetComponent<Button>().onClick.AddListener(btn_scene_clickEvent);//注册场景按钮事件
        btn_exit.GetComponent<Button>().onClick.AddListener(btn_exitEvent);//注册退出按钮事件

        // 初始化时更新音效按钮文本
        UpdateSoundButtonText();

        // 默认隐藏
        base.Hide();
    }
    // UI组件
    [SerializeField] private GameObject btn_back;
    [SerializeField] private GameObject btn_sound;
    [SerializeField] private GameObject btn_scene;
    [SerializeField] private GameObject btn_exit;
    [SerializeField] private GameObject soundButtonText;

    /// <summary>
    /// 返回按钮点击事件
    /// </summary>
    /// <remarks>
    /// 很栈的返回，直接关闭自己，调用Setting之前不需要关闭调用者
    /// 其可复用性强，哪都能放
    /// </remarks>
    private void btn_backEvent_closeSettingUI()
    {
        PanelManager.Instance.ClosePanel(typeof(SettingUI));
        PanelManager.Instance.OpenPanel(typeof(PlayStopUI),PanelManager.PanelLayer.PopWindow);
    }

    /// <summary>
    /// 更新音效按钮文本
    /// </summary>
    private void UpdateSoundButtonText()
    {
        if (soundButtonText)
        {
            Debug.Log("soundButtonText: " + soundButtonText.name);

            // 尝试获取TextMeshPro组件
            TextMeshProUGUI tmpText = soundButtonText.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = soundOn ? "音效: 开" : "音效: 关";
                Debug.Log("设置TMP文本为: " + tmpText.text);
                return;
            }

            // 尝试获取子物体的TextMeshPro组件
            tmpText = soundButtonText.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = soundOn ? "音效: 开" : "音效: 关";
                Debug.Log("设置子物体TMP文本为: " + tmpText.text);
                return;
            }

            // 尝试获取传统Text组件(备用)
            Text textComponent = soundButtonText.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = soundOn ? "音效: 开" : "音效: 关";
                Debug.Log("设置传统Text文本为: " + textComponent.text);
                return;
            }

            // 尝试获取子物体的传统Text组件(备用)
            textComponent = soundButtonText.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = soundOn ? "音效: 开" : "音效: 关";
                Debug.Log("设置子物体传统Text文本为: " + textComponent.text);
                return;
            }

            Debug.LogError("无法找到任何文本组件！请检查soundButtonText物体是否正确关联TextMeshPro或Text组件");
        }
        else
        {
            Debug.LogError("soundButtonText为空！请在Inspector中设置");
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
    private void btn_exitEvent()
    {
        Debug.Log("退出按钮点击事件");
        PanelManager.Instance.CloseAllPanels();
        PanelManager.Instance.OpenPanel(typeof(StartUI));
    }

    /// <summary>
    /// 重写Show方法，确保每次显示面板时更新音效按钮文本
    /// </summary>
    public override void Show()
    {
        base.Show();
        // 显示时更新音效按钮文本
        UpdateSoundButtonText();
    }
}