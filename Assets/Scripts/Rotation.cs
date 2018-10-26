using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    [SerializeField] private float _speed = 1;
    [SerializeField] private Vector3 _rotation = Vector3.forward;
    private Transform _transform;

    private void Start ()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update ()
    {
        _transform.Rotate(_rotation * _speed * TimeManager.deltaTime);
    }
}
