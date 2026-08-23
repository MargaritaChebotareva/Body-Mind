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
    [SerializeField] private GameObject _allDoorsRoot;

    private UIController _uiController;
    private GameInput _gameInput;
    private SoundBar _soundBar;
    private readonly HashSet<int> _keyItemInHandId = new();

    public void Init(UIController uiController, GameInput gameInput, SoundBar soundBar)
    {
        _uiController = uiController;
        _gameInput = gameInput;
        _soundBar = soundBar;

        foreach (var door in _allDoorsRoot.GetComponentsInChildren<Door>())
        {
            door.Init();
        }

        InitStates();
    }

    public void InitStates()
    {
        foreach (var x in _keyItemOnScene)
        {
            x.Init(this);
            x.SetActive(true);
        }

        foreach (var x in _keyItemInHand)
        {
            x.Init();
            x.SetActive(false);
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

        foreach (var door in _allDoorsRoot.GetComponentsInChildren<Door>())
        {
            door.ResetDoorState();
        }

        _keyItemInHandId.Clear();
    }

    public void ShowKey(int id)
    {
        int index = _keyItemInHand.FindIndex(key => key.Id == id);
        _keyItemInHand[index].SetActive(true);
        _keyItemInHandId.Add(id);
        _soundBar.PlayOnCharacter(_soundBar.PickItem);
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
            door.PlaySound(door.IsOpen ? _soundBar.DoorClosed : _soundBar.DoorOpen);
            ProceedWithOpening(door);
            return;
        }

        // door with key
        var keyItem = _doorsAndKeys.FirstOrDefault(x => x.Door == door.gameObject);
        if (keyItem != null)
        {
            if (_keyItemInHandId.Contains(keyItem.Key))
            {
                door.PlaySound(_soundBar.DoorUnlocked);
                _uiController.ShowMessage("Door unlocked");
                ProceedWithOpening(door);
            }

            door.PlaySound(_soundBar.DoorLocked);
            return;
        }

        // door with pincode
        var pincodeItem = _pincodeDoors.FirstOrDefault(x => x.Door == door.gameObject);
        if (pincodeItem != null && !string.IsNullOrEmpty(pincodeItem.Pincode))
        {
            _uiController.RequestEnterPin();
            door.PlaySound(_soundBar.PinStart);

            string pin = "";
            _gameInput.WaitForPin(c =>
            {
                if (pin.Length > 0 && !char.IsDigit(c))
                {
                    // invalid letter
                    door.PlaySound(_soundBar.PinError);
                    _uiController.IncorrectPin();
                    return false;
                }

                if (char.IsDigit(c))
                {
                    door.PlaySound(_soundBar.PinInput);
                    pin += c;
                }

                _uiController.ShowPin(pin);
                if (pin.Length >= pincodeItem.Pincode.Length)
                {
                    if (pincodeItem.Pincode == pin)
                    {
                        _uiController.CorrectPin();
                        _uiController.ShowMessage("Door unlocked");
                        door.PlaySound(_soundBar.DoorUnlocked);
                        ProceedWithOpening(door);
                    }
                    else
                    {
                        door.PlaySound(_soundBar.PinError);
                        _uiController.IncorrectPin();
                    }

                    return false;
                }

                // continue typing
                return true;
            });

            return;
        }

        ProceedWithOpening(door);
        door.PlaySound(door.IsOpen ? _soundBar.DoorClosed : _soundBar.DoorOpen);
        return;
    }

    private void ProceedWithOpening(Door door)
    {
        door.OpenDoor();
        if (door.FinalDoor)
        {
            _uiController.Win();
        }
    }
}