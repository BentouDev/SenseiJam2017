using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaModel : MonoBehaviour
{
    public float RotateSpeed = 2;

    private Vector3 oldForward;

    private void Start()
    {
        oldForward = transform.forward;
    }

    public void SetDirection(Vector3 direction)
    {
        if (direction.magnitude > 0.01f)
        {
            var target = Vector3.Slerp(oldForward, direction, Time.deltaTime * RotateSpeed);
                target.y = 0;
            
            transform.forward = target;
            oldForward = target;
        }
    }
}
