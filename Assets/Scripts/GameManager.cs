using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("GameSettings")]
    [SerializeField] Transform _pathParent;
    List<Transform> _paths = new List<Transform>();

    [Header("Blood donor settings")]
    [SerializeField] private BloodDonor _bloodDonor;
    [SerializeField] private Transform _bdSpawn;

    List<BloodDonor> _bloodDonors = new List<BloodDonor>();

	void Start ()
    {
        _paths = _pathParent.GetComponentsInChildren<Transform>().ToList();
        if (_paths[0] == _pathParent)
            _paths.Remove(_paths[0]);

        SpawnBloodDonor();
    }

	void Update ()
    {
		
	}

    void SpawnBloodDonor()
    {
        _bloodDonors.Add( Instantiate( _bloodDonor, _bdSpawn.position, _bdSpawn.rotation));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_paths == null || _paths.Count == 0)
        {
            _paths = _pathParent.GetComponentsInChildren<Transform>().ToList();
            if (_paths!= null && _paths[0] == _pathParent)
                _paths.Remove(_paths[0]);
        }

        for (int i = 0; i < _paths.Count; i++)
        {
            if (i < _paths.Count - 1)
                Gizmos.DrawLine(_paths[i].position, _paths[i + 1].position);
        }
    }
}
