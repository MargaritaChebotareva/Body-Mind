using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private MovementController _movementController;
    [SerializeField] private AnimationController _animationController;

    public void OnEnable()
    {
        _movementController.Init(_gameInput, _animationController);
        _animationController.Init(_gameInput);
    }

}
