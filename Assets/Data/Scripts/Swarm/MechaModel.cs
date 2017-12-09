using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaModel : MonoBehaviour
{
    public void SetDirection(Vector3 direction)
    {
        if (direction.magnitude > 0.01f)
            transform.forward = direction;
    }
}
