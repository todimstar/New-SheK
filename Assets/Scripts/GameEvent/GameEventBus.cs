using System;
using System.Collections.Generic;
using UnityEngine;

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

    private Dictionary<Type, Delegate> eventTable = new();

    public void Subscribe<TEvent>(Action<TEvent> callback)
        where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);

        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = Delegate.Combine(eventTable[eventType], callback);
        }
        else
        {
            eventTable[eventType] = callback;
        }
    }
    public void Unsubscribe<TEvent>(Action<TEvent> callback)
        where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);

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

    public void Trigger<TEvent>(TEvent data) 
        where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        if (eventTable.TryGetValue(eventType, out var actions))
        {
            if (actions is Action<TEvent> action)
            {
                action?.Invoke(data);
            }
        }
    }
}