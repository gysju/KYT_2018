using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWihDonor : Timer {

    #region Var
    protected BloodDonor _donor;
    [SerializeField] protected Transform _inside, _outside;
    private float _enterYPos;

    public bool occupied { get { return _donor != null; } }
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

        _enterYPos = _donor.transform.position.y;

        _donor.onProcess = true;
        _donor.desableKinematic = false;
        _donor.transform.position = _inside.position;
        _donor.transform.rotation = _inside.rotation;
    }
    protected override void End()
    {
        if (_donor == null) return;

        base.End();

        _donor.onProcess = false;
        _donor.desableKinematic = true;
        _donor.transform.position = new Vector3(_outside.position.x, _enterYPos, _outside.position.z);
        _donor.transform.rotation = _outside.rotation;

        _donor.Detach();
        _donor = null;
    }
    public void ResetObj()
    {
        base.End();
        _donor = null;
    }
    #endregion
}
