using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Compatibility : MonoBehaviour {

    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private TextMeshProUGUI _type;
    [SerializeField] private Image[] _images;

    private int[] _blood = { 1,1,1,1,
                             1,0,1,0,
                             1,1,0,0,
                             1,0,0,0 };
    private int[] _plasma = { 0,0,0,1,
                              0,0,1,1,
                              0,1,0,1,
                              1,1,1,1 };

    public void SetCompatibility(BloodInfo.BloodType type)
    {
        _type.text = type.ToString();

        Sprite sprite = _sprites[(int)type - 1];

        int[] compt;
        if (type == BloodInfo.BloodType.Plasma)
            compt = _plasma;
        else compt = _blood;

        for (int i = 0; i < compt.Length; i++)
        {
            _images[i].gameObject.SetActive(compt[i] == 1);
            _images[i].sprite = sprite;
        }
    }

    private float _openTime = .7f, _fadeOutTime = .3f, _increaseOpenTime = .1f, _openDelay = .2f;
    private Sequence _inTweenOpen;
    [SerializeField] private RectTransform _rect;

    public void Open()
    {
        _inTweenOpen.Kill();
        _rect.DOAnchorPos(new Vector2(250, _rect.anchoredPosition.y), 0);
        _inTweenOpen = DOTween.Sequence();
        _inTweenOpen.Append(_rect.DOAnchorPos(new Vector2(-54, _rect.anchoredPosition.y) , _openTime));
    }
    public void Close()
    {
        _inTweenOpen.Kill();
        _inTweenOpen = DOTween.Sequence();
        _inTweenOpen.Append(_rect.DOAnchorPos(new Vector2(250, _rect.anchoredPosition.y), _openTime * .6f));
    }
}
