﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BloodDonor : DragableObj
{
    public BloodInfo bloodInfo;
    private GameData _data;
    [SerializeField] private Image _waitingBar;

    [HideInInspector] public bool onProcess;
    [HideInInspector] public int progressionStat;
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
            else Destroy(gameObject);
        }
    }
    public State _state = State.idle;

    private Rigidbody _rgd;
    [HideInInspector] public NavMeshAgent _navMeshAgent;
    private float _currentIdle = 0.0f;

    private List<Transform> _currentPath;
    private int _currentPathIndex = 0;
    public Animator animator;

    [HideInInspector] public bool desableKinematic = true;   

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
            bloodInfo = GameManager.Instance.BloodInfoGetRand();
        }
    }
	
	void Update ()
    {
        if (GameManager.Instance.State != GameManager.GameState.InGame)
            return;

        if (CurrentState == State.idle)
        {
            animator.SetFloat("Speed", 0.0f);
        }
        else
        {
            animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        }

        if (!onProcess)
        {
            OnStateUpdate();
            HasReachedHisDestination();
        }
    }

    private void SetModels(GameData.HumanData humanData)
    {
        GetComponentInChildren<MeshFilter>().mesh = humanData.Model;
        GetComponentInChildren<MeshRenderer>().material = humanData.Mat;
    }

    public void SetData(GameData data)
    {
        _data = data;
    }

    private void OnStateEnter()
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

    private void OnStateUpdate()
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

    private void OnStateExit()
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
                GameManager.Instance.Removedonor(this);
                gameObject.SetActive(false);
                break;
            case State.rageQuit:
                GameManager.Instance.Removedonor(this);
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void CheckRageQuit()
    {
        if ( _currentIdle > _data.MaxIdleTime)
        {
            CurrentState = State.rageQuit;
        }
        _currentIdle += Time.deltaTime;
        SetHudBar();
    }

    private void CheckMedicTime()
    {
        if (_currentIdle > _data.MaxMedicTime)
        {
            CurrentState = State.idle;
        }
        _currentIdle += Time.deltaTime;
        SetHudBar();
    }

    private void SetHudBar()
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

    private void HasReachedHisDestination()
    {
        if (_currentPath == null)
            return;

        if (!_navMeshAgent.enabled) return;

        float dist = (_navMeshAgent.destination - transform.position).magnitude;
        if (dist != Mathf.Infinity && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && dist <= 0.25)
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
        _navMeshAgent.enabled = true;
        
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

        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.SetDestination(transform.position);
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
        }

        onProcess = true;

        //_rgd.useGravity = false;
        _rgd.isKinematic = true;
    }

    public override void Detach()
    {
        base.Detach();
        GetComponent<Collider>().isTrigger = false;

        if (desableKinematic)
        {
            _rgd.isKinematic = false;
            _navMeshAgent.enabled = true;
        }
    }
}
