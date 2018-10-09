using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableWihDonor {

    #region Var
    public BloodInfo.BloodType type;
    public GameObject bloodbag;

    [SerializeField] private Transform _bloodbagSpawn;
    #endregion
    #region MonoFunction
    protected override void Start()
    {
        base.Start();
        if (type == BloodInfo.BloodType.Blood)
            duration = _data.MaxTakingBloodTime;
        else if (type == BloodInfo.BloodType.Plasma)
            duration = _data.MaxTakingPlasmaTime;
        else if (type == BloodInfo.BloodType.Platelet)
            duration = _data.MaxTakingPlateletTime;
    }
    #endregion
    #region Function
    public override void Begin()
    {
        base.Begin();
        _donor.CurrentState = BloodDonor.State.taking;
        _donor.animator.SetBool("sleep", true);

        _donor.desableKinematic = false;
        _donor._navMeshAgent.enabled = false;
    }
    protected override void End()
    {
        if (_donor != null)
        {
            BloodBag b = Instantiate(bloodbag, _bloodbagSpawn.position, _bloodbagSpawn.rotation).GetComponent<BloodBag>();
            b.bloodInfo = _donor.Blood;

            GameManager.Instance.AddBag(b);

            _donor.desableKinematic = true;
            _donor.GetRigidbody().isKinematic = false;
            _donor._navMeshAgent.enabled = true;

            _donor.CurrentState = BloodDonor.State.leave;
            _donor.animator.SetBool("sleep", false);
        }
        base.End();

    }
    #endregion
}
