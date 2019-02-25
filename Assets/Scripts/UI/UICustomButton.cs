using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UICustomButton : MonoBehaviour
{
    [SerializeField] Image _image = null;

    [Header("Colors : ")]
    [Space(10)]
    public Color normal;
    public Color highlight;

    [Space(10)]
    public UICustomButton[] navigation = null; //up right down left

    [Space(10)]
    public UnityEvent onClick = null;

    UICallDonor UICallDonor;

    public void OnSelect()
    {
        _image.color = highlight;

        if (UICallDonor == null)
            UICallDonor = GetComponentInParent<UICallDonor>();
        UICallDonor.PlayBipTouch();
    }
    public void OnDiselect()
    {
        _image.color = normal;
    }
    public void SetColors(Color normal, Color highlight)
    {
        this.normal = normal;
        this.highlight = highlight;

        _image.color = normal;
    }
    public void InvokeEvent()
    {
        onClick.Invoke();
        UICallDonor.PlayBipTouch();
    }
}