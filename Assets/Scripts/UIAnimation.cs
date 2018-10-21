using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour {

    [SerializeField] private Image _background;
    [SerializeField] private Selectable _firstSelectable;
    [SerializeField] private RectTransform[] _objects;
    private float _openTime = .7f, _fadeOutTime = .3f, _increaseOpenTime = .1f, _openDelay = .2f;
    private Sequence _inTweenOpen;

    public void OpenMainMenu(bool firstTime = false)
    {
        OpenMainMenu(null, firstTime);
    }
    public void OpenMainMenu(TweenCallback callback, bool firstTime = false)
    {
        ResetOpen(_objects);
        _inTweenOpen = DOTween.Sequence();

        if (firstTime) _background.DOFade(1, 0);
        else _inTweenOpen.Append(_background.DOFade(1, _fadeOutTime));

        _inTweenOpen.AppendCallback(callback);
        _inTweenOpen.Append(_background.DOFade(0, _fadeOutTime));

        OpenMenu(_inTweenOpen, _objects).Play();
    }
    public void Open()
    {
        ResetOpen(_objects);
        OpenMenu(_inTweenOpen, _objects);
    }
    public void Close()
    {
        _inTweenOpen.Kill();
        _inTweenOpen = DOTween.Sequence();
        CloseMenu(_inTweenOpen, _objects);
    }
    public void OpenSubMenu()
    {
        _inTweenOpen.Kill();
        _inTweenOpen = DOTween.Sequence();
        _inTweenOpen.AppendInterval((_openTime + _increaseOpenTime) * .6f + .1f);
        _inTweenOpen.AppendCallback(() => { _firstSelectable.Select(); });
        OpenMenu(_inTweenOpen, _objects);
    }

    private void ResetOpen(RectTransform[] rects)
    {
        _inTweenOpen.Kill();
        for (int i = 0; i < rects.Length; i++)
            rects[i].DOAnchorPos(new Vector2(-250, rects[i].anchoredPosition.y), 0);
    }
    private Sequence OpenMenu(Sequence sequence, RectTransform[] rects)
    {
        for (int i = 0; i < rects.Length;  i++)
            sequence.Join(rects[i].DOAnchorPos(new Vector2(0, rects[i].anchoredPosition.y), _openTime + _increaseOpenTime * i));
        return sequence;
    }

    private Sequence CloseMenu(Sequence sequence, RectTransform[] rects)
    {
        for (int i = 0; i < rects.Length; i++)
            sequence.Join(rects[i].DOAnchorPos(new Vector2(-250, rects[i].anchoredPosition.y), (_openTime + _increaseOpenTime * i) * .6f));
        return sequence;
    }

    private void ButtonSetInteractable(Button[] buttons, bool value)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = value;
    }
}
