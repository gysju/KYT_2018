﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableObj : MonoBehaviour {

    #region Var
    private Player _attachBy;

    private MeshRenderer[] _meshRenderers;
    public MeshRenderer[] meshRenderers { get { return _meshRenderers; } }
    #endregion
    #region MonoFunction
    protected virtual void Start()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }
    #endregion
    #region Function
    public virtual void Attach(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public virtual void Attach(Transform parent, Player player)
    {
        Attach(parent);

        if (_attachBy != null)
            _attachBy.ForceDrop();
        _attachBy = player;
    }
    public virtual void Detach()
    {
        transform.parent = null;
    }
    #endregion
}
