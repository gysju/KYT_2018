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

    private List<BloodInfo> ask, given;
    private int _remaining;

    private List<int> _nBagnextCommands = new List<int>();

    private BloodInfo.Compatibilities[] _compts, _givenCompts;

    private MeshRenderer[] _meshRenderers;
    public MeshRenderer[] meshRenderers { get { return _meshRenderers; } }
    #endregion
    #region MonoFunction
    private void Start()
    {
        _data = GameManager.Instance.RequestData();
        Init();

        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }
    #endregion
    #region Function
    public void AddBag(BloodBag bag)
    {
        AddAnswer(bag.bloodInfo);
        //Destroy(bag.gameObject);
        bag.gameObject.SetActive(false);
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
            BloodInfo info = GameManager.Instance.BloodInfoGetRand();
            _compts[(int)info.type - 1].Increase(info.Compatibility());
            ask.Add(info);

            bordsNeeds[i].SetData(true, sprites[(int)info.type - 1], "" + info.family);
        }

        for (int i = nBag; i < Commands.maxQuantity; i++)
            bordsNeeds[i].SetData(false, null, null);

        for (int i = 0; i < Commands.maxQuantity; i++)
            bordsGiven[i].SetData(false, null, null);

        _remaining = nBag;
    }

    public void AddAnswer(BloodInfo answer)
    {
        if (_compts[(int)answer.type - 1].ababo[(int)answer.family - 1] <= _givenCompts[(int)answer.type - 1].ababo[(int)answer.family - 1])
        {
            Debug.Log("lose command");
            Generate();
        }
        else
        {
            given.Add(answer);
            bordsGiven[given.Count - 1].SetData(true, sprites[(int)answer.type - 1], "" + answer.family);
            CanvasManager.Instance.AddScore(_data.ScoreByCommandPartiallyComplete);
            _compts[(int)answer.type - 1].ababo[(int)answer.family - 1]++;
            _remaining--;
            if (_remaining <= 0)
            {
                _nBagnextCommands.RemoveAt(0);
                Debug.Log("command completed");
                _truck.SetNeedGo();
                CanvasManager.Instance.AddScore(_data.ScoreByCommandComplete);
                Generate();
            }
        }
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
    #endregion
}