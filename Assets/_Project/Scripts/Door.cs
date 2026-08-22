using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool _isOpen = false;
    private Coroutine _coroutine;
    private Quaternion _openRotation;
    private Quaternion _closeRotation;

    private void Start()
    {
        _closeRotation = transform.localRotation;
        _openRotation = _closeRotation * Quaternion.Euler(0f, 90, 0f);
    }

    public void Interact(bool canInteract)
    {
        if (canInteract)
        {
            var flag = !_isOpen;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            if (flag)
            {
                _coroutine = StartCoroutine(Open(_openRotation, 3, () => { _isOpen = true; }));
            }
            else
            {
                _coroutine = StartCoroutine(Open(_closeRotation, 1.5f, () => { _isOpen = false; }));
            }
        }
        else
        {
            // TODO
        }
    }


    public IEnumerator Open(Quaternion target, float period, Action callback)
    {
        var startRotation = transform.rotation;
        var startTime = Time.time;
        while (startTime + period > Time.time)
        {
            var t = Mathf.Clamp01((Time.time - startTime) / period);
            transform.localRotation = Quaternion.Slerp(startRotation, target, t);
            yield return null;
        }
        callback?.Invoke();
    }

}
