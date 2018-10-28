using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 2)]
public class LevelData : ScriptableObject
{
    [Header("Rules")]
    public float GameDuration = 10.0f;

    [Header("Blood")]
    [Space(10)]
    public BloodInfo.BloodType[] BloodTypes;
    public BloodInfo.BloodFamily[] BloodFamilies;

    [Header("Doc")]
    [Space(10)]
    public float RejectChance;

    [Header("Humans")]
    [Space(10)]
    public float SpawnSpeed = 15.0f;
    public int MaxHumanCount = 3;
}
