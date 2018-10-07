using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { Paused, Menu, InGame}
    public GameState State
    {
        get { return _state;}
        set { _state = value;}
    }

    private GameState _state;

    [Header("GameSettings")]
    [SerializeField] GameData _data;

    [Header("Blood donor settings")]
    [SerializeField] private BloodDonor _bloodDonor;
    [SerializeField] private Transform _bdSpawn;

    List<BloodDonor> _bloodDonors = new List<BloodDonor>();
    public Player _playerOne;
    public Player _playerTwo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        StartCoroutine( SpawnBloodDonor());
    }

	void Update ()
    {
        if (Input.GetButtonDown("Start0") || Input.GetButtonDown("Start1"))
        {
            if (State == GameState.InGame)
            {
                CanvasManager.Instance.DisplayPauseMenu();
            }
            else if (State == GameState.Paused)
            {
                CanvasManager.Instance.HidePauseMenu();
            }
        }
	}

    IEnumerator SpawnBloodDonor()
    {
        while (State != GameState.InGame)
        {
            yield return null;
        }

        if (_bloodDonors.Count > 0)
        {
            yield return new WaitForSeconds(_data.SpawnSpeed);
        }

        if(_bloodDonors.Count < _data.MaxHumanCount)
            _bloodDonors.Add( Instantiate( _bloodDonor, _bdSpawn.position, _bdSpawn.rotation));

        StartCoroutine(SpawnBloodDonor());
    }

    public void Removedonor( BloodDonor donor)
    {
        _bloodDonors.Remove(donor);
    }

    public void ClearAllInstance()
    {
        for (int i = 0; i < _bloodDonors.Count; i++)
        {
            Destroy(_bloodDonors[i].gameObject);
        }

        _bloodDonors.Clear();

        _playerOne.ResetPosition();
        _playerTwo.ResetPosition();
    }
}
