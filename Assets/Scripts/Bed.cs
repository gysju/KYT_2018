﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableWihDonor {

    #region Var
    public BloodInfo.BloodType type;
    public GameObject bloodbag;

    [SerializeField] private Transform bloodbagSpawn;
    #endregion
    #region MonoFunction
    private void Start()
    {
        if (type == BloodInfo.BloodType.Blood)
            duration = data.MaxTakingBloodTime;
        else if (type == BloodInfo.BloodType.Plasma)
            duration = data.MaxTakingPlasmaTime;
        else if (type == BloodInfo.BloodType.Platelet)
            duration = data.MaxTakingPlateletTime;
    }
    #endregion
    #region Function
    public override void Begin()
    {
        base.Begin();
        _donor.CurrentState = BloodDonor.State.taking;
    }
    protected override void End()
    {
        if (_donor != null)
        {
            BloodBag b = Instantiate(bloodbag, bloodbagSpawn.position, bloodbagSpawn.rotation).GetComponent<BloodBag>();
            b.bloodInfo = _donor.Blood;
            _donor.CurrentState = BloodDonor.State.leave;
        }

        base.End();
    }
    #endregion
}