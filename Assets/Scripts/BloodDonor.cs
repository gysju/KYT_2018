using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BloodDonor : DragableObj
{
    public BloodInfo Blood;
    [SerializeField] private GameData _data;

    public enum State { idle, home, medic, taking, leave, rageQuit }
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
    private float _currentIdle = 0.0f;

    private List<Transform> _currentPath;
    private int _currentPathIndex = 0;

    private void Awake()
    {
        _rgd = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start ()
    {
        CurrentState = State.home;
    }
	
	void Update ()
    {
        OnStateUpdate();
        HasReachedHisDestination();
    }

    void OnStateEnter()
    {
        switch (CurrentState)
        {
            case State.idle:
                _currentPath = null;
                _navMeshAgent.isStopped = true;
                break;
            case State.home:
                _currentPath = PathManager.Instance.PathsHomeToDoc;
                SetDestination(_currentPath[0].position);
                break;
            case State.medic:
                break;
            case State.taking:
                break;
            case State.leave:
                _currentPath = PathManager.Instance.PathsBedToExit;
                SetDestination(_currentPath[0].position);
                break;
            case State.rageQuit:
                _currentPath = PathManager.Instance.PathsBedToExit;
                SetDestination(_currentPath[0].position);
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
                CheckRageQuit();
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
                _currentIdle = 0.0f;
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

    void CheckRageQuit()
    {
        if ( _currentIdle > _data.MaxIdleTime)
        {
            CurrentState = State.rageQuit;
        }
        _currentIdle += Time.deltaTime;
    }

    void HasReachedHisDestination()
    {
        if (_currentPath == null)
            return;

        float dist = _navMeshAgent.remainingDistance;
        if (dist != Mathf.Infinity && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && _navMeshAgent.remainingDistance == 0)
        {
            _currentPathIndex ++;
            if (_currentPathIndex >= _currentPath.Count)
            {
                _currentPathIndex = 0;
                CurrentState = State.idle;
            }
            else
            {
                SetDestination(_currentPath[_currentPathIndex].position);
            }
        }
    }

    public void SetDestination( Vector3 destination )
    {
        _navMeshAgent.isStopped = false;
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
