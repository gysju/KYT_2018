using DG.Tweening;
using UnityEngine;
using TMPro;

public class UIAddScore : MonoBehaviour {

    [SerializeField] private Vector3 staticAngle = new Vector3(0, 137.446f, 0);
    private Sequence[] _inTweens;
    [SerializeField] private RectTransform[] _scores;
    [SerializeField] private TextMeshProUGUI[] _scoresText;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _volume = .8f;
    [SerializeField] private AudioClip _clipSource;


    private float _upTime = 0.6f;

    private Transform _transform;

	void Start ()
    {
        _transform = GetComponent<Transform>();

        _inTweens = new Sequence[_scores.Length];
        for (int i = 0; i < _scores.Length; i++)
            _inTweens[i] = DOTween.Sequence();
    }
	
	void Update ()
    {
        _transform.eulerAngles = staticAngle;
    }

    public void NewScore(int value, float interval = 0)
    {
        int index = GetIndex();
        RectTransform rect = _scores[index];
        _scoresText[index].text = "+" + value;

        _inTweens[index].Kill();
        rect.DOAnchorPos(new Vector2(rect.anchoredPosition.x, -.5f), 0);
        rect.gameObject.SetActive(true);
        _inTweens[index] = DOTween.Sequence();
        _inTweens[index].AppendInterval(interval + 0.02f);
        _inTweens[index].AppendCallback(() => { _audioSource.clip = _clipSource; _audioSource.volume = _volume; _audioSource.Play(); });
        _inTweens[index].Append(rect.DOAnchorPos(new Vector2(rect.anchoredPosition.x, .5f), _upTime));
        _inTweens[index].AppendCallback( () => { rect.gameObject.SetActive(false); } );
    }

    private int GetIndex()
    {
        int index = -1;
        for (int i = 0; i < _inTweens.Length; i++)
            if (!_inTweens[i].IsPlaying())
                index = i;
        if (index == -1)
            index = 0;

        return index;
    }

    public void NewScore(int[] value)
    {
        NewScore(value[0]);
        if (value[1] > 0)
            NewScore(value[1], _upTime * .4f);
    }
}
