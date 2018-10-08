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
    private void Start()
    {
        if (type == BloodInfo.BloodType.Blood)
            duration = data.MaxTakingBloodTime;
        else if (type == BloodInfo.BloodType.Plasma)
            duration = data.MaxTakingPlasmaTime;
        else if (type == BloodInfo.BloodType.Platelet)
            duration = data.MaxTakingPlateletTime;
    }
    #endregion
    #region Function
    public override void Begin()
    {
        base.Begin();
        _donor.CurrentState = BloodDonor.State.taking;
        _donor.animator.SetBool("sleep", true);
    }
    protected override void End()
    {
        if (_donor != null)
        {
            BloodBag b = Instantiate(bloodbag, _bloodbagSpawn.position, _bloodbagSpawn.rotation).GetComponent<BloodBag>();
            b.bloodInfo = _donor.Blood;

            GameManager.Instance.AddBag(b);
            
            _donor._navMeshAgent.enabled = true;
            //_donor._navMeshAgent.SetDestination(transform.position);
            //_donor._navMeshAgent.isStopped = false;

            _donor.CurrentState = BloodDonor.State.leave;
            _donor.animator.SetBool("sleep", false);
        }
        
        base.End();
    }
    #endregion
}
