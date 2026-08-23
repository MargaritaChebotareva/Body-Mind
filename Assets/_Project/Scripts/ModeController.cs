using Unity.Cinemachine;
using UnityEngine;

public class ModeController : MonoBehaviour, ILookAroundSubscriber
{
    [SerializeField] private CinemachineCamera _bodyCamera;
    [SerializeField] private CinemachineCamera _mindCamera;
    [SerializeField] private CinemachineCamera _menuCamera;
    private GameInput _gameInput;
    private KeyController _keyController;
    private MovementController _movementController;
    private AudioSource _audioSource;

    public void Init(GameInput gameInput, KeyController keyController, MovementController movementController, SoundBar soundBar)
    {
        _gameInput = gameInput;
        _gameInput.RegistrateLookAround(this);
        _keyController = keyController;
        _movementController = movementController;
        SetMenuMode(true);
        if (soundBar.MindModeTheme != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = soundBar.MindModeTheme;
        }
    }

    public void OnLookAround(bool value)
    {
        if (value)
        {
            FocusOnMind();
        }
        else
        {
            FocusOnBody();
        }
    }

    public void SetMenuMode(bool active)
    {
        InitStates();
        if (active)
        {
            _menuCamera.Priority = 50;
            _keyController.OnModeChanged(false);
            _movementController.OnModeChanged(false);
        }
        else
        {
            _menuCamera.Priority = 0;
            _bodyCamera.Priority.Value = 10;
            _mindCamera.Priority.Value = 5;
            _keyController.OnModeChanged(true);
            _movementController.OnModeChanged(true);
            FocusOnBody();
        }
    }

    private void InitStates()
    {
        _keyController.InitStates();
        _movementController.InitStates();
    }

    private void FocusOnBody()
    {
        _bodyCamera.Priority.Value = 10;
        _mindCamera.Priority.Value = 5;
        _keyController.OnModeChanged(true);
        _movementController.OnModeChanged(true);
        _audioSource?.Stop();
    }

    private void FocusOnMind()
    {
        _mindCamera.Priority.Value = 15;
        _keyController.OnModeChanged(false);
        _movementController.OnModeChanged(false);
        _audioSource?.Play();
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateLookAround(this);
    }
}

