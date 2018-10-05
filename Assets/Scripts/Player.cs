using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _playerID;
    [SerializeField] private Transform _attrachAnchor;
    [SerializeField] private float _speed;

    private Rigidbody _rgd;
    private BloodDonor _currentDonorAttached = null;
    private bool _hasBeenAttachedCheck = false;

    private void Awake()
    {
        _rgd = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_currentDonorAttached != null)
        {
            if (Input.GetButtonUp("x" + _playerID))
            {
                _hasBeenAttachedCheck = true;
            }
            else if (Input.GetButtonDown("x" + _playerID) && _hasBeenAttachedCheck)
            {
                DetachDonor();
            }
        }
    }

    void FixedUpdate ()
    {
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

    private void AttachDonor( BloodDonor donor)
    {
        donor.transform.parent = _attrachAnchor;
        donor.transform.localPosition = Vector3.zero;
        donor.GetComponent<Collider>().isTrigger = true;

        donor.GetRigidbody().useGravity = false;
        donor.GetRigidbody().isKinematic = true;

        _currentDonorAttached = donor;
    }

    private void DetachDonor()
    {
        _currentDonorAttached.transform.parent = null;
        _currentDonorAttached.GetComponent<Collider>().isTrigger = false;

        _currentDonorAttached.GetRigidbody().useGravity = true;
        _currentDonorAttached.GetRigidbody().isKinematic = false;

        _currentDonorAttached = null;
        _hasBeenAttachedCheck = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Donor") && Input.GetButtonDown("X" + _playerID ))
        {
            AttachDonor(other.GetComponent<BloodDonor>());
        }
    }
}
