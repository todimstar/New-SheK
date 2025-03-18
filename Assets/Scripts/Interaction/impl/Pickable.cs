using UnityEngine;

public class Pickable : MonoBehaviour, IInteractable
{
    public bool IsInteractable{ get; set; }=true;

    public void OnInteract(Interactor interactor)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IsInteractable = false;
            Debug.Log("Picked " + gameObject.name);
            Destroy(gameObject);
        }
    }
    public void OnDetectionEnter()
    {
        Debug.Log("可以按 E 捡起");
    }
    public void OnDetectionExit() { }
}