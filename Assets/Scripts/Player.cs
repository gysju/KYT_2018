using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform _attrachAnchor;
    [SerializeField] private float _speed;

    private Rigidbody _rgd;
    private BloodDonor _currentDonorAttached = null;

    private void Awake()
    {
        _rgd = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate ()
    {
        Move();

        if (Input.GetButtonUp("Fire1"))
        {
            DetachDonor();
        }
    }

    private void Move()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Debug.Log(hAxis);

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
        donor.GetComponent<Rigidbody>().isKinematic = true;
        _currentDonorAttached = donor;
    }

    private void DetachDonor()
    {
        _currentDonorAttached.transform.parent = null;
        _currentDonorAttached.GetComponent<Rigidbody>().isKinematic = false;

        _currentDonorAttached = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Donor") && Input.GetButtonDown("Fire1"))
        {
            AttachDonor(other.GetComponent<BloodDonor>());
        }
    }
}
