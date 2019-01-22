using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillIconClock : FillIcon {

    [SerializeField] private Transform _indicator = null;

    public override void Fill(float value)
    {
        base.Fill(value);
        _indicator.localEulerAngles = Vector3.back * value * 360;
    }
}
