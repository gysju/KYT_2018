using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : Timer {

    #region Var
    private Transform _donor;
    [SerializeField] private Transform _inside, _outside;
    #endregion
    #region MonoFunction
    #endregion
    #region Function
    public void SetDonor(Transform donor)
    {
        _donor = donor;
    }
    public override void Begin()
    {
        if (_donor == null) return;
        
        base.Begin();

        _donor.position = _inside.position;
    	_donor.rotation = _inside.rotation;
    }
    protected override void End()
    {
        if (_donor == null) return;

        base.End();

    	_donor.position = _outside.position;
    	_donor.rotation = _outside.rotation;
        _donor = null;
    }
    #endregion
}
