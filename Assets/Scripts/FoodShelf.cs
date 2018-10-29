using System.Collections.Generic;
using UnityEngine;

public class FoodShelf : Shelf {

    #region Var
    [SerializeField] private DragableObj _foodBag; //prefab
    #endregion
    #region MonoFunction
    #endregion
    #region Function
    public override void FillIn(DragableObj bag)
    {
        if (bag.CompareTag("FoodBag"))
        {
            base.FillIn(bag);
        }
    }
    public override DragableObj TakeOut()
    {
        if (_bags.Count > 0)
            return base.TakeOut();
        else return Instantiate(_foodBag);
    }
    #endregion
}
