using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour {

    private Transform _cam, _transform;
    private Quaternion _facingTo;

    void Start()
    {
        _cam = Camera.main.transform;
        _transform = GetComponent<Transform>();

        transform.LookAt(_cam.position);
        _facingTo = Quaternion.Euler(new Vector3(_transform.eulerAngles.x, 137.446f + 45, _transform.eulerAngles.z));
    }

    void Update ()
    {
        if (_transform.rotation != _facingTo)
            _transform.rotation = _facingTo;
    }
}
