using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    #region Var
    public static float deltaTime, time, timeScale, timeAbsolute;

    public static bool paused { get { return timeScale <= 0; } }
    #endregion
    #region MonoFunctions
    private void Awake()
    {
        deltaTime = 0;
        time = 0;
        timeScale = 1;
    }
    private void Update()
    {
        deltaTime = Time.deltaTime * timeScale;
        time += deltaTime;
        timeAbsolute += Time.deltaTime;
    }
    #endregion
    #region Functions
    #endregion
}
