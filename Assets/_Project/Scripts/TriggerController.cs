using UnityEngine;

public class TriggerController : MonoBehaviour
{
    [SerializeField] private TriggerBox[] _triggerBoxes;

    public void Init(GameInput gameInput, KeyController keyController, UIController uIController)
    {
        foreach (var trigger in _triggerBoxes)
        {
            trigger.Init(gameInput, keyController, uIController);
        }
    }
}
