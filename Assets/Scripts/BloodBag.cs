using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBag : DragableObj {

    public BloodInfo bloodInfo;
    private Bed _bed;

    public override void Attach(Transform parent)
    {
        base.Attach(parent);

        if (_bed != null)
        {
            _bed.TempEnd();
            _bed = null;
        }
    }

    public override void Detach()
    {
        base.Detach();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void SetBed(Bed bed)
    {
        _bed = bed;
    }
}
