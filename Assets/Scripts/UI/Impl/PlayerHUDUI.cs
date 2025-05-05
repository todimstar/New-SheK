using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏中的玩家HUD面板
/// </summary>
public class PlayerHUDUI : BasePanel
{
    [SerializeField] private GameObject stopButton;
    [Header("Mood Manager")]
    [SerializeField] private BarManager moodManager;

    [Header("Mood Text")]
    [SerializeField] GameObject moodText;  // 心情值文本

    [Header("Stop Button")]
    [SerializeField] private Image stopButtonImage;  // 按钮上的图像组件
    [SerializeField] private Sprite pauseIcon;       // 暂停图标
    [SerializeField] private Sprite playIcon;        // 播放图标

    [Header("Quest System")]
    [SerializeField] private TextMeshProUGUI questTypeText;       // 任务类型文本，默认显示“主线任务：”
    [SerializeField] private TextMeshProUGUI questDescriptionText; // 任务描述文本

    /// <summary>
    /// 心情状态枚举
    /// </summary>
    public enum MoodState
    {
        VeryLonely,  // 孤独
        Introverted, // 内向
        Normal,      // 普通
        Extroverted, // 外向
        SuperSocial  // 社牛
    }

    private void Awake()
    {
        Init(); // 确保注册到PanelManager
        //PanelManager.Instance.OpenPanel(typeof(PlayerHUDUI)); // 显示自己     //04.02 单场景后由StartUI控制显示
        Hide(); // 默认隐藏

        // 注册菜单按钮点击事件
        stopButton.GetComponent<Button>().onClick.AddListener(OnStopButtonClick);

        // 订阅心情变化事件
        if (moodManager != null)
        {
            moodManager.OnMoodChanged += OnMoodChanged;
        }
    }

    #region 暂停面板相关

