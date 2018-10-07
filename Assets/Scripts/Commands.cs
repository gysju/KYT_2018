using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : MonoBehaviour {

    #region Var
    public Command command;

    public Bord[] bords;
    public Sprite[] sprites;
    #endregion
    #region MonoFunction
    private void Start()
    {
        for (int i = 4; i < 4; i++)
            bords[i].SetData(false, null, null);

        command = new Command(bords, sprites);
    }
    #endregion
    #region Function
    public void AddBag(BloodBag bag)
    {
        command.AddAnswer(bag.bloodInfo);
        Destroy(bag.gameObject);
    }
    public void ResetCommand()
    {
        command.Init();
    }
    #endregion
}

public class Command
{
    public List<BloodInfo> ask, given;
    public int remaining;

    private List<int> nBagnextCommands = new List<int>();

    public BloodInfo.Compatibilities[] compts, givenCompts;

    public Bord[] bords;
    public Sprite[] sprites;

    public Command(Bord[] bords, Sprite[] sprites)
    {
        this.bords = bords;
        this.sprites = sprites;
        Init();
    }

    public void Init()
    {
        InitListCommands();
        Generate();
    }

    public void Generate()
    {
        int nBag = 4;
        if (nBagnextCommands.Count > 0)
        {
            nBag = nBagnextCommands[0];
            nBagnextCommands.RemoveAt(0);
        }

        ask = new List<BloodInfo>();
        given = new List<BloodInfo>();
        compts = new BloodInfo.Compatibilities[] {
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Blood),
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Platelet),
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Plasma)
        };
        givenCompts = new BloodInfo.Compatibilities[] {
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Blood),
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Platelet),
            new BloodInfo.Compatibilities(BloodInfo.BloodType.Plasma)
        };

        for (int i = 0; i < nBag; i++)
        {
            BloodInfo info = BloodInfo.GetRand();
            compts[(int)info.type - 1].Increase(info.Compatibility());
            ask.Add(info);

            bords[i].SetData(true, sprites[(int)info.type - 1], "" + info.family);
        }

        for (int i = nBag; i < 4; i++)
            bords[i].SetData(false, null, null);

        remaining = nBag;
    }

    public void AddAnswer(BloodInfo answer)
    {
        Debug.Log("a: " + compts[(int)answer.type - 1].ababo[(int)answer.family - 1] + "; b: " + givenCompts[(int)answer.type - 1].ababo[(int)answer.family - 1]);
        if (compts[(int)answer.type - 1].ababo[(int)answer.family - 1] <= givenCompts[(int)answer.type - 1].ababo[(int)answer.family - 1])
        {
            Debug.Log("lose command");
            Generate();
        }
        else
        {
            given.Add(answer);
            compts[(int)answer.type - 1].ababo[(int)answer.family - 1]++;
            remaining--;
            if (remaining <= 0)
            {
                Debug.Log("command completed");
                CanvasManager.Instance.AddScore(10);
                Generate();
            }
        }
    }

    private void InitListCommands()
    {
        nBagnextCommands = new List<int>();

        nBagnextCommands.Add(2);
        nBagnextCommands.Add(2);
        nBagnextCommands.Add(2);
        nBagnextCommands.Add(3);
        nBagnextCommands.Add(3);
        nBagnextCommands.Add(3);
        nBagnextCommands.Add(3);
        nBagnextCommands.Add(3);
        nBagnextCommands.Add(4);
        nBagnextCommands.Add(4);
        nBagnextCommands.Add(4);
    }
}