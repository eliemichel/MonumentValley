using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationTargetController : MonoBehaviour {
    public CharacController charac;
    public Renderer skinRenderer;

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

        Vector3 a = surf.cornerA.transform.position;
        Vector3 b = surf.cornerB.transform.position;
        float lambda = Vector3.Dot(transform.position - a, b - a) / (b - a).sqrMagnitude;
        lambda = Mathf.Clamp(lambda, 0, 1);

        Vector3 newPosition = a + lambda * (b - a);
        bool positionChanged = (transform.position - newPosition).sqrMagnitude > 1e-6;
        transform.position = newPosition;
        transform.rotation = Quaternion.Lerp(surf.cornerA.transform.rotation, surf.cornerB.transform.rotation, lambda);

        if (positionChanged)
        {
            neighborCorners = new NavigationCorner[] { surf.cornerA, surf.cornerB };
            skinRenderer.enabled = charac.FindPath();
        }
    }

    public void Reached()
    {
        skinRenderer.enabled = false;
    }
}
