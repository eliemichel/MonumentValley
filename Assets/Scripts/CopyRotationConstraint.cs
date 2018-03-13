using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CopyRotationConstraint : MonoBehaviour {
    public Transform target;

	void FixedUpdate () {
        if (target)
        {
            transform.rotation = target.rotation;
        }
	}
}
