using UnityEngine;

public class AnimationController : MonoBehaviour, IInteractSubscriber
{
    [SerializeField] private Animator _controller;
    private readonly int _hashSpeed = Animator.StringToHash("Speed");
    private readonly int _hashTakeItem = Animator.StringToHash("TakeItem");

    private GameInput _gameInput;
    public void Init(GameInput gameInput)
    {
        _gameInput = gameInput;
        _gameInput.RegistrateInteract(this);
    }

    public void OnInteract()
    {
        _controller.SetTrigger(_hashTakeItem);
    }

    public void UpdateSpeed(float value)
    {
        _controller.SetFloat(_hashSpeed, value);
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateIneract(this);
    }
}