using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BloodShelf : MonoBehaviour {

    #region Var
    public BloodInfo info;
    [SerializeField] private GameData _data;
    [HideInInspector] public List<BloodBag> bloodBags;

    [SerializeField] private Transform stock;
    [SerializeField] private TextMeshProUGUI[] family;
    #endregion
    #region MonoFunction
    private void Start()
    {
        SetBloodInfoText();
    }
    #endregion
    #region Function
    public void FillIn(BloodBag bag)
    {
        if (bag.bloodInfo.Equals(info, true, true, false))
        {
            bag.transform.position = stock.position;
            bag.gameObject.SetActive(false);

            bloodBags.Add(bag);
            CanvasManager.Instance.AddScore(_data.ScoreByBloodStocked);
        }
        else
        {
            Debug.Log("Your are bad: wrong shelf for this blood bag");
            Destroy(bag.gameObject);
        }
    }
    public BloodBag TakeOut()
    {
        if (bloodBags.Count > 0)
        {
            BloodBag b = bloodBags[0];
            b.gameObject.SetActive(true);
            bloodBags.RemoveAt(0);
            CanvasManager.Instance.AddScore(-_data.ScoreByBloodStocked);
            return b;
        }
        else Debug.Log("Empthy");

        return null;
    }

    private void SetBloodInfoText()
    {
        string f = info.family.ToString();
        for (int i = 0; i < family.Length; i++)
            family[i].text = f;
    }
    public int GetNumber()
    {
        return bloodBags.Count;
    }

    public void ResetStock()
    {
        bloodBags.Clear();
    }
    #endregion
}
