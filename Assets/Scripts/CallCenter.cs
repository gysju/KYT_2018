using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallCenter : Timer {

    #region Var
    [SerializeField] protected UICallDonor _UICallDonor = null;

    [SerializeField] protected FillIcon _fillIcon = null;

    [SerializeField] MeshRenderer[] _meshRenderers = null;
    public MeshRenderer[] meshRenderers { get { return _meshRenderers; } }

    public bool inRecorvery { get { return _running; } }

    private Player _user;
    #endregion
    #region MonoFunction
    protected override void Start()
    {
        base.Start();
        _duration = _data.MaxCallCenterReuse;
    }
    protected override void Update()
    {
        base.Update();

        if (_running)
            _fillIcon.Fill(Mathf.Clamp01(1 - (_time - TimeManager.time) / _duration));
    }
    #endregion
    #region Function
    public override void Begin()
    {
        base.Begin();
    }
    protected override void End()
    {
        _UICallDonor.ShowHideTimer(false);
        base.End();
    }
    public void Open(Player player)
    {
        if (!_running && _user == null)
        {
            _user = player;
            _user.canMove = false;
            _UICallDonor.Open(_user.playerId);
        }
    }
    public void Call(BloodInfo.BloodType type, BloodInfo.BloodFamily familly)
    {
        LiberatePlayer();
        GameManager.inst.CallDonor(type, familly);

        Begin();
    }
    public void LiberatePlayer()
    {
        _user.canMove = true;
        _user = null;
    }
    public void ResetCallCenter()
    {
        _UICallDonor.Close();
        End();
    }
    #endregion
}
