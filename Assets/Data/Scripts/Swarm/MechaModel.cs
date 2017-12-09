using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class MechaModel : MonoBehaviour
{
    public float RotateSpeed = 2;
    
    [Header("Refs")]
    public Animator Anim;
    public StatePawn Pawn;

    [Header("Anims")] 
    public string Forward;
    public string Side;

    private Vector3 oldForward;

    private void Start()
    {
        oldForward = transform.forward;
    }

    public void Update()
    {
        if (!Pawn || !Anim)
            return;

        var lookDir  = transform.forward;
        var rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(lookDir.x * -1, lookDir.y, lookDir.z));
        Vector3 movement = rotation * Pawn.Velocity;
        
        if (!string.IsNullOrEmpty(Forward))
            Anim.SetFloat(Forward, movement.z / Pawn.Movement.MaxSpeed);

        if (!string.IsNullOrEmpty(Side))
            Anim.SetFloat(Side, movement.x / Pawn.Movement.MaxSpeed);
    }

    public void SetDirection(Vector3 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            var target   = Vector3.Slerp(oldForward, direction, Time.deltaTime * RotateSpeed);
                target.y = 0;
            
            transform.forward = target;
            oldForward        = target;
        }
    }
}
