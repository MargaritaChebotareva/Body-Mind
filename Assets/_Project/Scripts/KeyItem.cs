using System;
using UnityEngine;

[Serializable]
public class KeyItem 
{
    [SerializeField] private InteractItem _key;
    [SerializeField] private InteractItem _item;
    [SerializeField] private int _id;
    private KeyController _keyController;
    private bool _isVisible;
    private bool _isBodyMode;
    private LayerMask _layerMask;

    private ModeController _modeController;
    public int Id => _id;
    public bool IsInteract { get; private set; }
    public void Init(KeyController keyController, ModeController modeController, LayerMask layerMask)
    {
        _keyController = keyController;
        _modeController = modeController;
        _layerMask = layerMask;
        _isVisible = _key.gameObject.activeSelf && _item.gameObject.activeSelf;
        _isBodyMode = true;
        IsInteract = false;
        HandleMode(_isBodyMode);
        _key.Init(this);
        _item.Init(this);
    }

    public void Interact(bool canInteract)
    {
        if (!IsInteract)
        {
            return;
        }
        SetActive(false);
        SetActive(false);
        _keyController.ShowKey(_id);
        _isVisible = false;
    }

    public void SetActive(bool visible)
    {
        _isVisible = visible;
        _key.gameObject.SetActive(visible);
        _item.gameObject.SetActive(visible);
        HandleMode(_isBodyMode);
    }

    public void OnModeChanged(bool isBodyMode)
    {
        _isBodyMode = isBodyMode;
        HandleMode(isBodyMode);
        if (!IsInteract)
        {
            IsInteract = CheckVisbileFromMind();
            _key.ChangeInteract();
            _item.ChangeInteract();
        }
    }

    private void HandleMode(bool isBodyMode)
    {
        if (_isVisible)
        {
            _item.gameObject.SetActive(_isBodyMode);
            _key.gameObject.SetActive(!_isBodyMode);
        }
    }

    private bool CheckVisbileFromMind()
    {
        var position = _key.transform.position;
        var ray = _modeController.GetPlayerRay();
        var cameraDir = ray.direction;
        var itemDir = (position - ray.origin).normalized;
        cameraDir.y = 0;
        cameraDir.Normalize();
        itemDir.y = 0;
        itemDir.Normalize();
        float dotProduct = Vector3.Dot(cameraDir, itemDir);
        var isVisible = dotProduct > 0;
        if (!isVisible)
        {
            return false;
        }
        var newRay = new Ray(position, ray.origin - position);
        if (Physics.Raycast(newRay, out var hit))
        {
            if (hit.collider.gameObject.layer == _layerMask)
            {
                return true;
            }
        }
        return false;
    } 

    public Vector3 GetKeyPos()
    {
        return _key.transform.position;
    }
}

[Serializable]
public class KeyItemNotInteract
{
    [SerializeField] private GameObject _key;
    [SerializeField] private GameObject _item;
    [SerializeField] private int _id;
    private bool _isVisible;
    private bool _isBodyMode;

    public int Id => _id;

    public void Init()
    {
        _isVisible = _key.activeSelf && _item.activeSelf;
        _isBodyMode = true;
        HandleMode(_isBodyMode);
    }

    public void SetActive(bool visible)
    {
        _isVisible = visible;
        _key.SetActive(visible);
        _item.SetActive(visible);
        HandleMode(_isBodyMode);
    }

    public void OnModeChanged(bool isBodyMode)
    {
        _isBodyMode = isBodyMode;
        HandleMode(isBodyMode);
    }
    private void HandleMode(bool isBodyMode)
    {
        if (_isVisible)
        {
            _item.gameObject.SetActive(_isBodyMode);
            _key.gameObject.SetActive(!_isBodyMode);
        }
    }
}

[Serializable]
public class MindBodyItem
{
    [SerializeField] private GameObject _itemMind;
    [SerializeField] private GameObject _itemBody;
    [SerializeField] private int _id;
    private KeyController _keyController;
    private bool _isVisible;
    private bool _isBodyMode;

    public int Id => _id;
    public void Init(KeyController keyController)
    {
        _keyController = keyController;
        _isVisible = _itemMind.gameObject.activeSelf && _itemBody.gameObject.activeSelf;
        _isBodyMode = true;
        HandleMode(_isBodyMode);
    }

    public void Interact(bool canInteract)
    {
        SetActive(false);
        SetActive(false);
        _keyController.ShowKey(_id);
        _isVisible = false;
    }

    public void SetActive(bool visible)
    {
        _isVisible = visible;
        _itemMind.gameObject.SetActive(visible);
        _itemBody.gameObject.SetActive(visible);
        HandleMode(_isBodyMode);
    }

    public void OnModeChanged(bool isBodyMode)
    {
        _isBodyMode = isBodyMode;
        HandleMode(isBodyMode);
    }

    private void HandleMode(bool isBodyMode)
    {
        if (_isVisible)
        {
            _itemBody.gameObject.SetActive(_isBodyMode);
            _itemMind.gameObject.SetActive(!_isBodyMode);
        }
    }
}


[Serializable]
public class KeyDoorItem
{
    [SerializeField] public int Key;
    [SerializeField] public GameObject Door;

    public void Init()
    {
        Door.GetComponent<Door>().Locked = true;
    }
}

[Serializable]
public class PincodeDoorItem
{
    [SerializeField] public string Pincode;
    [SerializeField] public GameObject Door;

    public void Init()
    {
        Door.GetComponent<Door>().Locked = true;
    }
}