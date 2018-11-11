using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BloodDonor : DragableObj
{
    public BloodInfo bloodInfo;
    private GameData _data;
    [SerializeField] private FillIcon _waitingFill;

    private Player _grabBy;

    [HideInInspector] public bool onProcess;
    [HideInInspector] public int progressionStat;
    public enum State { idle, home, medic, taking, leave, rageQuit }
    public State state {
        get {
            return _state;
        }
        set {
            OnStateExit();
            _state = value;
            if (gameObject.activeSelf)
                OnStateEnter();
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private State _state = State.idle;

    private Rigidbody _rgd;
    [HideInInspector] public NavMeshAgent _navMeshAgent;
    private float _currentIdle = 0.0f;

    private List<Transform> _currentPath;
    private int _currentPathIndex = 0;
    public Animator animator;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _rageQuit;

    [HideInInspector] public bool desableKinematic = true;   

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rgd = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Start ()
    {
        state = State.home;

        if (_data.HumansDatas != null)
        {
            SetModels(_data.HumansDatas[Random.Range(0, _data.HumansDatas.Count)]);
            bloodInfo = GameManager.inst.BloodInfoGetRand();

            _waitingFill.SetProgressionColor(_data.MedicColor);
        }
    }
	
	void Update ()
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            if (TimeManager.paused && !_navMeshAgent.isStopped)
            {
                animator.SetFloat("Speed", 0.0f);
                _navMeshAgent.isStopped = true;
            }
            else if (!TimeManager.paused && _navMeshAgent.isStopped)
                _navMeshAgent.isStopped = false;
        }

        if (GameManager.inst.State != GameManager.GameState.InGame)
            return;

        if (state == State.idle)
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
        switch (state)
        {
            case State.idle:
                _currentPath = null;
                //_navMeshAgent.isStopped = true;
                //_waitingBar.color = _data.IdleColor;
                break;
            case State.home:
                _currentPath = PathManager.Instance.PathsHomeToDoc;
                SetDestination(_currentPath[0].position);
                break;
            case State.medic:
                _waitingFill.SetProgressionColor(_data.TakingBloodColor);
                break;
            case State.taking:
                break;
            case State.leave:
                _currentPath = PathManager.Instance.PathsBedToExit;
                SetDestination(_currentPath[0].position);
                _waitingFill.SetProgressionColor(_data.IdleColor);
                _waitingFill.Fill(1.0f);
                break;
            case State.rageQuit:
                onProcess = false;
                _currentPath = PathManager.Instance.PathsBedToExit;
                SetDestination(_currentPath[0].position);
                _waitingFill.SetProgressionColor(_data.RageQuitColor);
                _currentIdle = .5f;
                _waitingFill.Fill(1.0f);

                _audioSource.clip = _rageQuit;
                _audioSource.loop = false;
                _audioSource.pitch = Random.Range(0.7f, 1.0f);
                _audioSource.Play();
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (state)
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
        switch (state)
        {
            case State.idle:
                _currentIdle = 0.0f;
                break;
            case State.home:
                progressionStat = (int)state;
                break;
            case State.medic:
                progressionStat = (int)state;               
                break;
            case State.taking:
                progressionStat = (int)state;
                break;
            case State.leave:
                progressionStat = (int)state;
                GameManager.inst.Removedonor(this);
                gameObject.SetActive(false);
                break;
            case State.rageQuit:
                GameManager.inst.Removedonor(this);
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
            state = State.rageQuit;
        }
        _currentIdle += TimeManager.deltaTime;
        SetHudBar();
    }

    private void SetHudBar()
    {
        switch (state)
        {
            case State.idle:
                _waitingFill.Fill(_currentIdle / _data.MaxIdleTime);
                break;
            case State.home:
                Debug.Log("fill home");
                break;
            case State.rageQuit:
                _waitingFill.Fill(1.0f);
                break;
            default:
                _waitingFill.SetActive(false);
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
                state = State.idle;
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

    public void DesableWaintingFill()
    {
        _waitingFill.SetActive(false);
    }
}
