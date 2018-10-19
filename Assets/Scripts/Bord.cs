using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bord : MonoBehaviour {

    //public TextMeshProUGUI text;
    public Text text;
    public Image img;

    public void SetData(bool active, Sprite sprite , string t)
    {
        gameObject.SetActive(active);
        if (active)
        {
            img.sprite = sprite;
            text.text = active ? t : "";
        }
    }
}
