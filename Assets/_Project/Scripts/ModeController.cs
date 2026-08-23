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
    private UIController _uiController;
    private AudioSource _audioSource;

    private CinemachineCamera _currentPlayerCamera;
    private Vector3 _viewPoint = new(0.5f, 0.5f, 0);
    public void Init(GameInput gameInput, KeyController keyController, MovementController movementController, SoundBar soundBar, UIController uiController)
    {
        _gameInput = gameInput;
        _gameInput.RegistrateLookAround(this);
        _keyController = keyController;
        _movementController = movementController;
        _uiController = uiController;
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
            FocusOnMind(true);
        }
        else
        {
            _menuCamera.Priority = 0;
            FocusOnBody(true);
        }
    }

    private void InitStates()
    {
        _keyController.InitStates();
        _movementController.InitStates();
    }

    private void FocusOnBody(bool silent = false)
    {
        _bodyCamera.Priority.Value = 10;
        _mindCamera.Priority.Value = 5;
        _currentPlayerCamera = _bodyCamera;
        _keyController.OnModeChanged(true);
        _movementController.OnModeChanged(true);
        _uiController.ShowModeText(true);
        if (!silent) _audioSource?.Stop();
    }

    private void FocusOnMind(bool silent = false)
    {
        _mindCamera.Priority.Value = 15;
        _currentPlayerCamera = _mindCamera;
        _keyController.OnModeChanged(false);
        _movementController.OnModeChanged(false);
        _uiController.ShowModeText(false);
        if (!silent) _audioSource?.Play();
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateLookAround(this);
    }

    public Ray GetPlayerRay()
    {
        Ray ray = Camera.main.ViewportPointToRay(_viewPoint);
        ray.origin = _currentPlayerCamera.Target.TrackingTarget.position;
        return ray;
    }

    public Ray GetCameraRay()
    {
        return Camera.main.ViewportPointToRay(_viewPoint);
    }
}

