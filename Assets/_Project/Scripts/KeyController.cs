using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private List<KeyItem> _keyItemOnScene;
    [SerializeField] private List<MindBodyItem> _mindBodyItems;
    [SerializeField] private List<KeyItemNotInteract> _keyItemInHand;
    [SerializeField] private List<KeyDoorItem> _doorsAndKeys;
    private readonly HashSet<int> _keyItemInHandId = new();

    public void Init()
    {
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

    public bool CanOpen(Door door)
    {
        var item = _doorsAndKeys.FirstOrDefault(x => x.Door == door.gameObject);
        if (item == null)
        {
            return true;
        }

        return _keyItemInHandId.Contains(item.Key);
    }
}