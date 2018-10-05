using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDonor : MonoBehaviour
{
    public BloodInfo Blood;

    private Rigidbody _rgd;

    private void Awake()
    {
        _rgd = GetComponent<Rigidbody>();
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public Rigidbody GetRigidbody()
    {
        return _rgd;
    }
}
