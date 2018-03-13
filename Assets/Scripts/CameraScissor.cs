using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraScissor : MonoBehaviour {
    public Rect rect;
    public bool liveUpdate = false;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateScissor();
    }

    void Update()
    {
		if (liveUpdate)
        {
            cam = GetComponent<Camera>();
            UpdateScissor();
        }
	}

    void UpdateScissor()
    {
        // Fix rect
        if (rect.x < 0)
        {
            rect.width += rect.x;
            rect.x = 0;
        }

        if (rect.y < 0)
        {
            rect.height += rect.y;
            rect.y = 0;
        }

        rect.width = Mathf.Min(1 - rect.x, rect.width);
        rect.height = Mathf.Min(1 - rect.y, rect.height);

        // Get camera matrix with full frame viewport, before setting it to the target scissor
        cam.rect = new Rect(0, 0, 1, 1);
        cam.ResetProjectionMatrix();
        Matrix4x4 m = cam.projectionMatrix;

        // Set rect
        cam.rect = rect;

        // Translate, then scale, to balance the deformation due to the change of viewport rect
        Vector3 t = new Vector3((1 - 2 * rect.x) - rect.width, (1 - 2 * rect.y) - rect.height, 0);
        Vector3 s = new Vector3(1 / rect.width, 1 / rect.height, 1);

        // Apply transform
        cam.projectionMatrix = Matrix4x4.Scale(s) * Matrix4x4.Translate(t) * m;
    }
}
