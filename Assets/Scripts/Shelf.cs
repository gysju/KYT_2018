using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour {

    #region Var
    protected List<DragableObj> _bags = new List<DragableObj>();
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
    public virtual void FillIn(DragableObj bag)
    {
        bag.gameObject.SetActive(false);
        _bags.Add(bag);
    }
    public virtual DragableObj TakeOut()
    {
        if (_bags.Count > 0)
        {
            DragableObj b = _bags[0];
            b.gameObject.SetActive(true);
            _bags.RemoveAt(0);
            return b;
        }
        return null;
    }
    public int GetNumber()
    {
        return _bags.Count;
    }
    public void ResetStock()
    {
        _bags.Clear();
    }
    #endregion
}
