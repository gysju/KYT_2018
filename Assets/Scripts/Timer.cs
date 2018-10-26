using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    #region Var
    protected GameData _data;
    protected float _duration = 5;
    private bool running;
    protected float _time;
    #endregion
    #region MonoFunction
    protected virtual void Start()
    {
        _data = GameManager.Instance.RequestData();
    }
    protected virtual void Update ()
    {
		if (_time < TimeManager.time && running)
        {
            End();
        }
	}
    #endregion
    #region Function
    public virtual void Begin()
    {
        running = true;
        _time = TimeManager.time + _duration;
    }
    protected virtual void End()
    {
        running = false;
    }
    #endregion
}
