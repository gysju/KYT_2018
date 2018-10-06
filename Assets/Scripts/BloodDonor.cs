using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDonor : DragableObj
{
    public BloodInfo Blood;
    private Rigidbody _rgd;

    public enum State { home, medic, taking, leave }
    public State state = State.home;

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

    public override void Attach(Transform parent)
    {
        base.Attach(parent);
        transform.localPosition = Vector3.zero;
        GetComponent<Collider>().isTrigger = true;

        _rgd.useGravity = false;
        _rgd.isKinematic = true;
    }

    public override void Detach()
    {
        base.Detach();
        GetComponent<Collider>().isTrigger = false;

        _rgd.useGravity = true;
        _rgd.isKinematic = false;
    }
}
