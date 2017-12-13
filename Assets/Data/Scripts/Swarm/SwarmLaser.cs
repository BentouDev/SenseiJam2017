using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwarmLaser : SwarmState
{
    private bool isDuringCircle;
    private int startingIndex;
    private int prevIndex;
    private int currentIndex;
    private int Spins;
    private float StartTime;
    private float SpinsPerSec;

    static List<Vector2> StaticCircle;
    static readonly Vector2[] Circle =
    {
         CalcPolarVec(0),
         CalcPolarVec(45),
         CalcPolarVec(90),
         CalcPolarVec(135),
         CalcPolarVec(180),
         CalcPolarVec(225),
         CalcPolarVec(270),
         CalcPolarVec(315),
    };

    void Start()
    {
        StaticCircle = Circle.ToList();
    }

    protected override void OnBegin()
    {
        Spins = 0;
    }

    static Vector2 CalcPolarVec(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

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
        if (Mathf.Abs(Swarm.LookDirection.x) > 0.01f || Mathf.Abs(Swarm.LookDirection.z) > 0.01f)
            Swarm.FireDirection = Swarm.LookDirection.normalized;

        return Swarm.FireDirection;
    }

    void FailedCircle()
    {
        // Spins = 0;
        isDuringCircle = false;
    }

    protected override void OnTick()
    {
        if (Swarm.SwarmDirection.magnitude < 0.01f)
        {
            FailedCircle();
            return;
        }
        
        var index = GetIndexForCurrent();
        if (!isDuringCircle)
        {            
            if (index == -1)
            {
                FailedCircle();
                return;
            }
            else
            {
                StartTime = Time.time;
                isDuringCircle = true;
                startingIndex  = index;
                prevIndex = index;
            }
        }
        else
        {
            if (index < prevIndex)
            {
                FailedCircle();
                return;
            }

            if (Mathf.Abs(index - startingIndex) <= 2 && Mathf.Abs(prevIndex - startingIndex) >= 2)
            {
                // Did circle
                SpinsPerSec = 1 / Time.time - StartTime;
                StartTime = Time.time;
                
                startingIndex = index;
                prevIndex = index;
                Spins++;
                return;
            }

            if (index > prevIndex && index - prevIndex <= 2)
            {
                prevIndex = currentIndex;
                currentIndex = index;
            }
        }
    }

    int GetIndexForCurrent()
    {
        var flatDir     = new Vector2(Swarm.SwarmDirection.x, Swarm.SwarmDirection.z);
        var beginVector = StaticCircle.Aggregate((curMin, x) => Vector2.Distance(curMin, flatDir) < Vector2.Distance(x, flatDir) ? x : curMin);
        var index       = StaticCircle.IndexOf(beginVector);
        return index;
    }

//    void OnGUI()
//    {
//        GUI.Label(new Rect(10,10,200,30), "Spins " + Spins);
//        if (Spins > 0)
//        {
//            GUI.Label(new Rect(10,30,200,30), "Spins/Sec " + SpinsPerSec);
//        }
//    }
}
