using UnityEngine;

public class InteractItem : MonoBehaviour, IInteractable
{
    private KeyItem _keyItem;
    public bool IsInteract { get; private set; } 
    public void Init(KeyItem keyItem)
    {
        _keyItem = keyItem;
        IsInteract = _keyItem.IsInteract;
    }

    public void Interact(bool canInteract)
    {
        _keyItem.Interact(canInteract);
    }

    public void ChangeInteract()
    {
        IsInteract = _keyItem.IsInteract;
    }
}
