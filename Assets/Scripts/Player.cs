using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _playerID;
    [SerializeField] private Transform _attrachAnchor;
    [SerializeField] private Transform _grabCenter;
    [SerializeField] private float _speed;

    [SerializeField] private LayerMask _interactObj;
    [SerializeField] private LayerMask _interactPlace;

    private Rigidbody _rgd;
    private DragableObj _currentObjAttached = null;
    private bool _hasBeenAttachedCheck = false;

    private HighlightObj _lastHighlightObj = new HighlightObj(null, null, false);

    private Animator _animator;

    private void Awake()
    {
        _rgd = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HighlightTargetObj();

        if (GameManager.Instance.State != GameManager.GameState.InGame)
            return;

        if (_currentObjAttached != null)
        {
            if (Input.GetButtonUp("X" + _playerID))
            {
                _hasBeenAttachedCheck = true;
            }
            else if (Input.GetButtonDown("X" + _playerID) && _hasBeenAttachedCheck)
            {
                CheckOnDrop();
                DetachObj();
            }
        }
        else if (Input.GetButtonDown("X" + _playerID))
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
    }

    private void DetachObj()
    {
        _currentObjAttached.Detach();

        _currentObjAttached = null;
        _hasBeenAttachedCheck = false;
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
                        if ((bd.progressionStat == (int)BloodDonor.State.medic || bd._state == BloodDonor.State.medic) && bed.type == bd.Blood.type)
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
            } else if (_currentObjAttached is BloodDonor)
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
            }
            else if (_lastHighlightObj.col != null)
            {
                _lastHighlightObj.mat.SetFloat("_Highlight", 0);
                _lastHighlightObj.col = null;
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
}