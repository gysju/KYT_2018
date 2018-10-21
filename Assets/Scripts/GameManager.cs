using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int numberOfLevel = 2;

    public static GameManager Instance;
    public enum GameState { Paused, Menu, InGame}
    public GameState State
    {
        get { return _state;}
        set { _state = value;}
    }

    private GameState _state;

    [Header("GameSettings")]
    [SerializeField] private GameData _data;

    [Header("Blood donor settings")]
    [SerializeField] private BloodDonor _bloodDonor;
    [SerializeField] private Transform _bdSpawn;

    List<BloodDonor> _bloodDonors = new List<BloodDonor>();
    public Player _playerOne;
    public Player _playerTwo;

    public BloodShelf[] shelves;
    public InteractableWihDonor[] interactableWihDonors;

    private List<BloodBag> bloodBags = new List<BloodBag>();

    public Commands command;

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
        State = GameState.Menu;
    }

	void Update ()
    {
        if ((Input.GetButtonDown("Start0") || Input.GetButtonDown("Start1")) && State != GameState.Menu)
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
        {
            BloodDonor donor = Instantiate(_bloodDonor, _bdSpawn.position, _bdSpawn.rotation);
            donor.SetData(_data);
            _bloodDonors.Add(donor);
        }

        StartCoroutine(SpawnBloodDonor());
    }

    public void Removedonor( BloodDonor donor)
    {
        _bloodDonors.Remove(donor);
    }

    public void ClearAllInstance()
    {
        for (int i = 0; i < interactableWihDonors.Length; i++)
            interactableWihDonors[i].ResetObj();

        foreach (BloodDonor donor in _bloodDonors)
        {
            Destroy(donor);
            Destroy(donor.gameObject);
        }

        _bloodDonors.Clear();

        _playerOne.Clear();
        _playerTwo.Clear();

        for (int i = 0; i < shelves.Length; i++)
            shelves[i].ResetStock();

        if (bloodBags != null)
        {
            foreach (BloodBag b in bloodBags)
            {
                if (b != null)
                    Destroy(b.gameObject);
            }
            bloodBags.Clear();
        }

        if (command != null)
            command.ResetCommand();
    }

    public void AddBag(BloodBag b)
    {
        bloodBags.Add(b);
    }

    public GameData RequestData()
    {
        return _data;
    }

    public BloodInfo BloodInfoGetRand()
    {
        BloodInfo.BloodType type = _data.bloodTypes[Random.Range(0, _data.bloodTypes.Length)];
        BloodInfo.BloodFamily fam = _data.bloodFamilies[Random.Range(0, _data.bloodFamilies.Length)];
        BloodInfo.BloodRhesus rhe = (BloodInfo.BloodRhesus)Random.Range(1, 3);

        return new BloodInfo(type, fam, rhe);
    }
}
