using UnityEngine;
using UnityEngine.UIElements;

public class TP_point : MonoBehaviour, IInteractable
{
    public Transform target;

    public bool IsInteractable { get; set; } = true;

    public void OnDetectionEnter()
    {
        // TODO 实现提示可以传送
        Debug.Log("可以传送到" + target.name);
    }

    public void OnDetectionExit()
    {
    }

    public void OnInteract(Interactor interactor)
    {
        if (IsInteractable && Input.GetKeyDown(KeyCode.E))
        {
            interactor.transform.GetComponent<CharacterController>().enabled = false;
            Debug.Log(interactor.gameObject.name + "传送到" + target.name);
            interactor.gameObject.transform.position = target.position;
            interactor.transform.GetComponent<CharacterController>().enabled = true;
        }
    }
}