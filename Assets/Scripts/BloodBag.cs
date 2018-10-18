using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBag : DragableObj {

    public BloodInfo bloodInfo;

    public override void Detach()
    {
        base.Detach();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
