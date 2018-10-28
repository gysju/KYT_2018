using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConstData", menuName = "ConstGameData", order = 1)]
public class ConstGameData : ScriptableObject
{
    [Header("Rules")]
    public int ScoreByCommandComplete = 25;
    public int ScoreByCommandPartiallyComplete = 5;
    public int ScoreByBloodStocked = 2;
    public int ScoreByFoodGiven = 2;

    [Header("HUD")]
    [Space(10)]

    public Color IdleColor;
    public float MaxIdleTime;

    [Space(10)]
    public Color MedicColor;
    public float MaxMedicTime;

    [Space(10)]
    public Color TakingBloodColor;
    public float MaxTakingBloodTime;
    [Space(10)]

    public Color TakingPlasmaColor;
    public float MaxTakingPlasmaTime;
    [Space(10)]

    public Color TakingPlateletColor;
    public float MaxTakingPlateletTime;

    [Space(10)]
    public Color RageQuitColor;

    [Header("Humans")]
    public List<GameData.HumanData> HumansDatas = new List<GameData.HumanData>();
}
