using UnityEngine;

public class InteractItem : MonoBehaviour, IInteractable
{
    private KeyItem _keyItem;
    public void Init(KeyItem keyItem)
    {
        _keyItem = keyItem;
    }

    public void Interact(bool canInteract)
    {
        _keyItem.Interact(canInteract);
    }
}
