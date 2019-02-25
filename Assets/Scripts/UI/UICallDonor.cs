using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICallDonor : MonoBehaviour
{
    [SerializeField] float _inViewY = 0, _outOfViewY = 12.5f;
    [SerializeField] float _transitionDuration = .4f;
    [SerializeField] RectTransform _typeParent = null, _famillyParent = null, _timerParent = null;

    [SerializeField] UICustomButton[] _typeSelectables = null, _famillySelectables = null;
    UICustomButton _crtSelected = null;

    [SerializeField] CallCenter _callCenter = null;

    enum States { close, type, familly }
    States _state;

    Sequence _inTween = null;

    int _playerId = -1;

    bool showType = true;

    bool haveMove = false;
    float min = .8f, max = .3f; //to navigate

    BloodInfo.BloodType _bloodType;
    BloodInfo.BloodFamily _bloodFamily;

    [SerializeField] AudioClip[] _clips = null;
    [SerializeField] AudioClip _phoneCall = null;
    AudioSource[] _sources = null;

    void Start()
    {
        GameData data = GameManager.inst.RequestData();
        showType = data.BloodTypes.Length > 1;
        if (!showType)
        {
            _bloodType = data.BloodTypes[0];
            SetColor((int)data.BloodTypes[0] - 1);
        }

        _sources = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (_playerId == -1 || _state == States.close)
            return;

        if (Input.GetButtonDown("B" + _playerId))
        {
            switch (_state)
            {
                case States.familly:
                    if (showType)
                        OpenType();
                    else
                    {
                        Close();
                        _callCenter.LiberatePlayer();
                    }
                    break;
                case States.type:
                    Close();
                    _callCenter.LiberatePlayer();
                    break;
            }
        }
        else if (Input.GetButtonDown("A" + _playerId))
            _crtSelected.InvokeEvent();
        else if (!haveMove)// navigation
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal" + _playerId)) < max) // check vertical input
            {
                if (Input.GetAxis("Vertical" + _playerId) < -min) //down
                    Navigate(2);
                else if (Input.GetAxis("Vertical" + _playerId) > min) //up
                    Navigate(0);
            }
            else if (Mathf.Abs(Input.GetAxis("Vertical" + _playerId)) < max) // check horizontal input
            {
                if (Input.GetAxis("Horizontal" + _playerId) > min) //right
                    Navigate(1);
                else if (Input.GetAxis("Horizontal" + _playerId) < -min) //left
                    Navigate(3);
            }
        }
        else if (haveMove && Mathf.Abs(Input.GetAxis("Horizontal" + _playerId)) < max && Mathf.Abs(Input.GetAxis("Vertical" + _playerId)) < max)
             haveMove = false;
    }

    void Navigate(int index)
    {
        haveMove = true;
        if (_crtSelected.navigation[index])
            Select(_crtSelected.navigation[index]);
    }

    void Select(UICustomButton button)
    {
        if (!button.gameObject.activeSelf)
            return;

        if (_crtSelected != null)
            _crtSelected.OnDiselect();
        _crtSelected = button;
        _crtSelected.OnSelect();
    }

    public void Open(int playerId = -1)
    {
        if (playerId != -1)
            _playerId = playerId;

        if (showType)
            OpenType();
        else OpenFamilly();
    }

    public void OpenType(bool resetTween = true)
    {
        if (resetTween)
            ResetTween();

        if (_state == States.familly)
        {
            _inTween.Join(_famillyParent.DOAnchorPosY(-_outOfViewY, _transitionDuration * .5f));
            _typeParent.anchoredPosition = new Vector2(_typeParent.anchoredPosition.x, _outOfViewY);
        } else _typeParent.anchoredPosition = new Vector2(_typeParent.anchoredPosition.x, -_outOfViewY);

        _state = States.type;

        Select(_typeSelectables[0]);
        _inTween.Append(_typeParent.DOAnchorPosY(0, _transitionDuration));

        _inTween.Play();
    }

    public void OpenFamilly(bool resetTween = true)
    {
        if (resetTween)
            ResetTween();

        if (_state == States.type)
        {
            _inTween.Join(_typeParent.DOAnchorPosY(_outOfViewY, _transitionDuration * .5f));

        }

        _state = States.familly;

        _famillyParent.anchoredPosition = new Vector2(_famillyParent.anchoredPosition.x, -_outOfViewY);
        Select(_famillySelectables[0]);
        _inTween.Append(_famillyParent.DOAnchorPosY(_inViewY, _transitionDuration));

        _inTween.Play();
    }

    public void Close()
    {
        ResetTween();

        if (_state == States.familly)
            _inTween.Append(_famillyParent.DOAnchorPosY(-_outOfViewY, _transitionDuration * .5f));
        else if (_state == States.type)
            _inTween.Append(_typeParent.DOAnchorPosY(-_outOfViewY, _transitionDuration * .5f));

        _state = States.close;

        _inTween.AppendCallback(() => { ResetPosition(); });

        _inTween.Play();

        _playerId = -1;        
    }

    void ResetPosition()
    {
        _typeParent.anchoredPosition = new Vector2(_typeParent.anchoredPosition.x, -_outOfViewY);
        _famillyParent.anchoredPosition = new Vector2(_famillyParent.anchoredPosition.x, -_outOfViewY);
    }

    void ResetTween()
    {
        _inTween.Kill();
        _inTween = DOTween.Sequence();
    }

    public void SetColor(int id)
    {
        for (int i = 0; i < _famillySelectables.Length; i++)
            _famillySelectables[i].SetColors(_typeSelectables[id].normal, _typeSelectables[id].highlight);
    }

    public void SelectType(int id)
    {
        SetColor(id);

        _bloodType = (BloodInfo.BloodType)(id + 1);

        OpenFamilly();
    }

    public void ApplyCall(int id) //blood type
    {
        _bloodFamily = (BloodInfo.BloodFamily)(id + 1);

        Close();

        _callCenter.Call(_bloodType, _bloodFamily);
        ShowHideTimer(true);

        PlaySound(_phoneCall);
    }

    public void ShowHideTimer(bool show, bool resetTween = true)
    {
        //if (resetTween)
            //ResetTween();

        //_timerParent.anchoredPosition = new Vector2(_timerParent.anchoredPosition.x, _outOfViewY);
        _inTween.Append(_timerParent.DOAnchorPosY(show ? _inViewY : _outOfViewY, _transitionDuration));
        _inTween.AppendCallback(() => { ResetPosition(); });
        _inTween.Play();
    }
    public void PlaySound(AudioClip clip)
    {
        int index = 0;
        if (_sources[index].isPlaying)
            index = 1;

        _sources[index].clip = clip;
        _sources[index].Play();
    }
    public void PlayBipTouch()
    {
        PlaySound(_clips[Random.Range(0, _clips.Length)]);
    }
}
