using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleController : MonoBehaviour {
    public Animator anim;
    public float speed = 1.0f;
    public float releaseSpeedFactor = 10.0f;
    public float sensitivity = 1.0f;

    // For path invalidation
    public CharacController charac;

    private float targetZAngle = 0;
    private float direction = 1;

    private Vector2 startDragPosition;
    private float startDragZAngle;
    private bool isDragging = false;

    private bool locked = false;

    void Start()
    {
        targetZAngle = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        if (isDragging)
        {
            float deltaX = Input.mousePosition.x - startDragPosition.x;
            targetZAngle = (startDragZAngle + deltaX * sensitivity + 360) % 360;

            if (charac && charac.IsWalking())
            {
                charac.FindPath();
            }
        }

        float d = targetZAngle - transform.rotation.eulerAngles.z;
        float deltaAngle = Mathf.Min(Mathf.Min(Mathf.Abs(d), Mathf.Abs(360 - d)), Mathf.Abs(d + 360));
        // shorter direction
        direction = Mathf.Sign(d);
        if (Mathf.Abs(d) > 180)
        {
            direction = -direction;
        }

        if (deltaAngle >= 1e-4)
        {
            float actualSpeed = speed;
            if (!isDragging)
            {
                actualSpeed = deltaAngle * releaseSpeedFactor;
            }
            transform.Rotate(Vector3.forward, direction * Mathf.Min(deltaAngle, actualSpeed * Time.deltaTime));
        }

        if (!isDragging && !locked)
        {
            anim.SetBool("locked", false);
        }
    }

    public void StartDrag()
    {
        if (locked)
        {
            return;
        }

        isDragging = true;
        anim.SetBool("locked", true);
        startDragPosition = Input.mousePosition;
        startDragZAngle = transform.rotation.eulerAngles.z;
    }
    public void StopDrag()
    {
        isDragging = false;
        anim.SetBool("locked", locked);
        targetZAngle = NearestLegalAngle(targetZAngle);
    }

    public void Lock()
    {
        locked = true;
        if (isDragging)
        {
            StopDrag();
        }
        anim.SetBool("locked", true);
    }

    public void Unlock()
    {
        locked = false;
    }

    float NearestLegalAngle(float angle)
    {
        return (Mathf.Round(angle / 90) * 90) % 360;
    }
}
