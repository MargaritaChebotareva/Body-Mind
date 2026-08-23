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
    private CinemachineCamera _currentPlayerCamera;
    private Vector3 _viewPoint = new(0.5f, 0.5f, 0);
    public void Init(GameInput gameInput, KeyController keyController, MovementController movementController)
    {
        _gameInput = gameInput;
        _gameInput.RegistrateLookAround(this);
        _keyController = keyController;
        _movementController = movementController;
        SetMenuMode(true);
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
        if (active)
        {
            _menuCamera.Priority = 50;
            FocusOnMind();
        }
        else
        {
            _menuCamera.Priority = 0;
            FocusOnBody();
        }
    }

    private void FocusOnBody()
    {
        _bodyCamera.Priority.Value = 10;
        _mindCamera.Priority.Value = 5;
        _keyController.OnModeChanged(true);
        _movementController.OnModeChanged(true);
        _currentPlayerCamera = _bodyCamera;
    }

    private void FocusOnMind()
    {
        _mindCamera.Priority.Value = 15;
        _keyController.OnModeChanged(false);
        _movementController.OnModeChanged(false);
        _currentPlayerCamera = _mindCamera;
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateLookAround(this);
    }

    public Ray GetRay()
    {
        Ray ray = Camera.main.ViewportPointToRay(_viewPoint);
        ray.origin = _currentPlayerCamera.Target.TrackingTarget.position;
        return ray;
    }
}

