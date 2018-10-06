using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _playerID;
    [SerializeField] private Transform _attrachAnchor;
    [SerializeField] private float _speed;
    [SerializeField] private Collider _trigger;

    [SerializeField] private LayerMask _interactObj;
    [SerializeField] private LayerMask _interactPlace;

    private Rigidbody _rgd;
    private DragableObj _currentObjAttached = null;
    private bool _hasBeenAttachedCheck = false;

    private void Awake()
    {
        _rgd = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.InGame)
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
        if (GameManager.Instance.GetGameState() != GameManager.GameState.InGame)
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

            _rgd.AddForce( transform.forward * _speed * Time.deltaTime, ForceMode.VelocityChange);
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
        Collider[] cols = Physics.OverlapSphere(_attrachAnchor.position, .75f, _interactObj);
        if (cols != null && cols.Length > 0)
        {
            if (cols[0].CompareTag("Donor"))
            {
                BloodDonor donor = (cols[0].GetComponent<BloodDonor>());
                if (donor.CurrentState != BloodDonor.State.taking)
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
            cols = Physics.OverlapSphere(_attrachAnchor.position, .75f, _interactPlace);
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
        Collider[] cols = Physics.OverlapSphere(_attrachAnchor.position, .75f, _interactPlace);
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
                        if (bd.CurrentState == BloodDonor.State.home)
                            doc.SetDonor((BloodDonor)_currentObjAttached);
                        else bd.CurrentState = BloodDonor.State.leave;
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
                        if (bd.CurrentState == BloodDonor.State.medic /*&& bed.type == bd.Blood.EBloodType*/)
                        {
                            bed.SetDonor(bd);
                        }
                        else bd.CurrentState = BloodDonor.State.leave;
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
        }
    }
}
