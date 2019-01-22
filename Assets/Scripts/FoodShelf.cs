using System.Collections.Generic;
using UnityEngine;

public class FoodShelf : Shelf {

    #region Var
    [SerializeField] private DragableObj _foodBag = null; //prefab
    #endregion
    #region MonoFunction
    #endregion
    #region Function
    public override int FillIn(DragableObj bag)
    {
        if (bag.CompareTag("FoodBag"))
        {
            base.FillIn(bag);
        }
        return -1;
    }
    public override DragableObj TakeOut()
    {
        if (_bags.Count > 0)
            return base.TakeOut();
        else return Instantiate(_foodBag);
    }
    #endregion
}
