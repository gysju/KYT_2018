﻿using System.Collections;
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

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        _rgd = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameManager.GameState.InGame)
            return;

        if (_currentObjAttached != null)
        {
            if (Input.GetButtonUp("A"+_playerID)/*Input.GetKeyUp("joystick " + (_playerID+1) + " button 0")*/)
            {
                _hasBeenAttachedCheck = true;

            }
            else if (Input.GetButtonDown("A" + _playerID)/*Input.GetKeyDown("joystick " + (_playerID+1) + " button 0")*/ && _hasBeenAttachedCheck)
            {
                CheckOnDrop();
                DetachObj();
            }
        }
        else if (Input.GetButtonDown("A" + _playerID)/*Input.GetKeyDown("joystick " + (_playerID+1) + " button 0")*/)
        {
            //CheckOnDrop();
            TryGrab();
        }
    }

    void FixedUpdate ()
    {

        if (GameManager.Instance.State != GameManager.GameState.InGame)
            return;

        HighlightManagement();

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

            _rgd.AddForce(transform.forward * _speed * TimeManager.deltaTime, ForceMode.VelocityChange);

            _animator.SetFloat("Speed", Mathf.Clamp01( Mathf.Abs( hAxis ) + Mathf.Abs(vAxis)));
        }
        else
        {
            _animator.SetFloat("Speed", 0.0f);
        }
    }

    private void AttachObj(DragableObj obj, Transform anchor)
    {
        obj.Attach(anchor, this);

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
                if (donor.state != BloodDonor.State.taking && donor.state != BloodDonor.State.leave && donor.state != BloodDonor.State.rageQuit)
                {
                    AttachObj(donor, _attrachAnchor);
                    SetBul(true, sprites[(int)donor.bloodInfo.type - 1], "" + donor.bloodInfo.family);
                    ResetHighlightAndBul(false);
                }
            }
            else if (cols[0].CompareTag("Bloodbag"))
            {
                BloodBag bloodbag = (cols[0].GetComponent<BloodBag>());
                AttachObj(bloodbag, _grabCenter);
                SetBul(true, sprites[(int)bloodbag.bloodInfo.type - 1], "" + bloodbag.bloodInfo.family);
                ResetHighlightAndBul(false);
            }
            else if (cols[0].CompareTag("FoodBag"))
            {
                FoodBag bloodbag = (cols[0].GetComponent<FoodBag>());
                AttachObj(bloodbag, _grabCenter);
                ResetHighlightAndBul(true);
            }
        }
        else
        {
            cols = Physics.OverlapSphere(_grabCenter.position, .75f, _interactPlace);
            if (cols != null && cols.Length > 0)
            {
                if (cols[0].CompareTag("BloodShelf") || cols[0].CompareTag("FoodShelf"))
                {
                    Shelf shelf = cols[0].GetComponent<Shelf>();
                    if (shelf != null)
                    {
                        DragableObj b = shelf.TakeOut();
                        if (b != null)
                        {
                            AttachObj(b, _grabCenter);
                            if (b is BloodBag)
                            {
                                BloodBag bloodBag = ((BloodBag)b);
                                SetBul(true, sprites[(int)bloodBag.bloodInfo.type - 1], "" + bloodBag.bloodInfo.family);
                            }
                        }
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
                        if (bd.progressionStat == (int)BloodDonor.State.home || bd.state == BloodDonor.State.home)
                        {
                            if (!doc.occupied)
                                doc.SetDonor(bd);
                        }
                        else bd.state = BloodDonor.State.rageQuit;
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
                        if ((bd.progressionStat == (int)BloodDonor.State.medic || bd.state == BloodDonor.State.medic) /*&& bed.type == bd.Blood.type*/)
                        {
                            if (!bed.occupied)
                                bed.SetDonor(bd);
                        }
                        else bd.state = BloodDonor.State.rageQuit;
                    }
                }
                if (_currentObjAttached is FoodBag)
                {
                    if (bed.occupied)
                    {
                        bed.TryFeed(_currentObjAttached);
                    }
                }
            }
            else if (cols[0].CompareTag("BloodShelf") || cols[0].CompareTag("FoodShelf"))
            {
                Shelf shelf = cols[0].GetComponent<Shelf>();
                if (shelf != null)
                    shelf.FillIn(_currentObjAttached);
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

        ResetHighlightAndBul(true);
    }

    private void HighlightManagement()
    {
        Collider[] cols = Physics.OverlapSphere(_grabCenter.position, .75f, _interactObj | _interactPlace);

        if (cols.Length <= 0)
        {
            ResetHighlightAndBul(true);
            return;
        }

        Collider col = cols[0];
        if (_currentObjAttached != null && col.transform == _currentObjAttached.transform && cols.Length > 1)
            col = cols[1];

        if (col.CompareTag("Donor"))
        {
            if (_currentObjAttached == null)
            {
                BloodDonor donor = (col.GetComponent<BloodDonor>());
                if (donor != null)
                    SetBul(true, sprites[(int)donor.bloodInfo.type - 1], "" + donor.bloodInfo.family);
                _lastHighlightObj.SetData(col, donor.meshRenderers, true);
            }
            else if (_lastHighlightObj.col != null && _lastHighlightObj.col.transform != col.transform)
                ResetHighlightAndBul(false);
        }
        else if (col.CompareTag("Bloodbag"))
        {
            if (_currentObjAttached == null)
            {
                BloodBag bloodbag = (col.GetComponent<BloodBag>());
                if (bloodbag != null)
                    SetBul(true, sprites[(int)bloodbag.bloodInfo.type - 1], "" + bloodbag.bloodInfo.family);
                _lastHighlightObj.SetData(col, bloodbag.meshRenderers, false);
            }
            else if (_lastHighlightObj.col != null && _lastHighlightObj.col.transform != col.transform)
                ResetHighlightAndBul(false);
        }
        else if (col.CompareTag("BloodShelf"))
        {
            BloodShelf shelf = col.GetComponent<BloodShelf>();
            if (_currentObjAttached == null || _currentObjAttached is BloodBag)
            {
                if (_currentObjAttached == null)
                    SetBul(true, "" + shelf.GetNumber());
                _lastHighlightObj.SetData(col, shelf.meshRenderers, false);
            } else ResetHighlightAndBul(false);
        }
        else if (col.CompareTag("FoodBag"))
        {
            if (_currentObjAttached == null)
                _lastHighlightObj.SetData(col, col.GetComponent<MeshRenderer>().material, false);
            else if (_lastHighlightObj.col != null && _lastHighlightObj.col.transform != col.transform)
                ResetHighlightAndBul(true);
        }
        else if (col.CompareTag("FoodShelf"))
        {
            FoodShelf shelf = col.GetComponent<FoodShelf>();
            if (_currentObjAttached == null || _currentObjAttached is FoodBag)
                _lastHighlightObj.SetData(col, shelf.meshRenderers, false);
            else ResetHighlightAndBul(false);
        }
        else if (col.CompareTag("Doctor_Door") || col.CompareTag("Bed"))
        {
            InteractableWihDonor interactableWihDonor = col.GetComponent<InteractableWihDonor>();
            if (_currentObjAttached != null && _currentObjAttached is BloodDonor)
            {
                BloodDonor donor = (BloodDonor)_currentObjAttached;
                if (/*donor.state != BloodDonor.State.taking &&*/ donor.state != BloodDonor.State.leave && donor.state != BloodDonor.State.rageQuit)
                    _lastHighlightObj.SetData(col, interactableWihDonor.meshRenderers, false);
            } 
            else if (_currentObjAttached != null && _currentObjAttached is FoodBag)
            {
                if (interactableWihDonor.occupied)
                    _lastHighlightObj.SetData(col, interactableWihDonor.meshRenderers, false);
            }
            else ResetHighlightAndBul(false);
        }
        else if (col.CompareTag("commands"))
        {
            Commands commands = col.GetComponent<Commands>();
            if (_currentObjAttached != null && _currentObjAttached is BloodBag)
                _lastHighlightObj.SetData(col, commands.meshRenderers, false);
        } else ResetHighlightAndBul(false);
    }

    public void ResetHighlightAndBul(bool bulTo)
    {
        _lastHighlightObj.SetHighLight(0);
        _lastHighlightObj.col = null;
        if (bulTo)
            SetBul(false, null);
    }

    private struct HighlightObj
    {
        public Material[] mats;
        public Collider col;
        public bool isAnObj;

        public HighlightObj(Collider col, Material mat, bool isAnObj)
        {
            this.mats = new Material[0];
            this.col = col;
            this.isAnObj = isAnObj;
        }

        public void SetData(Collider col, Material mat, bool isAnObj)
        {
            if (this.col != null)
                SetHighLight(0);

            this.mats = new Material[] { mat };
            this.col = col;
            this.isAnObj = isAnObj;

            SetHighLight(1);
        }

        public void SetData(Collider col, MeshRenderer[] renderers, bool isAnObj)
        {
            if (this.col != null)
                SetHighLight(0);

            if (renderers == null) return;

            this.mats = new Material[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
                mats[i] = renderers[i].material;
            this.col = col;
            this.isAnObj = isAnObj;

            SetHighLight(1);
        }

        public void SetHighLight(float value)
        {
            if (mats.Length > 0)
                for (int i = 0; i < mats.Length; i++)
                    mats[i].SetFloat("_Highlight", value);
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
    public void Clear()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        
        _hasBeenAttachedCheck = false;
        _bul.SetActive(false);
    }
    public void ForceDrop()
    {
        _currentObjAttached = null;
    }
}