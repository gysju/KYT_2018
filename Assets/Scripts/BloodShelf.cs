using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BloodShelf : Shelf {

    #region Var
    public BloodInfo info = null;
    private GameData _data;
    [SerializeField] private TextMeshProUGUI[] _family = null;

    [SerializeField] private AudioSource _audioSource = null;
    #endregion
    #region MonoFunction
    protected override void Start()
    {
        _data = GameManager.inst.RequestData();
        SetBloodInfoText();

        base.Start();
    }
    #endregion
    #region Function
    public override int FillIn(DragableObj bag)
    {
        int score = -1;
        if (bag.CompareTag("Bloodbag"))
        {
            if (((BloodBag)bag).bloodInfo.Equals(info, true, true, false))
            {
                base.FillIn(bag);
                score = _data.ScoreByBloodStocked;
                CanvasManager.inst.AddScore(score);
            }
            else
            {
                Debug.Log("Your are bad: wrong shelf for this blood bag");
                Destroy(bag.gameObject);
                _audioSource.Play();
            }
        }
        return score;
    }
    public override DragableObj TakeOut()
    {
        if (_bags.Count > 0)
            CanvasManager.inst.AddScore(-_data.ScoreByBloodStocked);
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