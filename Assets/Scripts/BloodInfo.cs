using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BloodInfo
{
    public enum BloodType { None, Blood, Platelet, Plasma};
    public enum BloodFamily { None, APos, BPos, OPos, ANeg, BNeg, ONeg}

    public BloodType type = BloodType.None;
    public BloodFamily family = BloodFamily.None;

    public bool Equals(BloodInfo info)
    {
        return family == info.family && type == info.type;
    }
}
