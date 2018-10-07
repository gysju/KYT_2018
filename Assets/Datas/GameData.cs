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
    public float rejectChance;

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

    [System.Serializable]
    public struct HumanData
    {
        public Mesh Model;
        public Material Mat;
    }

    [Header("Humans")]
    public List<HumanData> HumansDatas = new List<HumanData>();
    public float SpawnSpeed = 15.0f;
    public int MaxHumanCount = 3;
}
