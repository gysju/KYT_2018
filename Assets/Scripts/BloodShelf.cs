using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodShelf : MonoBehaviour {

    #region Var
    public BloodInfo info;

    [HideInInspector] public List<BloodBag> bloodBags;

    [SerializeField] private Transform stock;
    [SerializeField] private TextMesh family, resus;
    #endregion
    #region MonoFunction
    private void Start()
    {
        string f = info.family.ToString();
        family.text = ""+f[0];
        if (f[1] == 'P') resus.text = "+";
        else if (f[1] == 'N') resus.text = "-";
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
    #endregion
}
