using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    #region Var
    protected GameData _data;
    protected float duration = 5;
    private bool running;
    private float _time;
    #endregion
    #region MonoFunction
    protected virtual void Start()
    {
        _data = GameManager.Instance.RequestData();
    }
    private void Update ()
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
        _time = TimeManager.time + duration;
    }
    protected virtual void End()
    {
        running = false;
    }
    #endregion
}
