using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image _tip;

    public void Init()
    {
        HideTip();
    }

    public void ShowTip()
    {
        _tip.gameObject.SetActive(true);
    }

    public void HideTip()
    {
        _tip.gameObject.SetActive(false);
    }
}
