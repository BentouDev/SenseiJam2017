using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwarmState : MonoBehaviour
{
    public string SwitchButton;
    
    protected SwarmController Swarm;

    public void Init(SwarmController ctrl)
    {
        Swarm = ctrl;
        OnInit();
    }
    
    protected virtual void OnInit()
    { }

    public virtual float GetCameraMin()
    {
        return Swarm.MinCameraDistance;
    }

    public virtual float GetCameraMax()
    {
        return Swarm.MaxCameraDistance;
    }

    public abstract Vector3 CalcMasterMovement();

    public abstract Vector3 CalcPawnMovement(SwarmController.PawnInfo info);
    
    public abstract Vector3 CalcPawnFireDirection(SwarmController.PawnInfo info);

    public void Begin()
    {
        OnBegin();  
    }
    
    protected virtual void OnBegin()
    { }

    public void End()
    {
        OnEnd();        
    }

    protected virtual void OnEnd()
    { }

    public void Update()
    {
        if (Input.GetButtonDown(SwitchButton) && Swarm)
        {
            Swarm.SwitchState(this);
        }
    }

    public void Tick()
    {
        OnTick();
    }
    
    protected virtual void OnTick()
    { }
}
