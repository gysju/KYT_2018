using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : InteractableWihDonor {

    #region Var
    #endregion
    #region MonoFunction
    #endregion
    #region Function
    private void Start()
    {
        duration = data.MaxMedicTime;
    }
    public override void Begin()
    {
        base.Begin();
    }
    protected override void End()
    {
        _donor.CurrentState = Random.Range((float)0, 1) < data.rejectChance ? BloodDonor.State.leave : BloodDonor.State.medic;
        base.End();
    }
    #endregion
}
