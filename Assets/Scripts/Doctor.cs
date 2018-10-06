﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : InteractableWihDonor {

    #region Var
    private const float invalidePerson = .05f;
    #endregion
    #region MonoFunction
    #endregion
    #region Function
    public override void Begin()
    {
        base.Begin();
    }
    protected override void End()
    {
        _donor.CurrentState = Random.Range((float)0, 1) < invalidePerson ? BloodDonor.State.leave : BloodDonor.State.medic;
        base.End();
    }
    #endregion
}
