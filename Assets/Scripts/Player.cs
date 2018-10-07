using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _playerID;
    [SerializeField] private Transform _attrachAnchor;
    [SerializeField] private Transform _grabCenter;
    [SerializeField] private float _speed;

    [SerializeField] private LayerMask _interactObj;
    [SerializeField] private LayerMask _interactPlace;

    [SerializeField] private AudioClip AttachedSound;
    [SerializeField] private AudioClip DetachedSound;

    [SerializeField] private GameObject _bul;
    [SerializeField] private UnityEngine.UI.Image _bulImg;
    [SerializeField] private TextMeshProUGUI _bulText, _bulText_Img;

    [SerializeField] private Sprite[] sprites;

    private AudioSource Source;
    private Rigidbody _rgd;
    private DragableObj _currentObjAttached = null;
    private bool _hasBeenAttachedCheck = false;

    private HighlightObj _lastHighlightObj = new HighlightObj(null, null, false);

    private Animator _animator;

    public Vector3 startPosition;
    public Quaternion startRotation;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        _rgd = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HighlightTargetObj();

        if (GameManager.Instance.State != GameManager.GameState.InGame)
            return;

        if (_currentObjAttached != null)
        {
            if (Input.GetKeyUp("joystick " + (_playerID+1) + " button 0"))
            {
                _hasBeenAttachedCheck = true;

            }
            else if (Input.GetKeyDown("joystick " + (_playerID+1) + " button 0") && _hasBeenAttachedCheck)
            {
                CheckOnDrop();
                DetachObj();

            }
        }
        else if (Input.GetKeyDown("joystick " + (_playerID+1) + " button 0"))
        {
            CheckOnDrop();
            TryGrab();
        }
    }

    void FixedUpdate ()
    {
        if (GameManager.Instance.State != GameManager.GameState.InGame)
            return;

        Move();
    }

    private void Move()
    {
        float hAxis = Input.GetAxis("Horizontal" + _playerID);
        float vAxis = Input.GetAxis("Vertical" + _playerID);

        if (hAxis != 0.0f || vAxis != 0.0f)
        {
            float angle = Mathf.Atan2(vAxis, -hAxis) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            _rgd.AddForce(transform.forward * _speed * Time.deltaTime, ForceMode.VelocityChange);

            _animator.SetFloat("Speed", Mathf.Clamp01( Mathf.Abs( hAxis ) + Mathf.Abs(vAxis)));
        }
        else
        {
            _animator.SetFloat("Speed", 0.0f);
        }
    }

    private void AttachObj(DragableObj obj)
    {
        obj.Attach(_attrachAnchor);

        _currentObjAttached = obj;
        Source.clip = AttachedSound;
        Source.Play();
    }

    private void DetachObj()
    {
        _currentObjAttached.Detach();

        _currentObjAttached = null;
        _hasBeenAttachedCheck = false;
        Source.clip = DetachedSound;
        Source.Play();
    }

    private void TryGrab()
    {
        Collider[] cols = Physics.OverlapSphere(_grabCenter.position, .75f, _interactObj);
        if (cols != null && cols.Length > 0)
        {
            if (cols[0].CompareTag("Donor"))
            {
                BloodDonor donor = (cols[0].GetComponent<BloodDonor>());
                if (donor.CurrentState != BloodDonor.State.taking && donor.CurrentState != BloodDonor.State.rageQuit)
                    AttachObj(donor);
            }
            else if (cols[0].CompareTag("Bloodbag"))
            {
                BloodBag bloodbag = (cols[0].GetComponent<BloodBag>());
                AttachObj(bloodbag);
            }
        }
        else
        {
            cols = Physics.OverlapSphere(_grabCenter.position, .75f, _interactPlace);
            if (cols != null && cols.Length > 0)
            {
                if (cols[0].CompareTag("BloodShelf"))
                {
                    BloodShelf shelf = cols[0].GetComponent<BloodShelf>();
                    if (shelf != null)
                    {
                        BloodBag b = shelf.TakeOut();
                        if (b != null)
                            AttachObj(b);
                    }
                }
            }
        }
    }

    /// <summary>Check surounding object around the player</summary>
    private void CheckOnDrop()
    {
        Collider[] cols = Physics.OverlapSphere(_grabCenter.position, .75f, _interactPlace);
        if (cols != null && cols.Length > 0)
        {
            if (cols[0].CompareTag("Doctor_Door"))
            {
                Doctor doc = cols[0].GetComponent<Doctor>();
                if (doc != null && _currentObjAttached is BloodDonor)
                {
                    BloodDonor bd = (BloodDonor)_currentObjAttached;
                    if (bd != null)
                    {
                        if (bd.progressionStat == (int)BloodDonor.State.home || bd._state == BloodDonor.State.home)
                            doc.SetDonor((BloodDonor)_currentObjAttached);
                        else bd.CurrentState = BloodDonor.State.rageQuit;
                    }
                }
            }
            else if (cols[0].CompareTag("Bed"))
            {
                Bed bed = cols[0].GetComponent<Bed>();
                if (bed != null && _currentObjAttached is BloodDonor)
                {
                    BloodDonor bd = (BloodDonor)_currentObjAttached;
                    if (bd != null)
                    {
                        if ((bd.progressionStat == (int)BloodDonor.State.medic || bd._state == BloodDonor.State.medic) /*&& bed.type == bd.Blood.type*/)
                        {
                            bed.SetDonor(bd);
                        }
                        else bd.CurrentState = BloodDonor.State.rageQuit;
                    }
                }
            }
            else if (cols[0].CompareTag("BloodShelf"))
            {
                BloodShelf shelf = cols[0].GetComponent<BloodShelf>();
                if (shelf != null && _currentObjAttached is BloodBag)
                {
                    BloodBag bb = (BloodBag)_currentObjAttached;
                    if (bb != null)
                    {
                        shelf.FillIn(bb);
                    }
                }
            }
            else if (cols[0].CompareTag("commands"))
            {
                Commands commands = cols[0].GetComponent<Commands>();
                if (commands != null && _currentObjAttached is BloodBag)
                {
                    BloodBag bb = (BloodBag)_currentObjAttached;
                    if (bb != null)
                    {
                        commands.AddBag(bb);
                    }
                }
            }
            else if (_currentObjAttached is BloodDonor)
                ((BloodDonor)_currentObjAttached).onProcess = false;
        } else if (_currentObjAttached is BloodDonor)
            ((BloodDonor)_currentObjAttached).onProcess = false;
    }

    private void HighlightTargetObj()
    {
        Collider[] cols = Physics.OverlapSphere(_grabCenter.position, .75f, _interactObj);
        if (cols != null && cols.Length > 0)
        {
            if (_lastHighlightObj.col != cols[0])
            {
                if (_lastHighlightObj.col != null)
                    _lastHighlightObj.mat.SetFloat("_Highlight", 0);
                _lastHighlightObj.SetData(cols[0], cols[0].GetComponentInChildren<MeshRenderer>().material, true);
                _lastHighlightObj.mat.SetFloat("_Highlight", 1);

                if (cols[0].CompareTag("Donor"))
                {
                    BloodDonor donor = (cols[0].GetComponent<BloodDonor>());
                    if (donor != null)
                        SetBul(true, sprites[(int)donor.Blood.type - 1], "" + donor.Blood.family);
                }
                else if (cols[0].CompareTag("Bloodbag"))
                {
                    BloodBag bloodbag = (cols[0].GetComponent<BloodBag>());
                    if (bloodbag != null)
                        SetBul(true, sprites[(int)bloodbag.bloodInfo.type -1], "" + bloodbag.bloodInfo.family);
                }
            }
        }
        else
        {
            cols = Physics.OverlapSphere(_grabCenter.position, .75f, _interactPlace);
            if (cols != null && cols.Length > 0)
            {
                if (_lastHighlightObj.col != null)
                    _lastHighlightObj.mat.SetFloat("_Highlight", 0);
                _lastHighlightObj.SetData(cols[0], cols[0].GetComponentInChildren<MeshRenderer>().material, false);
                _lastHighlightObj.mat.SetFloat("_Highlight", 1);

                if (_lastHighlightObj.col.CompareTag("BloodShelf"))
                {
                    BloodShelf shelf = _lastHighlightObj.col.GetComponent<BloodShelf>();
                    if (shelf != null && _currentObjAttached == null)
                    {
                        SetBul(true, ""+shelf.GetNumber());
                    }
                }
            }
            else if (_lastHighlightObj.col != null)
            {
                _lastHighlightObj.mat.SetFloat("_Highlight", 0);
                _lastHighlightObj.col = null;

                if (_bul.activeSelf)
                    SetBul(false, "");
            }
        }
    }

    private struct HighlightObj
    {
        public Material mat;
        public Collider col;
        public bool isAnObj;

        public HighlightObj(Collider col, Material mat, bool isAnObj)
        {
            this.mat = mat;
            this.col = col;
            this.isAnObj = isAnObj;
        }

        public void SetData(Collider col, Material mat, bool isAnObj)
        {
            this.mat = mat;
            this.col = col;
            this.isAnObj = isAnObj;
        }
    }

    public void SetBul(bool active, string text)
    {
        _bul.SetActive(active);
        if (!string.IsNullOrEmpty(text))
            _bulText.text = text;

        _bulImg.gameObject.SetActive(false);
        _bulText_Img.text = "";
    }
    public void SetBul(bool active, Sprite sprite, string text)
    {
        _bul.SetActive(active);
        if (active)
        {
            _bulImg.sprite = sprite;
            _bulImg.gameObject.SetActive(true);
        } else _bulImg.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(text))
        {
            _bulText_Img.text = text;
            _bulText.text = "";
        }
    }
    public void ResetPosition()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}