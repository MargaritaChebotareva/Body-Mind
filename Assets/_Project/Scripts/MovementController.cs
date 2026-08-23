using UnityEngine;

public class MovementController : MonoBehaviour, IMoveSubscriber
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _acceleration = 25;
    [SerializeField] private float _rotationSpeed = 20;
    [SerializeField] private float _animationSpeed = 5f;

    [SerializeField] private float _gravity = -9.81f;
    private GameInput _gameInput;
    private AnimationController _animationController;
    private float _currentSpeed = 0f;
    private Vector3 _gravityMovement;
    private float _currentAnimationSpeed = 0f;
    private bool _isBodyMode;

    public void Init(GameInput gameInput, AnimationController animationController)
    {
        _isBodyMode = true;
        _gameInput = gameInput;
        _gameInput.RegistrateMove(this);
        _animationController = animationController;
    }

    public void OnMove(Vector2 movement)
    {
        if (_isBodyMode)
        {
            BodyMovement(movement);
        }
        else
        {
            BodyMovement(Vector2.zero);
        }
    }

    public void OnModeChanged(bool isBodyMode)
    {
        _isBodyMode = isBodyMode;
    }

    private void BodyMovement(Vector2 movement)
    {
        if (_characterController.isGrounded)
        {
            _gravityMovement.y = -2.0f;
        }

        float yRotation = movement.x * _rotationSpeed * Time.deltaTime;
        bool hasRotate = Mathf.Abs(yRotation) > 0.01f;

        transform.Rotate(Vector3.up * yRotation);

        float targetSpeed = 0;
        float targetAnimationSpeed = 0;
        if (movement.y > 0)
        {
            targetSpeed = hasRotate ? _speed * 0.5f : _speed;
            targetAnimationSpeed = hasRotate ? _animationSpeed * 0.5f : _animationSpeed;
        }
        else if (movement.y < 0)
        {
            targetSpeed = hasRotate ? -_speed * 0.5f : -_speed;
            targetAnimationSpeed = hasRotate ? -_animationSpeed * 0.5f : -_animationSpeed;
        }
        else
        {
            targetSpeed = 0;
            targetAnimationSpeed = 0;
        }

        var localMove = new Vector3(movement.x, 0, movement.y);
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, _acceleration * Time.deltaTime);
        _currentAnimationSpeed = Mathf.MoveTowards(_currentAnimationSpeed, targetAnimationSpeed, _acceleration * Time.deltaTime);

        var globalMovement = transform.TransformDirection(localMove);

        _characterController.Move(globalMovement * Mathf.Abs(_currentSpeed) * Time.deltaTime);
        _animationController.UpdateSpeed(_currentAnimationSpeed);

        _gravityMovement.y += _gravity * Time.deltaTime;
        _characterController.Move(_gravityMovement * Time.deltaTime);
    }


    private void OnDestroy()
    {
        _gameInput.UnregistrateMove(this);
    }
}
