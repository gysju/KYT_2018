using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodInfo
{
    public enum BloodType { None, Blood, Platelet};
    public enum BloodFamily { None, APos, BPos, OPos, ANeg, BNeg, ONeg}

    public BloodType EBloodType = BloodType.None;
    public BloodFamily EBloodFamily = BloodFamily.None;
}
