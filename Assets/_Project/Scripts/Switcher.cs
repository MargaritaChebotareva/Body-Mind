using UnityEngine;

public class Switcher : MonoBehaviour, IInteractable
{
    [SerializeField] private Light[] _lamps;
    [SerializeField] private ReflectionProbe _reflectionProbe;
    public void Interact()
    {
        for (int i = 0; i < _lamps.Length; i++)
        {
            _lamps[i].enabled = !_lamps[i].enabled;
        }

        if (_reflectionProbe != null)
        {
            _reflectionProbe.RenderProbe();
        }
    }
}