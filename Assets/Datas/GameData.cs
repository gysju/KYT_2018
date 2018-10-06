using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "GameData", order = 1)]
public class GameData : ScriptableObject
{
    public Color IdleColor;
    public float MaxIdleTime;

    [Space(10)]
    public Color MedicColor;
    public float MaxMedicTime;

    [Space(10)]
    public Color RageQuitColor;
}
