using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BloodShelf : Shelf {

    #region Var
    public BloodInfo info;
    private GameData _data;
    [SerializeField] private TextMeshProUGUI[] _family;
    #endregion
    #region MonoFunction
    protected override void Start()
    {
        _data = GameManager.Instance.RequestData();
        SetBloodInfoText();

        base.Start();
    }
    #endregion
    #region Function
    public override void FillIn(DragableObj bag)
    {
        if (bag.CompareTag("Bloodbag"))
        {
            if (((BloodBag)bag).bloodInfo.Equals(info, true, true, false))
            {
                base.FillIn(bag);
                CanvasManager.Instance.AddScore(_data.ScoreByBloodStocked);
            }
            else
            {
                Debug.Log("Your are bad: wrong shelf for this blood bag");
                Destroy(bag.gameObject);
            }
        }
    }
    public override DragableObj TakeOut()
    {
        if (_bags.Count > 0)
            CanvasManager.Instance.AddScore(-_data.ScoreByBloodStocked);
        else Debug.Log("Empthy");

        return base.TakeOut();
    }
    private void SetBloodInfoText()
    {
        string f = info.family.ToString();
        for (int i = 0; i < _family.Length; i++)
            _family[i].text = f;
    }
    #endregion
}