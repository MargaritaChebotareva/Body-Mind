using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private MovementController _movementController;
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private ModeController _modeController;
    [SerializeField] private UIController _uIController;
    [SerializeField] private InteractableController _interactableController;
    [SerializeField] private KeyController _keyController;
    [SerializeField] private SoundBar _soundBar;
    [SerializeField] private TriggerController _triggerController;

    [SerializeField] private LayerMask _playerMask;
    public void OnEnable()
    {
        _movementController.Init(_gameInput, _animationController);
        _animationController.Init(_gameInput);
        _keyController.Init(_uIController, _gameInput, _soundBar, _modeController, _playerMask);
        _modeController.Init(_gameInput, _keyController, _movementController, _soundBar, _uIController);
        _uIController.Init(_modeController);
        _interactableController.Init(_gameInput, _uIController, _modeController);
        _triggerController.Init(_gameInput, _keyController, _uIController);
    }

}
