using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("GameSettings")]


    [Header("Blood donor settings")]
    [SerializeField] private BloodDonor _bloodDonor;
    [SerializeField] private Transform _bdSpawn;

    List<BloodDonor> _bloodDonors = new List<BloodDonor>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        SpawnBloodDonor();
    }

	void Update ()
    {
		
	}

    void SpawnBloodDonor()
    {
        _bloodDonors.Add( Instantiate( _bloodDonor, _bdSpawn.position, _bdSpawn.rotation));
    }
}
