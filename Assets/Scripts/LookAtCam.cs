using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour {

    private Transform _cam, _transform;

    void Start()
    {
        _cam = Camera.main.transform;
        _transform = GetComponent<Transform>();
    }

    void Update ()
    {
        _transform.LookAt(_cam.position);
    }
}
