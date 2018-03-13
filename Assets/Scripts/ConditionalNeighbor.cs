using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNeighbor : MonoBehaviour {
    public HandleController handle;
    public float angle;
    public NavigationCorner neighbor;
    public float angleLimit = 5;

    // DEBUG
    public bool isActive = false;

    private void Update()
    {
        isActive = IsActive();
    }

    public bool IsActive()
    {
        float deltaAngle = (handle.transform.rotation.eulerAngles.z - angle + 360 + 180) % 360 - 180;
        return Mathf.Abs(deltaAngle) < angleLimit;
    }
}
