using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody _rgd;

    private void Awake()
    {
        _rgd = GetComponent<Rigidbody>();
    }

    void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        Move();
    }

    private void Move()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Debug.Log(hAxis);

        if (hAxis != 0.0f || vAxis != 0.0f)
        {
            float angle = Mathf.Atan2(vAxis, -hAxis) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            _rgd.AddForce( transform.forward * _speed * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}
