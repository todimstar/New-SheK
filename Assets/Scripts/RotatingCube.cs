using UnityEngine;

/// <summary>
/// 简单的旋转立方体，用于测试时间暂停功能
/// </summary>
public class RotatingCube : MonoBehaviour
{
    // 旋转速度
    [SerializeField] private float rotationSpeed = 50f;
    
    // 是否使用Time.deltaTime (受时间缩放影响)
    [SerializeField] private bool useTimeScale = true;
    
    // 显示当前时间缩放值
    [SerializeField] private TMPro.TextMeshProUGUI timeScaleText;

    void Update()
    {
        // 更新时间缩放显示
        if (timeScaleText != null)
        {
            timeScaleText.text = $"Time Scale: {Time.timeScale:F2}";
        }
        
        // 根据是否使用时间缩放来旋转
        if (useTimeScale)
        {
            // 使用 Time.deltaTime (受时间缩放影响)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // 使用 Time.unscaledDeltaTime (不受时间缩放影响)
            transform.Rotate(Vector3.up, rotationSpeed * Time.unscaledDeltaTime);
        }
    }
} 