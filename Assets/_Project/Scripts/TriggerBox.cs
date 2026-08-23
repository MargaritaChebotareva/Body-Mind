using UnityEngine;

public class TriggerBox : MonoBehaviour, IInteractSubscriber
{
    [SerializeField] private int _keySetId;
    private GameInput _gameInput;
    private KeyController _keyController;
    private UIController _uiController;
    private bool _inside;

    public void Init(GameInput gameInput, KeyController keyController, UIController uIController)
    {
        _gameInput = gameInput;
        _keyController = keyController;
        _uiController = uIController;
        _gameInput.RegistrateInteract(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        var hasKey = _keyController.HasKey(_keySetId);
        if (!hasKey && _keyController.CanSee(_keySetId))
        {
            _uiController.ShowTriggerTip();
        }
        _inside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        var hasKey = _keyController.HasKey(_keySetId);
        if (!hasKey || !_keyController.CanSee(_keySetId))
        {
            _uiController.HideTriggerTip();
        }
        _inside = false;
    }


    private void OnTriggerStay(Collider other)
    {
        var hasKey = _keyController.HasKey(_keySetId);
        if (!hasKey && _keyController.CanSee(_keySetId))
        {
            _uiController.ShowTriggerTip();
        }
        else
        {
            _uiController.HideTriggerTip();
        }
    }

    public void OnInteract()
    {
        if (_inside)
        {
            _keyController.InteractWithKeyItemOnScene(_keySetId);
        }
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateIneract(this);
    }
}