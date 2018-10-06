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

        _donor.onProcess = true;
        _donor.transform.position = _inside.position;
        _donor.transform.rotation = _inside.rotation;
    }
    protected override void End()
    {
        if (_donor == null) return;

        base.End();

        _donor.onProcess = false;
        _donor.transform.position = _outside.position;
        _donor.transform.rotation = _outside.rotation;
        if (_donor._navMeshAgent.enabled)
            _donor._navMeshAgent.SetDestination(_outside.position);
        _donor = null;
    }
    #endregion
}
