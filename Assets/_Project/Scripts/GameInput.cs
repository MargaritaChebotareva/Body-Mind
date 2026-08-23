using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActionAsset;

    private List<IMoveSubscriber> _moveSubscribers = new();
    private List<ILookSubscriber> _lookSubscribers = new();
    private List<IInteractSubscriber> _interactSubscribers = new();
    private List<ILookAroundSubscriber> _lookAroundSubscribers = new();
    private List<Func<char, bool>> _textInputSubscribers = new();

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _interactAction; 
    private InputAction _lookAroundAction;

    private bool _isLookAroundTurnOn = false;

    private void OnEnable()
    {
        _moveAction = _inputActionAsset.FindAction("Move");
        _lookAction = _inputActionAsset.FindAction("Look");
        _interactAction = _inputActionAsset.FindAction("Interact");
        _lookAroundAction = _inputActionAsset.FindAction("LookAround");

        _interactAction.performed += OnInteractActionPerformed;
        _lookAroundAction.performed += OnLookAroundActionPerformed;

        _moveAction.Enable();
        _lookAction.Enable();
        _interactAction.Enable();
        _lookAroundAction.Enable();

        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnLookAroundActionPerformed(InputAction.CallbackContext obj)
    {
        _isLookAroundTurnOn = !_isLookAroundTurnOn;
        foreach (var item in _lookAroundSubscribers)
        {
            item.OnLookAround(_isLookAroundTurnOn);
        }
    }

    private void OnInteractActionPerformed(InputAction.CallbackContext obj)
    {
        foreach (var item in _interactSubscribers)
        {
            item.OnInteract();
        }
    }

    private void Update()
    {
        // var look = _lookAction.ReadValue<Vector2>();

        var movement = _moveAction.ReadValue<Vector2>();
        foreach (var item in _lookSubscribers)
        {
            item.OnLook(movement);
        }

        foreach (var item in _moveSubscribers)
        {
            item.OnMove(movement);
        }

    }

    private void OnDestroy()
    {
        _moveAction.Disable();
        _lookAction.Disable();
        _interactAction.Disable();
        _lookAroundAction.Disable();

        Keyboard.current.onTextInput -= OnTextInput;
    }

    public void RegistrateMove(IMoveSubscriber moveSubscriber)
    {
        _moveSubscribers.Add(moveSubscriber);
    }

    public void UnregistrateMove(IMoveSubscriber moveSubscriber)
    {
        _moveSubscribers.Remove(moveSubscriber);
    }

    public void RegistrateLook(ILookSubscriber lookSubscriber)
    {
        _lookSubscribers.Add(lookSubscriber);
    }

    public void UnregistrateLook(ILookSubscriber lookSubscriber)
    {
        _lookSubscribers.Remove(lookSubscriber);
    }

    public void RegistrateInteract(IInteractSubscriber interactSubscriber)
    {
        _interactSubscribers.Add(interactSubscriber);
    }

    public void UnregistrateIneract(IInteractSubscriber interactSubscriber)
    {
        _interactSubscribers.Remove(interactSubscriber);
    }

    public void RegistrateLookAround(ILookAroundSubscriber lookAroundSubscriber)
    {
        _lookAroundSubscribers.Add(lookAroundSubscriber);
    }

    public void UnregistrateLookAround(ILookAroundSubscriber lookAroundSubscriber)
    {
        _lookAroundSubscribers.Remove(lookAroundSubscriber);
    }

    public void WaitForPin(Func<char, bool> onValueEntered)
    {
        _textInputSubscribers.Add(onValueEntered);
    }

    private void OnTextInput(char c)
    {
        List<Func<char, bool>> toRemove = new();
        foreach (var x in _textInputSubscribers)
        {
            if (!x(c))
            {
                toRemove.Add(x);
            }
        }

        foreach (var x in toRemove)
        {
            _textInputSubscribers.Remove(x);
        }
    }
}

public interface IMoveSubscriber
{
    public void OnMove(Vector2 movement);
}

public interface ILookSubscriber
{
    public void OnLook(Vector2 look);
}

public interface IInteractSubscriber
{
    public void OnInteract();
}

public interface ILookAroundSubscriber
{
    public void OnLookAround(bool value);
}