using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableWihDonor {

    #region Var
    private BloodInfo.BloodType _type;
    public GameObject bloodbag;

    private bool _hasAlrBeFeed;
    [SerializeField] private Transform _bloodbagSpawn;

    [SerializeField] private Material[] _materialsBloodBags;
    #endregion
    #region MonoFunction
    protected override void Start()
    {
        base.Start();

    }
    #endregion
    #region Function
    public override void Begin()
    {
        SetBloodBagUIolor(_donor.bloodInfo.type);

        base.Begin();
        _donor.state = BloodDonor.State.taking;
        _donor.animator.SetBool("sleep", true);

        //_donor.desableKinematic = false;
        //_donor._navMeshAgent.enabled = false;
    }
    protected override void End()
    {
        if (_donor != null)
        {
            BloodBag b = Instantiate(bloodbag, _bloodbagSpawn.position, _bloodbagSpawn.rotation).GetComponent<BloodBag>();
            b.bloodInfo = _donor.bloodInfo;
            b.SetBed(this);
            b.GetComponent<MeshRenderer>().material = _materialsBloodBags[((int)_type) - 1];

            GameManager.Instance.AddBag(b);
        }

        _running = false;
    }
    public override void AlternativeEnd()
    {
        if (_donor != null)
        {
            //_donor.desableKinematic = true;
            //_donor.GetRigidbody().isKinematic = false;
            //_donor._navMeshAgent.enabled = true;

            _donor.state = BloodDonor.State.leave;
            _donor.animator.SetBool("sleep", false);
            _hasAlrBeFeed = false;
        }
        base.End();
        base.AlternativeEnd();
    }
    public void TryFeed(DragableObj food)
    {
        if (!_hasAlrBeFeed)
        {
            _hasAlrBeFeed = true;
            CanvasManager.Instance.AddScore(_data.ScoreByFoodGiven);
        }
        Destroy(food.gameObject);
    }

    private void SetBloodBagUIolor(BloodInfo.BloodType type)
    {
        _type = type;
        if (type == BloodInfo.BloodType.Blood)
        {
            _duration = _data.MaxTakingBloodTime;
            _fillIcon.SetProgressionColor(_data.TakingBloodColor);
        }
        else if (type == BloodInfo.BloodType.Plasma)
        {
            _duration = _data.MaxTakingPlasmaTime;
            _fillIcon.SetProgressionColor(_data.TakingPlasmaColor);
        }
        else if (type == BloodInfo.BloodType.Platelet)
        {
            _duration = _data.MaxTakingPlateletTime;
            _fillIcon.SetProgressionColor(_data.TakingPlateletColor);
        }
    }
    #endregion
}
