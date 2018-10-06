using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : MonoBehaviour {

    #region Var

    #endregion
    #region MonoFunction

    #endregion
    #region Function

    #endregion
}

public struct Command
{
    public List<BloodInfo> ask, given;
    public int remaining;

    public Vector4 ababo;

    public Command(int nBag)
    {
        ask = new List<BloodInfo>();
        given = new List<BloodInfo>();
        ababo = new Vector4();

        for (int i = 0; i < nBag; i++)
        {
            BloodInfo info = BloodInfo.GetRand();

            ask.Add(info);
        }

        remaining = nBag;
    }

    public void AddAnswer(BloodInfo answer)
    {
        bool compt = false;

        foreach (BloodInfo a in ask)
        {
            if (!compt && answer.Equals(a, true, true, false))
            {
                compt = true;
            }
        }

        if (compt)
        {
            //add
        }
    }
}