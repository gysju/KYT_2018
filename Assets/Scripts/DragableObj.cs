using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableObj : MonoBehaviour {

    #region Var
    #endregion
    #region MonoFunction
    #endregion
    #region Function
    public virtual void Attach(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public virtual void Detach()
    {
        transform.parent = null;
    }
    #endregion
}
