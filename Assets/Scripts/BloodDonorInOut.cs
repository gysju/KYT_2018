using UnityEngine;

public class BloodDonorInOut : MonoBehaviour {

    [SerializeField] private bool _in = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Donor"))
        {
            BloodDonor d = other.GetComponent<BloodDonor>();
            if (_in)
            {
                if (d.state == BloodDonor.State.home)
                    d.state = BloodDonor.State.idle;
            }
            else if (d.state == BloodDonor.State.leave || d.state == BloodDonor.State.rageQuit)
                d.state = d.state;
        }
    }
}
