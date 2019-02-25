using System.Collections.Generic;
using UnityEngine;

public struct GameData
{
    private ConstGameData _constData;
    private LevelData _levelData;

    public float GameDuration { get { return _levelData.GameDuration; } }
    public int ScoreByCommandComplete { get { return _constData.ScoreByCommandComplete; } }
    public int ScoreByCommandPartiallyComplete { get { return _constData.ScoreByCommandPartiallyComplete; } }
    public int ScoreByBloodStocked { get { return _constData.ScoreByBloodStocked; } }
    public int ScoreByFoodGiven { get { return _constData.ScoreByFoodGiven; } }

    public BloodInfo.BloodType[] BloodTypes { get { return _levelData.BloodTypes; } }
    public BloodInfo.BloodFamily[] BloodFamilies { get { return _levelData.BloodFamilies; } }

    public Color IdleColor { get { return _constData.IdleColor; } }
    public float MaxIdleTime { get { return _constData.MaxIdleTime; } }

    public Color MedicColor { get { return _constData.MedicColor; } }
    public float MaxMedicTime { get { return _constData.MaxMedicTime; } }
    public float RejectChance { get { return _levelData.RejectChance; } }

    public Color TakingBloodColor { get { return _constData.TakingBloodColor; } }
    public float MaxTakingBloodTime { get { return _constData.MaxTakingBloodTime; } }

    public Color TakingPlasmaColor { get { return _constData.TakingPlasmaColor; } }
    public float MaxTakingPlasmaTime { get { return _constData.MaxTakingPlasmaTime; } }

    public Color TakingPlateletColor { get { return _constData.TakingPlateletColor; } }
    public float MaxTakingPlateletTime { get { return _constData.MaxTakingPlateletTime; } }

    public Color RageQuitColor { get { return _constData.RageQuitColor; } }

    [System.Serializable]
    public struct HumanData
    {
        public Mesh Model;
        public Material Mat;
    }

    public List<HumanData> HumansDatas { get { return _constData.HumansDatas; } }
    public float SpawnSpeed { get { return _levelData.SpawnSpeed; } }
    public int MaxHumanCount { get { return _levelData.MaxHumanCount; } }

    public GameData(ConstGameData constData, LevelData levelData)
    {
        _constData = constData;
        _levelData = levelData;
    }

    public float MaxCallCenterReuse { get { return _levelData.MaxCallCenterReuseTime; } }
    public int NumberOfDonorByCall { get { return _levelData.NumberOfDonorByCall; } }
    public float TimeBetweenDonor { get { return _levelData.TimeBetweenDonor; } }
}