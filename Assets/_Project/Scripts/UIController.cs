using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image _tip;
    [SerializeField] private Image _triggerTip;
    [SerializeField] private Image _tip2;
    [SerializeField] private Text _tip2Text;

    [SerializeField] private Image _pinLabel;
    [SerializeField] private Text _pinInput;

    [SerializeField] private Text _messageText;
    [SerializeField] private Image _messageImage;
    [SerializeField] private Image _buttonPlay;
    [SerializeField] private Image _fadeWhite;
    [SerializeField] private Image _title;
    private Coroutine _coroutineHidePin;
    private Coroutine _coroutineHideMessage;
    private ModeController _modeController;

    private static readonly Color _fadeColor0 = new Color(1, 1, 1, 1);
    private static readonly Color _fadeColor100 = new Color(1, 1, 1, 0);

    public void Init(ModeController modeController)
    {
        _modeController = modeController;
        
        HideTip();
        HideTriggerTip();
        HidePin(false);
        HideMessage();
        ShowMenu();
        _fadeWhite.color = _fadeColor100;
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
    }

    public void ShowMenu()
    {
        _buttonPlay.gameObject.SetActive(true);
        _modeController.SetMenuMode(true);
        _title.gameObject.SetActive(true);
        _tip2.gameObject.SetActive(false);
    }

    public void ShowGame()
    {
        _buttonPlay.gameObject.SetActive(false);
        _title.gameObject.SetActive(false);
        _tip2.gameObject.SetActive(true);
        _modeController.SetMenuMode(false);
        ShowMessage("Objective: open the door", 10);
    }

    public void Win()
    {
        ShowMessage("Victory!");
        StartCoroutine(WinRoutine());
    }

    private IEnumerator WinRoutine()
    {
        // todo lock all controls
        _modeController.OnLookAround(true);

        yield return new WaitForSeconds(1.5f);

        _modeController.OnLookAround(false);
        _tip2.gameObject.SetActive(false);

        var fadeTime = 2f;
        var time = fadeTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            _fadeWhite.color = Color.Lerp(_fadeColor100, _fadeColor0, (fadeTime - time) / fadeTime);
            yield return null;
        }

        ShowMenu();

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            _fadeWhite.color = Color.Lerp(_fadeColor0, _fadeColor100, time / fadeTime);
            yield return null;
        }
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

    public void HideMessageDelay(float delay)
    {
        if (_coroutineHideMessage != null)
        {
            StopCoroutine(_coroutineHideMessage);
            _coroutineHideMessage = null;
        }
        _coroutineHideMessage = StartCoroutine(WaitAndDo(delay, () =>
        {
            HideMessage();
        }));
    }

    public void ShowMessage(string text, float hideDelay = 3)
    {
        if (_coroutineHideMessage != null)
        {
            StopCoroutine(_coroutineHideMessage);
            _coroutineHideMessage = null;
        }
        _messageText.text = text;
        _messageImage.gameObject.SetActive(true);
        HideMessageDelay(hideDelay);
    }

    public void ShowModeText(bool bodyMode)
    {
        _tip2Text.text = bodyMode ? "Attention" : "Relax";
    }
}
