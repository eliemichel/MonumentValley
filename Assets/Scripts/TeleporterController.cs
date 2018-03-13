using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour {
    public Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // TODO: change orientation as well
            other.transform.position += target.position - transform.position;
            CharacController charac = other.GetComponent<CharacController>();
            if (charac)
            {
                //charac.target.position += target.position - transform.position;
                charac.NextPathTarget();
            }
        }
    }
}
