using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWihDonor : Timer {

    #region Var
    protected BloodDonor _donor;
    [SerializeField] protected Transform _inside, _outside;

    public bool occupied { get { return _donor == null; } }
    #endregion
    #region MonoFunction
    #endregion
    #region Function
    public void SetDonor(BloodDonor donor)
    {
        _donor = donor;
        Begin();
    }
    public override void Begin()
    {
        if (_donor == null) return;

        base.Begin();

        _donor.transform.position = _inside.position;
        _donor.transform.rotation = _inside.rotation;
    }
    protected override void End()
    {
        if (_donor == null) return;

        base.End();

        _donor.transform.position = _outside.position;
        _donor.transform.rotation = _outside.rotation;
        _donor = null;
    }
    #endregion
}
