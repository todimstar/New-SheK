using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏中的玩家HUD面板
/// </summary>
public class PlayerHUDUI : BasePanel
{
    [SerializeField] private GameObject menuButton;
    [Header("Mood Manager")]
    [SerializeField] private BarManager moodManager;

    [Header("Mood Text")]
    [SerializeField] GameObject moodText;  // 心情值文本

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

    private void Start()
    {
        Init(); // 确保注册到PanelManager
        PanelManager.Instance.OpenPanel(typeof(PlayerHUDUI)); // 显示自己

        // 注册菜单按钮点击事件
        menuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClick);

        // 订阅心情变化事件
        if (moodManager != null)
        {
            moodManager.OnMoodChanged += OnMoodChanged;
        }
    }
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
                OnMenuButtonClick();
                Debug.Log("PlayerHUDUI尝试暂停游戏");
                PlayStopUI.isGamePaused = true;
            }
        }
    }

    public void OnMenuButtonClick()
    {
        // 菜单按钮点击事件
        Debug.Log("菜单按钮点击事件");

        //打开菜单面板
        PanelManager.Instance.OpenPanel(typeof(PlayStopUI), PanelManager.PanelLayer.PopWindow);

        // 获取PlayStopUI实例并调用暂停方法
        PlayStopUI playStopUI = FindObjectOfType<PlayStopUI>();
        if (playStopUI != null)
        {
            // 调用私有方法需要使用反射或修改PlayStopUI的PauseGame为public
            // 这里假设我们已将PauseGame改为public
            playStopUI.PauseGame();
        }
        else
        {
            // 如果找不到PlayStopUI实例，直接设置时间缩放
            Debug.LogWarning("找不到PlayStopUI实例，直接设置Time.timeScale = 0");
            Time.timeScale = 0f;
        }
    }

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

        // 可以在这里添加其他心情效果，如粒子效果、音效等
    }
}
