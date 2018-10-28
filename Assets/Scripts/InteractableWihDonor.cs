using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWihDonor : Timer {

    #region Var
    protected BloodDonor _donor;
    [SerializeField] protected Transform _inside, _outside;
    private float _enterYPos;

    [SerializeField] protected FillIcon _fillIcon;

    public bool occupied { get { return _donor != null; } }
    #endregion
    #region MonoFunction
    protected override void Start()
    {
        base.Start();
        _fillIcon.SetActive(false);
    }
    protected override void Update()
    {
        base.Update();
        if (_donor != null)
            _fillIcon.Fill(Mathf.Clamp01(1 - (_time - TimeManager.time) / _duration));
    }
    #endregion
    #region Function
    public void SetDonor(BloodDonor donor)
    {
        _donor = donor;
        _donor.DesableWaintingFill();
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
        _fillIcon.SetActive(false);
    }
    public virtual void AlternativeEnd()
    {
        _fillIcon.SetActive(false);
    }
    #endregion
}
