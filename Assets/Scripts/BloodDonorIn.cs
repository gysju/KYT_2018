using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDonorIn : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Donor"))
        {
            BloodDonor d = other.GetComponent<BloodDonor>();
            if (d.state == BloodDonor.State.home)
                d.state = BloodDonor.State.idle;
        }
    }
}
