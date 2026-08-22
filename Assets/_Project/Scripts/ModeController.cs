using Unity.Cinemachine;
using UnityEngine;

public class ModeController : MonoBehaviour, ILookAroundSubscriber
{
    [SerializeField] private CinemachineCamera _bodyCamera;
    [SerializeField] private CinemachineCamera _mindCamera;
    private GameInput _gameInput;
    private KeyController _keyController;
    public void Init(GameInput gameInput, KeyController keyController)
    {
        _gameInput = gameInput;
        _gameInput.RegistrateLookAround(this);
        _keyController = keyController;
        FocusOnBody();
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

    private void FocusOnBody()
    {
        _bodyCamera.Priority.Value = 10;
        _mindCamera.Priority.Value = 5;
        _keyController.OnModeChanged(true);
    }

    private void FocusOnMind()
    {
        _mindCamera.Priority.Value = 15;
        _keyController.OnModeChanged(false);
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateLookAround(this);
    }
}

