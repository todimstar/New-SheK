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
        detector.GetCurrentInteractable()?.OnInteract(this);
    }
}