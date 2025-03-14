using UnityEngine;

public class Pickable : MonoBehaviour, IInteractable
{
    public bool isInteractable = true;
    public bool IsInteractable()
    {
        return isInteractable;
    }
    public void OnInteract(Interactor interactor)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isInteractable = false;
            Debug.Log("Picked " + gameObject.name);
            Destroy(gameObject);
        }
    }
    public void OnDetectionEnter() { }
    public void OnDetectionExit() { }
}