using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏中的暂停面板_也是Normal型面板
/// </summary>
/// <remarks>
/// 按照目前暂停的设计，脚本必须绑定在Canvas下的PlayStopPanel上
/// </remarks>
public class PlayStopUI : BasePanel
{
    [SerializeField] private GameObject settingButton;//设置按钮
    [SerializeField] private GameObject closeButton;//返回按钮
    [SerializeField] private GameObject playStopPanel = null;//暂停面板
    [SerializeField] private bool stopUIShow = false;//是否已开启过stop面板

    void Start()
    {
        // 默认隐藏该弹窗
        gameObject.SetActive(false);
        // 防止playStopPanel没有手动拖过，为空
        if(playStopPanel == null){
            Transform canvas = transform.parent;
            GameObject playStopPanel = canvas.Find("PlayStopPanel").gameObject;
        }
        
    }

    void Update()//应该需要一个StuteManager来管理游戏状态，其中的GameStatus枚举类型包括：游戏中、游戏暂停、游戏结束
    {   
        if(stopUIShow){
            // 暂停键
            if(Input.GetKeyDown(KeyCode.Escape)){
                //按下ESC键

                // 检查是否找到并设置为活动状态
                if (playStopPanel != null)
                {
                    playStopPanel.SetActive(false);
                    stopUIShow = false;
                }
                else
                {
                    Debug.LogError("PlayStopPanel 没在同一个 Canvas下找到");
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape)){
            //按下ESC键

            // Check if found and set it active
            if (playStopPanel != null)
            {
                playStopPanel.SetActive(true);
                stopUIShow = true;
            }
            else
            {
                Debug.LogError("PlayStopPanel 没在同一个 Canvas下找到");
            }
        }
        
    }


    /// <summary>
    /// 关闭按钮点击事件
    /// </summary>
    public void OnCloseButtonClick(){
        // Close button click event
        Debug.Log("关闭按钮点击事件");
        // Close the PlaySettingPanel
        playStopPanel.SetActive(false);
    }

    /// <summary>
    /// 设置按钮点击事件
    /// </summary>
    /// <remarks>
    /// 转向设置面板
    /// 寻找同一个 Canvas 下的 SettingPanel
    /// 检查是否找到并设置为活动状态
    /// </remarks>
    public void OnSettingButtonClick(){
        
        Debug.Log("设置按钮点击事件");
        // 转向设置面板
        // 寻找同一个 Canvas 下的 SettingPanel
        Transform canvas = transform.parent;
        GameObject settingPanel = canvas.Find("SettingPanel").gameObject;

        // 检查是否找到并设置为活动状态
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("SettingPanel 没在同一个 Canvas下找到");
        }
    }
}
