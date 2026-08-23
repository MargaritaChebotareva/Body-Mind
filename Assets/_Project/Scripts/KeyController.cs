using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    private ModeController _modeController;
    private readonly HashSet<int> _keyItemInHandId = new();

    public void Init(UIController uiController, GameInput gameInput, ModeController modeController)
    {
        _uiController = uiController;
        _gameInput = gameInput;
        _modeController = modeController;

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
            ProceedWithOpening(door);
            return;
        }

        // door with key
        var keyItem = _doorsAndKeys.FirstOrDefault(x => x.Door == door.gameObject);
        if (keyItem != null)
        {
            if (_keyItemInHandId.Contains(keyItem.Key))
            {
                _uiController.ShowMessage("Door unlocked");
                ProceedWithOpening(door);
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
                        _uiController.ShowMessage("Door unlocked");
                        ProceedWithOpening(door);
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

        ProceedWithOpening(door);
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

    public bool HasKey(int id)
    {
        return _keyItemInHandId.Contains(id);
    }

    public void InteractWithKeyItemOnScene(int id)
    {
        if (!HasKey(id))
        {
            var index = _keyItemOnScene.FindIndex(x => x.Id == id);
            var item = _keyItemOnScene[index];
            item.Interact(true);
        }
    }

    public bool CanSee(int id)
    {
        if (!HasKey(id))
        {
            var index = _keyItemOnScene.FindIndex(x => x.Id == id);
            var item = _keyItemOnScene[index];
            var position = item.GetKeyPos();
            var ray = _modeController.GetRay();
            var cameraDir = ray.direction;
            var itemDir = (position - ray.origin).normalized;
            cameraDir.y = 0;
            cameraDir.Normalize();
            itemDir.y = 0;
            itemDir.Normalize();
            float dotProduct = Vector3.Dot(cameraDir, itemDir);
            float grad45 = 0.7071f;
            return dotProduct > grad45;
        }
        return false;
    }
}