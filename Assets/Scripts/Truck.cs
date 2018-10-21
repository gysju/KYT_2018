using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour {

    private bool _isBack, _needToGo;

    [SerializeField] private Animator anim;

    private void Update()
    {
        if (_isBack && _needToGo)
        {
            _isBack = false;
            _needToGo = false;
            anim.SetTrigger("Go");
        }
    }

    public void SetBack() { _isBack = true; }
    public void SetNeedGo() { _needToGo = true; }
}
