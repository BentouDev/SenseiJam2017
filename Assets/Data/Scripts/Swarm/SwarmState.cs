using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwarmState : MonoBehaviour
{
    protected SwarmController Swarm;

    public void Init(SwarmController ctrl)
    {
        Swarm = ctrl;
        OnInit();
    }
    
    protected virtual void OnInit()
    { }

    public virtual Vector3 CalcPawnMovement(SwarmController.PawnInfo info)
    {
        return Swarm.CalcDefaultMovement(info);
    }
    
    public abstract Vector3 CalcPawnFireDirection(SwarmController.PawnInfo info);
}
