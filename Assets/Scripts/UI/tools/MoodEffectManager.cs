using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  该脚本使用方法：
///  在相机上添加该脚本并创建一个Canvas (设为Screen Space - Overlay)
///  将Canvas拖入MoodEffectManager的overlayCanvas字段
///  选择或创建几个特效预制体，分配给相应的情绪状态，还可以设置位置
/// </summary>
public class EffectManager : MonoBehaviour 
{
    [System.Serializable]
    public class EffectData
    {
        public EffectType effectType;
        public GameObject effectPrefab;
        public Vector2 screenPosition; // 预设位置(0,0为左下角，1,1为右上角)
    }
    
    [SerializeField] private Canvas overlayCanvas; // 覆盖在屏幕上的Canvas

    [Header("screenPosition (0,0为左下角，1,1为右上角，0.5,0.5为中心)")]
    [SerializeField] private EffectData[] effectData;
    
    private Dictionary<EffectType, GameObject> activeEffects = new Dictionary<EffectType, GameObject>();
    
    private static EffectManager _instance;
    public static EffectManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<EffectManager>();
            return _instance;
        }
    }
    
    /// <summary>
    /// 显示指定类型的特效
    /// </summary>
    /// <param name="type">特效类型</param>
    /// <param name="duration">持续时间(0表示不自动销毁)</param>
    public void ShowEffect(EffectType type, float duration = 3f)
    {
        // 如果该类型特效已存在且不想重复显示，先销毁
        if (activeEffects.ContainsKey(type))
        {
            Destroy(activeEffects[type]);
            activeEffects.Remove(type);
        }
        
        // 查找并显示匹配的特效
        foreach (var effect in effectData)
        {
            if (effect.effectType == type)
            {
                // 实例化特效并添加到Canvas
                GameObject effectObj = Instantiate(effect.effectPrefab, overlayCanvas.transform);
                
                // 设置位置
                RectTransform rectTransform = effectObj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    SetPositionFromPreset(rectTransform, effect.screenPosition);
                }
                
                // 如果需要自动销毁
                if (duration > 0)
                {
                    StartCoroutine(DestroyAfterDelay(type, effectObj, duration));
                }
                else
                {
                    // 否则记录到活动特效字典
                    activeEffects[type] = effectObj;
                }
                
                return;
            }
        }
    }
    
    /// <summary>
    /// 隐藏指定类型的特效
    /// </summary>
    public void HideEffect(EffectType type)
    {
        if (activeEffects.ContainsKey(type))
        {
            Destroy(activeEffects[type]);
            activeEffects.Remove(type);
        }
    }
    
    /// <summary>
    /// 隐藏所有特效
    /// </summary>
    public void HideAllEffects()
    {
        foreach (var effect in activeEffects.Values)
        {
            Destroy(effect);
        }
        activeEffects.Clear();
    }
    
    private void SetPositionFromPreset(RectTransform rectTransform, Vector2 screenPosition)
    {
        // 设置锚点和位置
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        
        // 将0-1的值转换为实际屏幕坐标
        float x = screenPosition.x * Screen.width;
        float y = screenPosition.y * Screen.height;
        rectTransform.anchoredPosition = new Vector2(x, y);
    }
    
    private IEnumerator DestroyAfterDelay(EffectType type, GameObject effectObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (activeEffects.ContainsKey(type) && activeEffects[type] == effectObj)
        {
            activeEffects.Remove(type);
        }
        
        Destroy(effectObj);
    }
}