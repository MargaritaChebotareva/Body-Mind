using NUnit.Framework;
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
    

    public void OnEnable()
    {
        _movementController.Init(_gameInput, _animationController);
        _animationController.Init(_gameInput);
        _keyController.Init(_uIController, _gameInput);
        _modeController.Init(_gameInput, _keyController);
        _uIController.Init();
        _interactableController.Init(_gameInput, _uIController);
    }

}
