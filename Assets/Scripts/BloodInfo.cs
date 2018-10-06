using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BloodInfo
{
    public enum BloodType { None, Blood, Platelet, Plasma};
    public enum BloodFamily { None, A, B, AB, O}
    public enum BloodRhesus { None, neg, pos }

    public BloodType type = BloodType.None;
    public BloodFamily family = BloodFamily.None;
    public BloodRhesus rhesus = BloodRhesus.None;

    public bool Equals(BloodInfo info)
    {
        return family == info.family && type == info.type && rhesus == info.rhesus;
    }
    public bool EqualsType(BloodInfo info)
    {
        return type == info.type;
    }
    public bool EqualsFamRhe(BloodInfo info)
    {
        return family == info.family && rhesus == info.rhesus;
    }
    public bool EqualsFam(BloodInfo info)
    {
        return family == info.family;
    }
    public bool EqualsRhe(BloodInfo info)
    {
        return rhesus == info.rhesus;
    }
}
