﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : MonoBehaviour {

    #region Var
    public Command command;
    
    #endregion
    #region MonoFunction
    private void Start()
    {
        command = new Command();
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

public class Command
{
    public List<BloodInfo> ask, given;
    public int remaining;

    private List<int> nBagnextCommands = new List<int>();

    public BloodInfo.Compatibilities[] compts, givenCompts;

    public Command()
    {
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
        }

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
                Debug.Log("command completed");
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