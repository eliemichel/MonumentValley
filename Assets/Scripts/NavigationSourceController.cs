using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSourceController : MonoBehaviour
{
    private NavigationCorner[] neighborCorners;
    public NavigationCorner[] NeighborCorners { get { return neighborCorners; } }

    private void OnTriggerEnter(Collider other)
    {
        Replace(other.GetComponent<NavigationSurface>());
    }

    private void OnTriggerStay(Collider other)
    {
        Replace(other.GetComponent<NavigationSurface>());
    }

    private void Replace(NavigationSurface surf)
    {
        if (!surf)
        {
            return;
        }
        neighborCorners = new NavigationCorner[] { surf.cornerA, surf.cornerB };
    }
}
