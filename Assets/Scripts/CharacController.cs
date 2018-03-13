using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacController : MonoBehaviour {
    public Animator anim;
    public float speed = 1.0f;
    public Transform target;

    public NavigationTargetController navTarget;
    public NavigationSourceController navSource;

    private Vector3 characterUp = Vector3.up;
    private List<Transform> path = new List<Transform>();

    void Update()
    {
        Transform actualTarget = target;

        if (path.Count > 0)
        {
            actualTarget = path[path.Count - 1];
        }
        else
        {
            actualTarget = null;
        }

        if (actualTarget)
        {
            float d = Vector3.Distance(actualTarget.position, transform.position);
            if (d > 1e-3)
            {
                anim.SetBool("isWalking", true);
                transform.LookAt(actualTarget, characterUp);
                transform.Translate(Vector3.forward * Mathf.Min(d, speed * Time.deltaTime));
            }
            else
            {
                NextPathTarget();
            }
        }
    }

    public void NextPathTarget()
    {
        if (path.Count > 0)
        {
            path.RemoveAt(path.Count - 1);
            if (path.Count == 0)
            {
                navTarget.Reached();
            }
        }

        if (path.Count == 0)
        {
            anim.SetBool("isWalking", false);
        }
    }

    void ClearPath()
    {
        path.Clear();
        anim.SetBool("isWalking", false);
    }

    public bool IsWalking()
    {
        return path.Count > 0;
    }

    /**
     * Rough pathfinding: we explore the whole graph with no subtility because it is always quite small
     */
    public bool FindPath()
    {
        Debug.Log("== Start Pathfinding ==");
        ClearPath();
        bool found = false;

        HashSet<NavigationCorner> targets = new HashSet<NavigationCorner>();
        foreach (NavigationCorner t in navTarget.NeighborCorners)
        {
            targets.Add(t);
        }

        bool isOnSameSurface = true;
        
        // Keep a list of modified corners for cleanup
        List<NavigationCorner> affectedCorners = new List<NavigationCorner>();

        Queue<NavigationCorner> heads = new Queue<NavigationCorner>();
        foreach (NavigationCorner s in navSource.NeighborCorners)
        {
            s.prev = null;
            s.status = NavigationCorner.Status.Enqueued;
            affectedCorners.Add(s);
            heads.Enqueue(s);

            isOnSameSurface = isOnSameSurface && targets.Contains(s);
        }

        if (isOnSameSurface)
        {
            path.Clear();
            path.Add(navTarget.transform);
            return true;
        }

        while (!found && heads.Count > 0)
        {
            NavigationCorner corner = heads.Dequeue();
            if (corner.status == NavigationCorner.Status.Visited)
            {
                continue;
            }

            // Mark visited to avoid handling it multiple times
            corner.status = NavigationCorner.Status.Visited;
            affectedCorners.Add(corner);

            // Process
            Debug.Log("Processing " + corner + "-" + corner.GetInstanceID() + ", coming from " + (corner.prev ? (corner.prev + "-" + corner.prev.GetInstanceID()) : "null"));

            if (targets.Contains(corner))
            {
                path.Clear();
                path.Add(navTarget.transform);
                NavigationCorner c = corner;
                while (c != null)
                {
                    path.Add(c.transform);
                    c = c.prev;
                }
                found = true;
                break;
            }

            // Recurse
            foreach (NavigationCorner c in corner.Neighbors)
            {
                if (c.status == NavigationCorner.Status.NotVisited)
                {
                    c.prev = corner;
                    c.status = NavigationCorner.Status.Enqueued;
                    heads.Enqueue(c);
                }
            }
        }

        // Cleanup
        foreach (NavigationCorner corner in affectedCorners)
        {
            corner.status = NavigationCorner.Status.NotVisited;
            corner.prev = null;
        }
        while (heads.Count > 0)
        {
            NavigationCorner corner = heads.Dequeue();
            corner.status = NavigationCorner.Status.NotVisited;
            corner.prev = null;
        }

        if (!found)
        {
            navTarget.Reached();
        }
        return found;
    }
}
