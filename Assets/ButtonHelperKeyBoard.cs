using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHelperKeyBoard : ButtonHelper, IDeselectHandler
{
    [SerializeField] private Color _selected = Color.white, _unselected = new Color(1, .5390625f, .23046875f, 1);

    private Text _text;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SetTextColor(_selected);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetTextColor(_unselected);
    }

    private void SetTextColor(Color color)
    {
        if (_text == null)
            _text = GetComponentInChildren<Text>();

        _text.color = color;
    }
}
