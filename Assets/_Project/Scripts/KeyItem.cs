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

    public int Id => _id;
    public void Init(KeyController keyController)
    {
        _keyController = keyController;
        _isVisible = _key.gameObject.activeSelf && _item.gameObject.activeSelf;
        _isBodyMode = true;
        HandleMode(_isBodyMode);
        _key.Init(this);
        _item.Init(this);
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
        _key.gameObject.SetActive(visible);
        _item.gameObject.SetActive(visible);
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
    private bool _isBodyMode;

    public int Id => _id;
    public void Init(KeyController keyController)
    {
        _keyController = keyController;
        _isBodyMode = true;
        HandleMode(_isBodyMode);
    }

    public void Interact(bool canInteract)
    {
        SetActive(false);
        SetActive(false);
        _keyController.ShowKey(_id);
    }

    public void SetActive(bool visible)
    {
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
        _itemBody.gameObject.SetActive(_isBodyMode);
        _itemMind.gameObject.SetActive(!_isBodyMode);
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