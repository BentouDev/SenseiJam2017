using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class Follow : Controller
{
    public StatePawn Target;

    protected override void OnProcessControll()
    {
        Vector3 direction = Vector3.zero;
        if (Enabled && Pawn.IsAlive())
        {
            var distance = Target.transform.position - Pawn.transform.position;
            direction = distance.normalized;
        }
        
        Pawn.ProcessMovement(direction);
        Pawn.Tick();
    }

    protected override void OnFixedTick()
    {
        Pawn.FixedTick();
    }

    protected override void OnLateTick()
    {
        Pawn.LateTick();
    }
}
