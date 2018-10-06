using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Rules")]
    public float GameDuration = 10.0f;
    [HideInInspector] public float Score = 0.0f;

    [Header("HUD")]
    [Space(10)]

    public Color IdleColor;
    public float MaxIdleTime;

    [Space(10)]
    public Color MedicColor;
    public float MaxMedicTime;

    [Space(10)]
    public Color RageQuitColor;
}
