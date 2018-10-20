using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour {

    [SerializeField] private Image _background;
    [SerializeField] private RectTransform[] _buttons;
    public float openTime = .7f;


    private void Start()
    {
        EnterAnim();
    }

    public void EnterAnim()
    {
        _background.CrossFadeAlpha(.5f, .3f, false);
        OpenMenu(_buttons);
    }

    private Sequence inTweenOpen;
    private void OpenMenu(RectTransform[] rects)
    {
        inTweenOpen = DOTween.Sequence();
        for (int i = 0; i < rects.Length;  i++)
        {
            inTweenOpen.Join(rects[i].DOAnchorPos(new Vector2(-250, rects[i].anchoredPosition.y), 0));
            inTweenOpen.Join(rects[i].DOAnchorPos(new Vector2(0, rects[i].anchoredPosition.y), openTime + .1f * i));
            inTweenOpen.SetDelay(.1f * i);
        }        
    }

}
