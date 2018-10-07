using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BloodDonor : DragableObj
{
    public BloodInfo Blood;
    [SerializeField] private GameData _data;
    [SerializeField] private Image _waitingBar;

    /*[HideInInspector]*/ public bool onProcess;
    /*[HideInInspector]*/ public int progressionStat;
    public enum State { idle, home, medic, taking, leave, rageQuit }
    public State CurrentState {
        get {
            return _state;
        }
        set {
            OnStateExit();
            _state = value;
            if (gameObject.activeSelf)
                OnStateEnter();
            else
                Destroy(gameObject);
        }
    }
    public State _state = State.idle;

    private Rigidbody _rgd;
    [HideInInspector] public NavMeshAgent _navMeshAgent;
    private float _currentIdle = 0.0f;

    private List<Transform> _currentPath;
    private int _currentPathIndex = 0;
    public Animator animator;

    private void Awake()
    {
        _rgd = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Start ()
    {
        CurrentState = State.home;

        if (_data.HumansDatas != null)
        {
            SetModels(_data.HumansDatas[Random.Range(0, _data.HumansDatas.Count)]);
        }
    }
	
	void Update ()
    {
        if (GameManager.Instance.State != GameManager.GameState.InGame)
            return;

        if (!onProcess)
        {
            OnStateUpdate();
            HasReachedHisDestination();
        }

        if ( CurrentState == State.idle)
        {
            animator.SetFloat("Speed", 0.0f);
        }
        else
        {
            animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        }
    }

    void SetModels(GameData.HumanData humanData)
    {
        GetComponentInChildren<MeshFilter>().mesh = humanData.Model;
        GetComponentInChildren<MeshRenderer>().material = humanData.Mat;
    }

    void OnStateEnter()
    {
        switch (CurrentState)
        {
            case State.idle:
                _currentPath = null;
                _navMeshAgent.isStopped = true;
                _waitingBar.color = _data.IdleColor;
                break;
            case State.home:
                _currentPath = PathManager.Instance.PathsHomeToDoc;
                SetDestination(_currentPath[0].position);
                break;
            case State.medic:
                _waitingBar.color = _data.MedicColor;
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
                _waitingBar.color = _data.RageQuitColor;
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
                progressionStat = (int)CurrentState;
                break;
            case State.medic:
                progressionStat = (int)CurrentState;
                break;
            case State.taking:
                progressionStat = (int)CurrentState;
                break;
            case State.leave:
                progressionStat = (int)CurrentState;
                break;
            case State.rageQuit:
                GameManager.Instance.Removedonor(this);
                gameObject.SetActive(false);
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
        SetHudBar();
    }

    void CheckMedicTime()
    {
        if (_currentIdle > _data.MaxMedicTime)
        {
            CurrentState = State.idle;
        }
        _currentIdle += Time.deltaTime;
        SetHudBar();
    }

    void SetHudBar()
    {
        switch (CurrentState)
        {
            case State.medic:
                _waitingBar.fillAmount = _currentIdle / _data.MaxMedicTime;
                break;
            case State.idle:
            case State.home:
                _waitingBar.fillAmount = _currentIdle / _data.MaxIdleTime;
                break;
            case State.rageQuit:
                _waitingBar.fillAmount = 1.0f;
                break;
            default:
                _waitingBar.fillAmount = 0.0f;
                break;
        }
    }

    void HasReachedHisDestination()
    {
        if (_currentPath == null)
            return;

        if (!_navMeshAgent.enabled) return;

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
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destination);
        }
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

        _navMeshAgent.SetDestination(transform.position);
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;

        onProcess = true;

        _rgd.useGravity = false;
        _rgd.isKinematic = true;
    }

    public override void Detach()
    {
        base.Detach();
        GetComponent<Collider>().isTrigger = false;

        if (!onProcess)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(transform.position);
            _navMeshAgent.isStopped = false;
        }

        _rgd.useGravity = true;
        _rgd.isKinematic = false;
    }
}
