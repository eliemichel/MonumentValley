using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationCorner : MonoBehaviour {
    public NavigationCorner[] neighbors;

    public NavigationCorner[] Neighbors {
        get {
            List<NavigationCorner> allNeighbors = new List<NavigationCorner>();
            foreach (NavigationCorner n in neighbors)
            {
                allNeighbors.Add(n);
            }
            ConditionalNeighbor[] condNeighbors = GetComponents<ConditionalNeighbor>();
            foreach (ConditionalNeighbor cn in condNeighbors)
            {
                if (cn.IsActive())
                {
                    allNeighbors.Add(cn.neighbor);
                }
            }
            return allNeighbors.ToArray();
        }
    }

    public enum Status
    {
        NotVisited,
        Enqueued,
        Visited,
    }
    //[HideInInspector]
    public Status status = Status.NotVisited;
    //[HideInInspector]
    public NavigationCorner prev = null;
}
