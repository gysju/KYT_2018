using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : MonoBehaviour {

    #region Var
    public Command command;
    #endregion
    #region MonoFunction
    private void Start()
    {
        command = new Command(4);
    }
    #endregion
    #region Function
    public void AddBag(BloodBag bag)
    {
        command.AddAnswer(bag.bloodInfo);
        Destroy(bag.gameObject);
    }
    #endregion
}

[System.Serializable]
public class Command
{
    public List<BloodInfo> ask, given;
    public int remaining;

    public BloodInfo.Compatibilities[] compts, givenCompts;

    public Command(int nBag)
    {
        Generate(nBag);
    }

    public void Generate(int nBag)
    {
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
        }

        remaining = nBag;
    }

    public void AddAnswer(BloodInfo answer)
    {
        Debug.Log("a: " + compts[(int)answer.type - 1].ababo[(int)answer.family - 1] + "; b: " + givenCompts[(int)answer.type - 1].ababo[(int)answer.family - 1]);
        if (compts[(int)answer.type - 1].ababo[(int)answer.family - 1] <= givenCompts[(int)answer.type - 1].ababo[(int)answer.family - 1])
        {
            Debug.Log("lose command");
            Generate(4);
        }
        else
        {
            given.Add(answer);
            compts[(int)answer.type - 1].ababo[(int)answer.family - 1]++;
            remaining--;
            if (remaining <= 0)
                Debug.Log("command completed");
        }
    }
}