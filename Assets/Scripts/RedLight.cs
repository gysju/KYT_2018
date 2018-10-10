using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLight : MonoBehaviour {

    [SerializeField] private Collider _stopCollider;
    [SerializeField] private Light[] _lights;

    [SerializeField] private Color _red, _orange, _green;

    private float _transitionSpeed = 2f;
    public bool isRed = true, canDrive;
    
    public List<Car> cars = new List<Car>();

    private void Start()
    {
        if (isRed)
        {
            for (int i = 0; i < _lights.Length; i++)
                _lights[i].color = _red;
            //_stopCollider.enabled = true;
            canDrive = false;
        }            
        else
        {
            for (int i = 0; i < _lights.Length; i++)
                _lights[i].color = _green;
            //_stopCollider.enabled = false;
            canDrive = true;
        }
    }

    IEnumerator ChangeLightColor(Color trans, Color end)
    {
        for (int i = 0; i < _lights.Length; i++)
            _lights[i].enabled = false;
        yield return new WaitForSeconds(.1f); //transition
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].enabled = true;
            _lights[i].color = trans;
        }
        if (isRed)
        {
            //_stopCollider.enabled = true;
            canDrive = false;
        }
        yield return new WaitForSeconds(_transitionSpeed);
        for (int i = 0; i < _lights.Length; i++)
            _lights[i].enabled = false;
        yield return new WaitForSeconds(.1f); //transition
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].enabled = true;
            _lights[i].color = end;
            if (!isRed)
            {
                //_stopCollider.enabled = false;
                canDrive = true;
            }
        }
    }

    public void ToggleLight()
    {
        if (isRed)
            StartCoroutine(ChangeLightColor(_orange, _green));
        else StartCoroutine(ChangeLightColor(_orange, _red));
        isRed = !isRed;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Car car = other.GetComponent<Car>();
            car.AtRedLight();
            cars.Add(car);
        }
    }*/
}
