using UnityEngine;

/// <summary>
/// 不受时间缩放影响的运动，作为对照
/// </summary>
public class UnscaledMovement : MonoBehaviour
{
    // 移动速度
    [SerializeField] private float speed = 2f;
    
    // 移动范围
    [SerializeField] private float movementRange = 3f;
    
    // 初始位置
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 使用 Time.unscaledTime 作为无视时间缩放的时间值
        float offset = Mathf.Sin(Time.unscaledTime * speed) * movementRange;
        
        // 在X轴上来回移动
        transform.position = startPosition + new Vector3(offset, 0, 0);
    }
} 