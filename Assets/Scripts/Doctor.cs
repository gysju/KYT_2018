using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : InteractableWihDonor {

    #region Var
    #endregion
    #region MonoFunction
    #endregion
    #region Function
    protected override void Start()
    {
        base.Start();
        duration = _data.MaxMedicTime;
    }
    public override void Begin()
    {
        base.Begin();
    }
    protected override void End()
    {
        _donor.CurrentState = Random.Range((float)0, 1) < _data.rejectChance ? BloodDonor.State.leave : BloodDonor.State.medic;
        base.End();
    }
    #endregion
}
