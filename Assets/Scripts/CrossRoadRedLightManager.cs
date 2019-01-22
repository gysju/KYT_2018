using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossRoadRedLightManager : MonoBehaviour {


    [SerializeField] private RedLight[] redLights = null;


    [SerializeField] private float _refreshTime = 25f;
    private float _time = 10;

    private void Update ()
    {
		if (_time < Time.time)
        {
            _time = Time.time + _refreshTime;
            for (int i = 0; i < redLights.Length; i++)
                redLights[i].ToggleLight();
        }
	}
}