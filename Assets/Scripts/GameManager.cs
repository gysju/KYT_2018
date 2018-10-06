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
        SpawnBloodDonor();
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

    void SpawnBloodDonor()
    {
        _bloodDonors.Add( Instantiate( _bloodDonor, _bdSpawn.position, _bdSpawn.rotation));
    }
}
