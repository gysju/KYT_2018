using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    #region Var
    public GameData data;
    protected float duration = 5;
    private bool running;
    private float _time;
    #endregion
    #region MonoFunction
    private void Update ()
    {
		if (_time < Time.time && running)
        {
            End();
        }
	}
    #endregion
    #region Function
    public virtual void Begin()
    {
        running = true;
        _time = Time.time + duration;
    }
    protected virtual void End()
    {
        running = false;
    }
    #endregion
}
