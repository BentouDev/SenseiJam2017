using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmLock : SwarmState
{	
    public string LookX = "Look X";
    public string LookY = "Look Y";

    public float GrowSpeed = 0.25f;
    
    [Range(0, 1)]
    public float MinAngle = 0;
    
    [Range(0, 1)]
    public float MaxAngle = 1;
    
    [Range(0, 1)]
    public float Angle;

    public override Vector3 CalcMasterMovement()
    {
        return Vector3.zero;
    }

    public override Vector3 CalcPawnMovement(SwarmController.PawnInfo info)
    {
        return Vector3.zero;
    }

    public override Vector3 CalcPawnFireDirection(SwarmController.PawnInfo info)
    {
        var x = Input.GetAxis(LookX);
        var y = Input.GetAxis(LookY);
		
        if (Mathf.Abs(x) > 0.01f || Mathf.Abs(y) > 0.01f)
            Swarm.FireDirection = new Vector3(x, 0, y).normalized;

        var custom = new Vector3(info.FormationOffset.x, 0, info.FormationOffset.y).normalized;
        
        return Vector3.Lerp(Swarm.FireDirection, custom, Angle);
    }

    protected override void OnTick()
    {
        Angle += Swarm.SwarmDirection.y * GrowSpeed * Time.deltaTime;
        Angle  = Mathf.Clamp(Angle, MinAngle, MaxAngle);

//        if (Mathf.Abs(Swarm.SwarmDirection.y) > 0.01f)
//        {
//            
////            if (Swarm.SwarmDirection.y > 0)
////            {
////                DoShrink();
////            }
////            else
////            {
////                DoGrow();
////            }
//        }
    }
    
    void DoShrink()
    {
        Angle -= GrowSpeed * Time.deltaTime;
    }

    void DoGrow()
    {
        Angle += GrowSpeed * Time.deltaTime;
    }
}
