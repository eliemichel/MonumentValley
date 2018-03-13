using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour {
    public Camera cam;
    public NavigationTargetController navTarget;

    private int plateformsLayerMask;
    private bool hasClicked = false;

    void Start()
    {
        plateformsLayerMask = LayerMask.GetMask("ClickablePlateforms");
    }

    void Update()
    {
        hasClicked = Input.GetMouseButtonDown(0);
    }

    void FixedUpdate()
    {
        if (hasClicked)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000, (int)0xffffff, QueryTriggerInteraction.Ignore))
            {
                navTarget.transform.position = hit.point;
                navTarget.transform.rotation = Quaternion.FromToRotation(navTarget.transform.up, hit.normal) * navTarget.transform.rotation;
            }
        }

        hasClicked = false;
    }
}
