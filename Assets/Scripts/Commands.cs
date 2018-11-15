using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : MonoBehaviour {

    #region Var
    public Bord[] bordsNeeds, bordsGiven;
    public Sprite[] sprites;
    private GameData _data;

    public static int maxQuantity = 4;

    [SerializeField] private Truck _truck;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _soundSuccess = null, _soundLose = null;

    private List<BloodInfo> ask, given;
    private int _remaining;

    private List<int> _nBagnextCommands = new List<int>();

    private BloodInfo.Compatibilities[] _compts, _givenCompts;

    private MeshRenderer[] _meshRenderers;
    public MeshRenderer[] meshRenderers { get { return _meshRenderers; } }

    [SerializeField] private GameObject _given;

    #endregion
    #region MonoFunction
    private void Start()
    {
        _data = GameManager.inst.RequestData();
        Init();

        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }
    #endregion
    #region Function
    public int[] AddBag(BloodBag bag)
    {
        int[] score = AddAnswer(bag.bloodInfo);
        //Destroy(bag.gameObject);
        bag.gameObject.SetActive(false);

        return score;
    }
    public void ResetCommand()
    {
        Init();
    }

    public void Init()
    {
        InitListCommands();
        Generate();
    }

    public void Generate()
    {
        int nBag = Commands.maxQuantity;
        if (_nBagnextCommands.Count > 0)
        {
            nBag = _nBagnextCommands[0];
        }

        ask = new List<BloodInfo>();
        given = new List<BloodInfo>();
        _compts = new BloodInfo.Compatibilities[] {
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Blood),
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Platelet),
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Plasma)
        };
        _givenCompts = new BloodInfo.Compatibilities[] {
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Blood),
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Platelet),
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Plasma)
        };

        for (int i = 0; i < nBag; i++)
        {
            BloodInfo info = GameManager.inst.BloodInfoGetRand();
            _compts[(int)info.type - 1].Increase(info.Compatibility());
            ask.Add(info);

            bordsNeeds[i].SetData(true, sprites[(int)info.type - 1], "" + info.family);
        }

        for (int i = nBag; i < Commands.maxQuantity; i++)
            bordsNeeds[i].SetData(false, null, null);

        for (int i = 0; i < Commands.maxQuantity; i++)
            bordsGiven[i].SetData(false, null, null);

        _remaining = nBag;

        _given.SetActive(false);
    }

    public int[] AddAnswer(BloodInfo answer)
    {
        _given.SetActive(true);

        int[] score = { -1, -1 };
        if (_compts[(int)answer.type - 1].ababo[(int)answer.family - 1] <= _givenCompts[(int)answer.type - 1].ababo[(int)answer.family - 1])
        {
            Debug.Log("lose command");
            PlaySound(_soundLose);
            Generate();
            score[0] = -1;
        }
        else
        {
            given.Add(answer);
            bordsGiven[given.Count - 1].SetData(true, sprites[(int)answer.type - 1], "" + answer.family);
            score[0] = _data.ScoreByCommandPartiallyComplete;
            CanvasManager.inst.AddScore(score[0]);
            _compts[(int)answer.type - 1].ababo[(int)answer.family - 1]++;
            _remaining--;

            if (_remaining <= 0)
            {
                _nBagnextCommands.RemoveAt(0);
                Debug.Log("command completed");
                PlaySound(_soundSuccess);
                _truck.SetNeedGo();
                score[1] = _data.ScoreByCommandComplete;
                CanvasManager.inst.AddScore(score[1]);
                Generate();
            }
        }
        return score;
    }

    private void InitListCommands()
    {
        _nBagnextCommands = new List<int>();

        _nBagnextCommands.Add(2);
        _nBagnextCommands.Add(2);
        _nBagnextCommands.Add(2);
        _nBagnextCommands.Add(3);
        _nBagnextCommands.Add(3);
        _nBagnextCommands.Add(3);
        _nBagnextCommands.Add(3);
        _nBagnextCommands.Add(3);
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
    #endregion
}