using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private List<KeyItem> _keyItemOnScene;
    [SerializeField] private List<MindBodyItem> _mindBodyItems;
    [SerializeField] private List<KeyItemNotInteract> _keyItemInHand;
    [SerializeField] private List<KeyDoorItem> _doorsAndKeys;
    [SerializeField] private List<PincodeDoorItem> _pincodeDoors;

    private UIController _uiController;
    private GameInput _gameInput;
    private readonly HashSet<int> _keyItemInHandId = new();

    public void Init(UIController uiController, GameInput gameInput)
    {
        _uiController = uiController;
        _gameInput = gameInput;

        foreach (var x in _keyItemOnScene)
        {
            x.Init(this);
        }

        foreach (var x in _keyItemInHand)
        {
            x.Init();
        }

        foreach (var x in _mindBodyItems)
        {
            x.Init(this);
        }

        foreach (var x in _doorsAndKeys)
        {
            x.Init();
        }

        foreach (var x in _pincodeDoors)
        {
            x.Init();
        }
    }

    public void ShowKey(int id)
    {
        int index = _keyItemInHand.FindIndex(key => key.Id == id);
        _keyItemInHand[index].SetActive(true);
        _keyItemInHandId.Add(id);
    }

    public void OnModeChanged(bool isBodyMode)
    {
        foreach (var x in _keyItemOnScene)
        {
            x.OnModeChanged(isBodyMode);
        }

        foreach (var x in _keyItemInHand)
        {
            x.OnModeChanged(isBodyMode);
        }

        foreach (var x in _mindBodyItems)
        {
            x.OnModeChanged(isBodyMode);
        }
    }

    public void RequestOpen(Door door)
    {
        // not locked door
        if (!door.Locked)
        {
            door.OpenDoor();
            return;
        }

        // door with key
        var keyItem = _doorsAndKeys.FirstOrDefault(x => x.Door == door.gameObject);
        if (keyItem != null)
        {
            if (_keyItemInHandId.Contains(keyItem.Key))
            {
                door.OpenDoor();
            }

            return;
        }

        // door with pincode
        var pincodeItem = _pincodeDoors.FirstOrDefault(x => x.Door == door.gameObject);
        if (pincodeItem != null && !string.IsNullOrEmpty(pincodeItem.Pincode))
        {
            _uiController.RequestEnterPin();
            
            string pin = "";
            _gameInput.WaitForPin(c =>
            {
                if (pin.Length > 0 && !char.IsDigit(c))
                {
                    // invalid letter
                    _uiController.IncorrectPin();
                    return false;
                }

                if (char.IsDigit(c))
                {
                    pin += c;
                }

                _uiController.ShowPin(pin);
                if (pin.Length >= pincodeItem.Pincode.Length)
                {
                    if (pincodeItem.Pincode == pin)
                    {
                        _uiController.CorrectPin();
                        door.OpenDoor();
                    }
                    else
                    {
                        _uiController.IncorrectPin();
                    }

                    return false;
                }

                // continue typing
                return true;
            });

            return;
        }

        door.OpenDoor();
        return;
    }
}