using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;

public class BarManager : MonoBehaviour
{

    [Header("Health Bar")]
    [SerializeField] MicroBar healthBar;

    // 定义委托和事件
    public delegate void MoodChangeHandler(float oldValue, float newValue, bool isHeal);
    public event MoodChangeHandler OnMoodChanged;

    const float MAX_HP = 100f;
    float _hp = MAX_HP;

    #region Debug and testing controls
    // Debug and testing controls
    [Header("Debug - Testing Only")]
    [SerializeField] private bool enableTestingControls = true;
    [SerializeField] private UnityEngine.UI.Button increaseMoodButton;
    [SerializeField] private UnityEngine.UI.Button decreaseMoodButton;
    [SerializeField] private float testMoodChangeAmount = 10f;

    private void Awake()
    {
        // Setup test buttons
        if (enableTestingControls)
        {
            if (increaseMoodButton != null)
                increaseMoodButton.onClick.AddListener(TestIncreaseMood);
            
            if (decreaseMoodButton != null)
                decreaseMoodButton.onClick.AddListener(TestDecreaseMood);
            
        }
    }

    // Test methods
    #if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void TestIncreaseMood()
    {
        AddMood(testMoodChangeAmount);
        Debug.Log($"[TEST] Increased mood by {testMoodChangeAmount}. New value: {_hp}");
    }

    private void TestDecreaseMood()
    {
        ReduceMood(testMoodChangeAmount);
        Debug.Log($"[TEST] Decreased mood by {testMoodChangeAmount}. New value: {_hp}");
    }
    #endif

    #endregion

    float HP
    {
        get => _hp;
        set
        {
            // 先保存旧值用于事件
            float oldValue = _hp;
            bool isHeal = value > _hp;
            _hp = Mathf.Clamp(value, 0f, MAX_HP);

            if (isHeal)
            {
                healthBar.UpdateBar(_hp, UpdateAnim.Heal);
            }
            else
            {
                healthBar.UpdateBar(_hp, UpdateAnim.Damage);
            }

            // 触发事件
            OnMoodChanged?.Invoke(oldValue, _hp, isHeal);
        }
    }
    private void Start()
    {
        healthBar.Initialize(MAX_HP);
    }

    #region 设置心情值和接口相关
    // 设置心情值
    public void SetMoodValue(float moodValue)
    {
        HP = moodValue;
    }

    // 获取当前心情值
    public float GetCurrentMood()
    {
        return _hp;
    }

    // 获取心情值百分比
    public float GetMoodPercentage()
    {
        return _hp / MAX_HP;
    }

    // 增加心情值
    public void AddMood(float amount)
    {
        HP += amount;
    }

    // 减少心情值
    public void ReduceMood(float amount)
    {
        HP -= amount;
    }

    #endregion
}