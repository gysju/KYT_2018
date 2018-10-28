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
        _duration = _data.MaxMedicTime;
    }
    public override void Begin()
    {
        _donor.state = BloodDonor.State.medic;
        base.Begin();
    }
    protected override void End()
    {
        _fillIcon.SetActive(false);

        BloodDonor tempDonor = _donor;
        base.End();
        tempDonor.state = Random.Range(0f, 1) < _data.RejectChance ? BloodDonor.State.leave : BloodDonor.State.idle;
    }
    #endregion
}
