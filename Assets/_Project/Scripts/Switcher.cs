using UnityEngine;

public class Switcher : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] _lamps;
    public void Interact()
    {
        for (int i = 0; i < _lamps.Length; i++)
        {
            _lamps[i].SetActive(!_lamps[i].activeSelf);
        }
    }
}