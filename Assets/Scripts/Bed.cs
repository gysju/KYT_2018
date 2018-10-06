using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableWihDonor {

    #region Var
    public BloodInfo.BloodType type;
    public GameObject bloodbag;

    [SerializeField] private Transform bloodbagSpawn;
    #endregion
    #region MonoFunction
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
