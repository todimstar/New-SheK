using System;
using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public InteractionDetector detector;

    private void Awake() {
        detector = GetComponentInChildren<InteractionDetector>();
    }

    private void Update()
    {
        // 拿到当前的交互物体，并调用交互方法
        detector.GetCurrentInteractable()?.OnInteract(this);
    }
}