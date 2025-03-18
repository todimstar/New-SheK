using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件总线
/// </summary>
public class GameEventBus : MonoBehaviour
{

    private static GameEventBus _instance;
    public static GameEventBus Instance => _instance ??= FindObjectOfType<GameEventBus>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            // 切换场景时，不用删除
            DontDestroyOnLoad(gameObject);
        }
    }
    /// <summary>
    /// 事件表
    /// </summary>
    private Dictionary<Type, Delegate> eventTable = new(); 

    /// <summary>
    /// 添加 订阅 事件
    /// </summary>
    /// <typeparam name="TEvent">实现IEvent的类</typeparam>
    /// <param name="callback"></param>
    public void Subscribe<TEvent>(Action<TEvent> callback)
        where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        // 如果事件表里已经存在这个事件，则添加回调
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = Delegate.Combine(eventTable[eventType], callback);
        }
        else
        {
            // 如果事件表里不存在这个事件，则添加事件和回调
            eventTable[eventType] = callback;
        }
    }

    /// <summary>
    /// 去除 监听 的事件
    /// </summary>
    /// <typeparam name="TEvent">实现IEvent的类</typeparam>
    public void Unsubscribe<TEvent>(Action<TEvent> callback)
        where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        // 如果事件表里存在这个事件，则删除回调
        if (eventTable.TryGetValue(eventType, out var actions))
        {
            actions = Delegate.Remove(actions, callback);
            if (actions == null)
            {
                eventTable.Remove(eventType);
            }
            else
            {
                eventTable[eventType] = actions;
            }
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <typeparam name="TEvent">实现IEvent的类</typeparam>
    public void Trigger<TEvent>(TEvent data) 
        where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        if (eventTable.TryGetValue(eventType, out var actions))
        {
            data.OnEvent();
            
            (actions as Action<TEvent>)?.Invoke(data);
            
        }
    }
}