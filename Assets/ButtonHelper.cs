using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerClickHandler, IPointerEnterHandler
{
    private AudioSource _source;

    private bool _onSelected;

    public virtual void OnSelect(BaseEventData eventData)
    {
        _onSelected = true;
        PlaySound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_onSelected)
            _onSelected = false;
        else PlaySound();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlaySound();
    }

    public void PlaySound()
    {
        if (_source == null)
            _source = GetComponentInParent<AudioSource>();

        _source.pitch = Random.Range(0.7f, 1);
        _source.loop = false;
        _source.Play();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Selectable s = GetComponent<Selectable>();
        if (s != null)
            s.Select();
    }
}
