using UnityEngine;

public class FoodBag : DragableObj {

    public override void Detach()
    {
        base.Detach();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
