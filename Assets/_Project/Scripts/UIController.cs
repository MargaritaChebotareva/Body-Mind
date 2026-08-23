using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image _tip;
    [SerializeField] private Image _triggerTip;

    [SerializeField] private Image _pinLabel;
    [SerializeField] private Text _pinInput;

    [SerializeField] private Text _messageText;
    [SerializeField] private Image _messageImage;
    [SerializeField] private Image _buttonPlay;

    private Coroutine _coroutineHidePin;
    private Coroutine _coroutineHideMessage;
    private ModeController _modeController;

    public void Init(ModeController modeController)
    {
        _modeController = modeController;
        
        HideTip();
        HideTriggerTip();
        HidePin(false);
        HideMessage();
        ShowMenu();
    }

    public void ShowTip()
    {
        _tip.gameObject.SetActive(true);
    }

    public void HideTip()
    {
        _tip.gameObject.SetActive(false);
    }

    public void ShowTriggerTip()
    {
        _triggerTip.gameObject.SetActive(true);
    }

    public void HideTriggerTip()
    {
        _triggerTip.gameObject.SetActive(false);
    }
    public void RequestEnterPin()
    {
        if (_coroutineHidePin != null)
        {
            StopCoroutine(_coroutineHidePin);
            _coroutineHidePin = null;
        }

        _pinInput.text = "";
        _pinLabel.gameObject.SetActive(true);
    }

    public void ShowPin(string pin)
    {
        _pinInput.text = pin;
    }

    public void HideEnterPin()
    {
        HidePin(false);
    }

    public void IncorrectPin()
    {
        _pinInput.text = "Incorrect PIN";
        HidePin(true);
    }

    public void CorrectPin()
    {
        HidePin(true);
        HideMessageDelay();
    }

    public void ShowMenu()
    {
        _buttonPlay.gameObject.SetActive(true);
        _modeController.SetMenuMode(true);
    }

    public void ShowGame()
    {
        _buttonPlay.gameObject.SetActive(false);
        _modeController.SetMenuMode(false);
    }

    public void Win()
    {
        ShowMessage("Victory!");
        // todo make character unable to move
        // todo fade white
        // todo credits
        ShowMenu();
    }

    private void HidePin(bool delayed)
    {
        if (_coroutineHidePin != null)
        {
            StopCoroutine(_coroutineHidePin);
            _coroutineHidePin = null;
        }

        if (!delayed)
        {
            _pinLabel.gameObject.SetActive(false);
            return;
        }        

        _coroutineHidePin = StartCoroutine(WaitAndDo(1.5f, () =>
        {
            _pinLabel.gameObject.SetActive(false);
        }));
    }

    public IEnumerator WaitAndDo(float period, Action callback)
    {
        var startTime = Time.time;
        while (startTime + period > Time.time)
        {
            yield return null;
        }

        callback?.Invoke();
    }

    public void HideMessage()
    {
        _messageImage.gameObject.SetActive(false);
    }

    public void HideMessageDelay()
    {
        if (_coroutineHideMessage != null)
        {
            StopCoroutine(_coroutineHideMessage);
            _coroutineHideMessage = null;
        }
        _coroutineHideMessage = StartCoroutine(WaitAndDo(1.5f, () =>
        {
            HideMessage();
        }));
    }

    public void ShowMessage(string text)
    {
        if (_coroutineHideMessage != null)
        {
            StopCoroutine(_coroutineHideMessage);
            _coroutineHideMessage = null;
        }
        _messageText.text = text;
        _messageImage.gameObject.SetActive(true);
    }
}
