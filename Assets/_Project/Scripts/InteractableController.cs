using System.Collections;
using UnityEngine;

public class InteractableController : MonoBehaviour, IInteractSubscriber
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _interactLayer;
    private GameInput _gameInput;
    private UIController _uiController;
    private Vector3 _viewPoint = new(0.5f, 0.5f, 0);

    public void Init(GameInput gameInput, UIController uIController)
    {
        _gameInput = gameInput;
        _gameInput.RegistrateInteract(this);
        _uiController = uIController;
    }

    private void Start()
    {
        StartCoroutine(CheckEnviroment());
    }
    private IEnumerator CheckEnviroment()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            FindItem();
        }
    }

    private IInteractable FindItem()
    {
        var ray = _mainCamera.ViewportPointToRay(_viewPoint);
        if (Physics.Raycast(ray, out var hit, 20, _interactLayer) && hit.collider.gameObject.TryGetComponent<IInteractable>(out var item))
        {
            _uiController.ShowTip();
            return item;
        }
        _uiController.HideTip();
        _uiController.HideEnterPin();
        return null;

    }

    public void OnInteract()
    {
        var item = FindItem();
        if (item != null)
        {
            item.Interact(true);
        }
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateIneract(this);
    }
}