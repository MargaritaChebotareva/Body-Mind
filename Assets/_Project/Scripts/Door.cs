using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool _isOpen = false;
    private Coroutine _coroutine;
    private Quaternion _openRotation;
    private AudioSource _audioSource;
    private Quaternion _closeRotation;
    [SerializeField] private KeyController _keyController;
    public bool Locked = false;
    public bool IsOpen => _isOpen;
    [SerializeField] public bool FinalDoor;

    public void Init()
    {
        _closeRotation = transform.localRotation;
        _openRotation = _closeRotation * Quaternion.Euler(0f, 90, 0f);
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ResetDoorState()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        transform.localRotation = _closeRotation;
        _isOpen = false;
    }

    public void Interact(bool canInteract)
    {
        if (canInteract)
        {
            _keyController.RequestOpen(this);
        }
    }

    public void OpenDoor()
    {
        Locked = false;
        var flag = !_isOpen;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if (flag)
        {
            _coroutine = StartCoroutine(Open(_openRotation, 1.5f, () => { _isOpen = true; }));
        }
        else
        {
            _coroutine = StartCoroutine(Open(_closeRotation, 1.5f, () => { _isOpen = false; }));
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

    public void PlaySound(AudioClip audio)
    {
        if (audio != null)
        {
            _audioSource.PlayOneShot(audio);
        }
    }
}
