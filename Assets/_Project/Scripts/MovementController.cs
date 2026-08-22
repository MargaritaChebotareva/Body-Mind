using UnityEngine;

public class MovementController : MonoBehaviour, IMoveSubscriber
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _acceleration = 25;
    [SerializeField] private float _rotationSpeed = 20;
    private GameInput _gameInput;
    private AnimationController _animationController;
    private float _currentSpeed = 0f;

    public void Init(GameInput gameInput, AnimationController animationController)
    {
        _gameInput = gameInput;
        _gameInput.RegistrateMove(this);
        _animationController = animationController;
    }

    public void OnMove(Vector2 movement)
    {
        float y = movement.x * _rotationSpeed * Time.deltaTime;
        bool hasRotate = Mathf.Abs(y) > 0.01f;
        transform.Rotate(Vector3.up * y);

        float targetSpeed = 0;
        if (movement.y > 0)
        {
            targetSpeed = hasRotate ? _speed * 0.5f : _speed;
        }
        else if (movement.y < 0)
        {
            targetSpeed = hasRotate ? -_speed * 0.5f : -_speed;
        }
        else
        {
            targetSpeed = 0;
        }


        var localMove = new Vector3(movement.x, 0, movement.y);
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, _acceleration * Time.deltaTime);

        var globalMovement = transform.TransformDirection(localMove);

        _characterController.Move(globalMovement * Mathf.Abs(_currentSpeed) * Time.deltaTime);
        _animationController.UpdateSpeed(_currentSpeed);
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateMove(this);
    }
}
