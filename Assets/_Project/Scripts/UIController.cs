using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image _tip;
    [SerializeField] private Image _pinLabel;
    [SerializeField] private Text _pinInput;

    private Coroutine _coroutineHidePin;

    public void Init()
    {
        HideTip();
        HidePin(false);
    }

    public void ShowTip()
    {
        _tip.gameObject.SetActive(true);
    }

    public void HideTip()
    {
        _tip.gameObject.SetActive(false);
    }

    public void RequestEnterPin()
    {
        _pinInput.text = "";
        _pinLabel.gameObject.SetActive(true);
    }

    public void ShowPin(string pin)
    {
        _pinInput.text = pin;
    }

    public void CorrectPin()
    {
        HidePin(true);
    }

    public void WrongPin()
    {
        _pinInput.text = "Incorrect PIN";
        HidePin(true);
    }

    private void HidePin(bool delayed)
    {
        if (!delayed)
        {
            _pinLabel.gameObject.SetActive(false);
            return;
        }

        if (_coroutineHidePin != null)
        {
            StopCoroutine(_coroutineHidePin);
            _coroutineHidePin = null;
        }

        _coroutineHidePin = StartCoroutine(HidePinRoutine(1.5f, () =>
        {
            _pinLabel.gameObject.SetActive(false);
        }));
    }

    public IEnumerator HidePinRoutine(float period, Action callback)
    {
        var startTime = Time.time;
        while (startTime + period > Time.time)
        {
            yield return null;
        }

        callback?.Invoke();
    }
}
