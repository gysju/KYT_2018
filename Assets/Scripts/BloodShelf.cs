using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BloodShelf : MonoBehaviour {

    #region Var
    public BloodInfo info;

    [HideInInspector] public List<BloodBag> bloodBags;

    [SerializeField] private Transform stock;
    [SerializeField] private TextMeshProUGUI[] family, rhesus1, rhesus2;
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
        if (bag.bloodInfo.Equals(info))
        {
            bag.transform.position = stock.position;
            bag.gameObject.SetActive(false);

            bloodBags.Add(bag);
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
            return b;
        }
        else Debug.Log("Empthy");

        return null;
    }

    private void SetBloodInfoText()
    {
        string f = info.family.ToString();
        string r = info.rhesus.ToString();
        for (int i = 0; i < family.Length; i++)
            family[i].text = f;
        if (r.Equals("neg"))
        {
            if (f.Length > 1)
            {
                for (int i = 0; i < rhesus1.Length; i++)
                {
                    rhesus1[i].text = "-"; ;
                    rhesus2[i].text = ""; ;
                }
            }
            else
            {
                for (int i = 0; i < rhesus1.Length; i++)
                {
                    rhesus1[i].text = ""; ;
                    rhesus2[i].text = "-"; ;
                }
            }
        }
        else if (r.Equals("pos"))
        {
            if (f.Length > 1)
            {
                for (int i = 0; i < rhesus1.Length; i++)
                {
                    rhesus1[i].text = "+"; ;
                    rhesus2[i].text = ""; ;
                }
            }
            else
            {
                for (int i = 0; i < rhesus1.Length; i++)
                {
                    rhesus1[i].text = ""; ;
                    rhesus2[i].text = "+"; ;
                }
            }
        }
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
