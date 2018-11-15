using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public float acceleration = 1;
    public float maxSpeed = 1;
    private float _crtSpeed = 1;

    private bool _breaked = false;

    private RedLight redLight;

    private Transform _transform;
    [SerializeField] private Transform _start = null, _end = null;

	private void Start ()
    {
        _transform = GetComponent<Transform>();

        _crtSpeed = maxSpeed;
    }
	
	private void Update ()
    {
        if (!_breaked)
        {
            if (_crtSpeed < maxSpeed)
                _crtSpeed += acceleration * Time.deltaTime; //do not replace by TimeManager -> animated pause scene
            transform.Translate(Vector3.forward * _crtSpeed * Time.deltaTime, Space.Self);

            if (Vector3.Distance(_transform.position, _end.position) < 1f)
                _transform.position = _start.position;
        }
        else if (redLight != null && redLight.canDrive)
        {
            AtGreenLight();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedLight"))
        {
            redLight = other.GetComponent<RedLight>();
            if (!redLight.canDrive)
                AtRedLight();
        }
        else
        {
            _crtSpeed = 0;
            _breaked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _breaked = false;
    }

    public void AtRedLight()
    {
        _crtSpeed = 0;
        _breaked = true;
    }
    public void AtGreenLight()
    {
        _breaked = false;
        redLight = null;
    }
}