    void Update()   
    {
        // 按ESC键打开/关闭暂停面板
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("检测到ESC键按下");

            //根据PlayStopUI的单例isGamePaused状态，决定是否打开/关闭暂停面板
            if (PlayStopUI.isGamePaused)
            {
                PlayStopUI playStopUI = FindObjectOfType<PlayStopUI>();
                playStopUI.OnCloseButtonClick();
                Debug.Log("PlayerHUDUI尝试恢复游戏");
                PlayStopUI.isGamePaused = false;
            }
            else
            {
                OnStopButtonClick();
                Debug.Log("PlayerHUDUI尝试暂停游戏");
                PlayStopUI.isGamePaused = true;
            }
        }
        // 根据游戏暂停状态更新按钮图标
        if (stopButtonImage != null && pauseIcon != null && playIcon != null)
        {
            stopButtonImage.sprite = PlayStopUI.isGamePaused ? playIcon : pauseIcon;//暂停显示播放图标，进行中显示暂停图标
        }
    }

    public void OnStopButtonClick()
    {
        // 暂停按钮点击事件
        Debug.Log("暂停按钮点击事件");
        if (PlayStopUI.isGamePaused)
        {
            //关闭stop面板
            PanelManager.Instance.ClosePanel(typeof(PlayStopUI), PanelManager.PanelLayer.PopWindow);

            PlayStopUI playStopUI = FindObjectOfType<PlayStopUI>();
            if (playStopUI != null)
            {
                // 调用私有方法需要使用反射或修改PlayStopUI的ResumeGame为public
                // ResumeGame已改为public
                playStopUI.OnCloseButtonClick();
                PlayStopUI.isGamePaused = false;
            }
            else
            {
                // 如果找不到PlayStopUI实例，直接设置时间缩放
                Debug.LogWarning("找不到PlayStopUI实例，直接设置Time.timeScale = 1");
                Time.timeScale = 1f;
                PlayStopUI.isGamePaused = false;
            }

        }
        else
        {
            //打开stop面板
            PanelManager.Instance.OpenPanel(typeof(PlayStopUI), PanelManager.PanelLayer.PopWindow);

            // 获取PlayStopUI实例并调用暂停方法
            PlayStopUI playStopUI = FindObjectOfType<PlayStopUI>();
            if (playStopUI != null)
            {
                playStopUI.PauseGame();
                PlayStopUI.isGamePaused = true;
            }
            else
            {
                // 如果找不到PlayStopUI实例，直接设置时间缩放
                Debug.LogWarning("找不到PlayStopUI实例，直接设置Time.timeScale = 0");
                Time.timeScale = 0f;
                PlayStopUI.isGamePaused = true;
            }
        }
    }

    #endregion

    #region 对接任务系统

    /// <summary>
    /// 更新任务信息
    /// </summary>
    /// <param name="questType">任务类型（如"主线任务："、"支线任务："等）</param>
    /// <param name="description">任务描述</param>
    /// <remarks>
    /// "questType"任务类型（如"主线任务："、"支线任务："等）
    /// "description"任务描述文本
    /// </remarks>
    public void UpdateQuestInfo(string questType, string description)
    {
        // 更新任务类型
        if (questTypeText != null)
        {
            questTypeText.text = questType;
        }

        // 更新任务描述
        if (questDescriptionText != null)
        {
            questDescriptionText.text = description;
        }

        Debug.Log($"任务已更新: {questType} - {description}");
    }

    /// <summary>
    /// 获取当前任务信息
    /// </summary>
    public void GetQuestInfo(out string questType, out string description)//out，简洁实现多返回值
    {
        questType = questTypeText != null ? questTypeText.text : string.Empty;
        description = questDescriptionText != null ? questDescriptionText.text : string.Empty;

        Debug.Log($"当前任务信息: {questType} - {description}");
    }

    /// <summary>
    /// 清除当前任务显示
    /// </summary>
    public void ClearQuestInfo()
    {
        if (questTypeText != null)
        {
            questTypeText.text = string.Empty;
        }

        if (questDescriptionText != null)
        {
            questDescriptionText.text = string.Empty;
        }

        Debug.Log("任务显示已清除");
    }

    #endregion

    #region 心情值相关

    /// <summary>
    /// 根据心情值获取对应的心情状态
    /// </summary>
    private MoodState GetMoodState(float moodValue)
    {
        // 假设心情值范围是0-100
        if (moodValue < 20f)
            return MoodState.VeryLonely;
        else if (moodValue < 40f)
            return MoodState.Introverted;
        else if (moodValue < 60f)
            return MoodState.Normal;
        else if (moodValue < 80f)
            return MoodState.Extroverted;
        else
            return MoodState.SuperSocial;
    }

    /// <summary>
    /// 获取心情状态对应的文本
    /// </summary>
    private string GetMoodStateText(MoodState state)
    {
        switch (state)
        {
            case MoodState.VeryLonely:
                return "孤独";
            case MoodState.Introverted:
                return "内向";
            case MoodState.Normal:
                return "普通";
            case MoodState.Extroverted:
                return "外向";
            case MoodState.SuperSocial:
                return "社牛";
            default:
                return "未知";
        }
    }

    private void OnMoodChanged(float oldValue, float newValue, bool isHeal)
    {
        // 获取心情状态
        MoodState moodState = GetMoodState(newValue);
        string stateText = GetMoodStateText(moodState);

        // 更新UI文本
        if (moodText != null)
        {
            TextMeshProUGUI tmpText = moodText.GetComponent<TextMeshProUGUI>();
            tmpText.text = $"心情: {stateText} ({Mathf.RoundToInt(newValue)}%)";
        }

        // 日志记录
        Debug.Log($"心情值变化: {oldValue} -> {newValue}, 心情状态: {stateText}");

        // 显示对应特效
        switch (moodState)
        {
            case MoodState.VeryLonely:
                EffectManager.Instance.ShowEffect(EffectType.VeryLonely);//若无duration参数则默认3秒后消失
                break;
            case MoodState.Introverted:
                EffectManager.Instance.ShowEffect(EffectType.Introverted);
                break;
            // ...其他状态
            case MoodState.Normal:
                EffectManager.Instance.ShowEffect(EffectType.Normal);
                break;
            case MoodState.Extroverted:
                EffectManager.Instance.ShowEffect(EffectType.Extroverted);
                break;
            case MoodState.SuperSocial:
                EffectManager.Instance.ShowEffect(EffectType.SuperSocial);
                break;
        }

        // 可以在这里添加其他心情效果，如粒子效果、音效等
    }

    #endregion
}
