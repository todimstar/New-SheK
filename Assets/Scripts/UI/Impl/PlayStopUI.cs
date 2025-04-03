using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏中的暂停面板_也是Normal型面板
/// </summary>
/// <remarks>
/// 使用PanelManager管理面板的显示和隐藏
/// 按ESC键可以打开和关闭暂停面板
/// </remarks>
public class PlayStopUI : BasePanel
{
    [SerializeField] private GameObject settingButton;//设置按钮
    [SerializeField] private GameObject closeButton;//返回按钮

    // 是否已暂停游戏
    public static bool isGamePaused = false;

    void Awake()
    {
        // // 添加调试日志，验证类型
        // Debug.Log("PlayStopUI类型： " + this.GetType().Name);
        // Debug.Log("PlayStopUI类型全名： " + this.GetType().FullName);
        // Debug.Log("BasePanel类型： " + typeof(BasePanel).Name);
        // Debug.Log("是否继承自BasePanel: " + (this is BasePanel));

        // 确保正确注册
        Init(); // 注册到PanelManager
        // 默认隐藏该弹窗
        Hide();

        // 注册按钮事件
        if (closeButton != null)
        {
            closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCloseButtonClick);
        }

        if (settingButton != null)
        {
            settingButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnSettingButtonClick);
        }
    }


    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        // 暂停游戏时间
        Time.timeScale = 0f;
        isGamePaused = true;

        Debug.Log("游戏已暂停");
    }

    /// <summary>
    /// 恢复游戏
    /// </summary>
    public void ResumeGame()
    {
        // 恢复游戏时间
        Time.timeScale = 1f;
        isGamePaused = false;

        Debug.Log("游戏已恢复");
    }

    /// <summary>
    /// 关闭按钮点击事件
    /// </summary>
    public void OnCloseButtonClick()
    {
        Debug.Log("关闭按钮点击事件");
        // 关闭面板并暂停游戏
        PanelManager.Instance.ClosePanel(typeof(PlayStopUI), PanelManager.PanelLayer.PopWindow);

        ResumeGame();
    }

    /// <summary>
    /// 设置按钮点击事件
    /// </summary>
    /// <remarks>
    /// 打开设置面板
    /// </remarks>
    public void OnSettingButtonClick()
    {
        Debug.Log("设置按钮点击事件");
        // 隐藏当前暂停面板（不关闭）
        PanelManager.Instance.ClosePanel(typeof(PlayStopUI), PanelManager.PanelLayer.PopWindow);
        // 打开设置面板
        PanelManager.Instance.OpenPanel(typeof(SettingUI));
    }

}
