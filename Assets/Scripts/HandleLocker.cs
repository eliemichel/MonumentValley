using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleLocker : MonoBehaviour {
    public HandleController handle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            handle.Lock();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            handle.Lock();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            handle.Unlock();
        }
    }
}
