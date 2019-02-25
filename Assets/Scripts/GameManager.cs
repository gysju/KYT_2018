using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static string username_path = "username.txt";

    public int levelIndex = -1;
    public const int numberOfLevel = 5;
    public const string buildVersion = "0.9.1";

    public static GameManager inst;
    public enum GameState { Paused, Menu, InGame, GameOver }
    public GameState State
    {
        get { return _state;}
        set { _state = value;}
    }

    private GameState _state;

    [Header("GameSettings")]
    [SerializeField] private ConstGameData _constData = null;
    [SerializeField] private LevelData _levelData = null;
    private GameData _data;

    [Header("Blood donor settings")]
    [SerializeField] private BloodDonor _bloodDonor = null;
    [SerializeField] private Transform _bdSpawn = null;

    List<BloodDonor> _bloodDonors = new List<BloodDonor>();
    public Player _playerOne = null;
    public Player _playerTwo = null;

    public BloodShelf[] shelves = null;
    public InteractableWihDonor[] interactableWihDonors = null;

    private List<BloodBag> bloodBags = new List<BloodBag>();

    public Commands command = null;

    [SerializeField] CallCenter _callCenter = null;

    [SerializeField] private TextMeshProUGUI _scoreText = null;
    [SerializeField] private TextMeshProUGUI _timerText = null;

    private int _calledDonorCount = 0;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            _data = new GameData(_constData, _levelData);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        CanvasManager.inst.LevelLoaded();
        StartCoroutine( SpawnBloodDonor());
        //State = GameState.Menu;

        if (levelIndex < 0)
            Debug.LogError("levelIndex have to be set");
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

        if(_bloodDonors.Count < _data.MaxHumanCount + _calledDonorCount)
        {
            BloodDonor donor = Instantiate(_bloodDonor, _bdSpawn.position, _bdSpawn.rotation);
            donor.Init(_data, BloodInfoGetRand());
            _bloodDonors.Add(donor);
        }

        StartCoroutine(SpawnBloodDonor());
    }

    public void Removedonor( BloodDonor donor)
    {
        if (donor.called)
            _calledDonorCount--;
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

        _callCenter.ResetCallCenter();
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
        BloodInfo.BloodType type = _data.BloodTypes[Random.Range(0, _data.BloodTypes.Length)];
        BloodInfo.BloodFamily fam = _data.BloodFamilies[Random.Range(0, _data.BloodFamilies.Length)];
        BloodInfo.BloodRhesus rhe = (BloodInfo.BloodRhesus)Random.Range(1, 3);

        return new BloodInfo(type, fam, rhe);
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public TextMeshProUGUI[] GetHUDText()
    {
        return new TextMeshProUGUI[] { _timerText, _scoreText };
    }

    public void CallDonor(BloodInfo.BloodType type, BloodInfo.BloodFamily familly)
    {
        _calledDonorCount += _data.NumberOfDonorByCall;
        for (int i = 0; i < _data.NumberOfDonorByCall; i++)
        {
            BloodDonor donor = Instantiate(_bloodDonor, _bdSpawn.position, _bdSpawn.rotation);
            donor.Init(_data, new BloodInfo(type, familly, BloodInfo.BloodRhesus.None));
            donor.called = true;
            _bloodDonors.Add(donor);
        }
    }
}
