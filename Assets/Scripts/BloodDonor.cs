using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BloodDonor : DragableObj
{
    public BloodInfo Blood;

    public enum State { idle, home, medic, taking, leave }
    public State CurrentState {
        get {
            return _state;
        }
        set {
            OnStateExit();
            _state = value;
            OnStateEnter();
        }
    }
    public State _state = State.idle;

    private Rigidbody _rgd;
    private NavMeshAgent _navMeshAgent;

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

    void OnStateEnter()
    {
        switch (CurrentState)
        {
            case State.idle:
                break;
            case State.home:
                break;
            case State.medic:
                break;
            case State.taking:
                break;
            case State.leave:
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (CurrentState)
        {
            case State.idle:
                break;
            case State.home:
                break;
            case State.medic:
                break;
            case State.taking:
                break;
            case State.leave:
                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {
        switch (CurrentState)
        {
            case State.idle:
                break;
            case State.home:
                break;
            case State.medic:
                break;
            case State.taking:
                break;
            case State.leave:
                break;
            default:
                break;
        }
    }

    public void SetDestination( Vector3 destination )
    {
        _navMeshAgent.SetDestination(destination);
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
