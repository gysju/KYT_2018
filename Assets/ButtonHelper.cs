using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHelper : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerClickHandler
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
}
