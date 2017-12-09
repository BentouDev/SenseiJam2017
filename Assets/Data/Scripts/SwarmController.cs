using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Framework;
using NUnit.Framework.Internal.Filters;
using UnityEngine;

public class SwarmController : Framework.Controller
{
    [System.Serializable]
    public struct PawnInfo
    {
        [SerializeField] public StatePawn Pawn;
        [SerializeField] public Vector2 FormationOffset;
    }
    
    [SerializeField]
    public List<PawnInfo> Pawns = new List<PawnInfo>();
    private List<StatePawn> ToDelete = new List<StatePawn>();

    public Vector3 GroupCenter;

    public float Alpha = 0;
    public float TargetRadius= 15;
    public float MinRadius = 5;
    public float MaxRadius = 45;
    public float ChangeSpeed = 2;

    public SwarmState CurrentState;

    public string Expand;
    public string Shrink;

    public Vector3 SwarmDirection;

    private static readonly float GoldenPretzel = (Mathf.Sqrt(5) + 1) * 0.5f;

    private enum FormationSize
    {
        Stopped,
        Shrinking,
        Expanding
    }

    private FormationSize RadiusMode;

    protected override void OnInit()
    {
        Pawns.AddRange(GetComponentsInChildren<StatePawn>().Select(p => new PawnInfo(){Pawn = p}));
        
        var poses = CalcPositions(Pawns.Count, Alpha);
        for (int index = 0; index < Pawns.Count; index++)
        {
            var info = Pawns[index];
            info.FormationOffset.Set(poses[index].x, poses[index].y);
            info.Pawn.transform.position = GroupCenter + new Vector3(info.FormationOffset.x, 0, info.FormationOffset.y) * TargetRadius;
            info.Pawn.Init();

            Pawns[index] = info;
        }
    }

    protected override void OnFixedTick()
    {
        foreach (PawnInfo info in Pawns)
        {
            info.Pawn.FixedTick();
        }
    }

    protected override void OnLateTick()
    {
        foreach (PawnInfo info in Pawns)
        {
            info.Pawn.LateTick();
        }
    }

    protected override void OnProcessControll()
    {
        // Input
        if (Enabled)
        {
            RadiusMode = FormationSize.Stopped;
            
            if (Input.GetButton(Expand))
            {
                RadiusMode = FormationSize.Expanding;
                // TargetRadius = Mathf.Clamp(TargetRadius + ChangeSpeed * Time.deltaTime, MinRadius, MaxRadius);
            }

            if (Input.GetButton(Shrink))
            {
                RadiusMode = FormationSize.Shrinking;
                // TargetRadius = Mathf.Clamp(TargetRadius - ChangeSpeed * Time.deltaTime, MinRadius, MaxRadius);
            }
        }

        // Update
        foreach (PawnInfo info in Pawns)
        {
            if (!info.Pawn.IsAlive())
                ToDelete.Add(info.Pawn);
            else
            {
                info.Pawn.DesiredForward = SwarmDirection;
                info.Pawn.ProcessMovement(CalcMovement(info));
            }
        }
        
        foreach (PawnInfo info in Pawns)
        {
            info.Pawn.Tick();
        }

        // Deletion
        foreach (StatePawn pawn in ToDelete)
        {
            Pawns.Remove(Pawns.First(i => i.Pawn == pawn));
            DestroyObject(pawn.gameObject);
        }
        
        ToDelete.Clear();
    }

    Vector3 CalcMovement(PawnInfo info)
    {
        Vector3 direction = SwarmDirection;
        switch (RadiusMode)
        {
            case FormationSize.Expanding:
                direction += new Vector3(info.FormationOffset.x, 0, info.FormationOffset.y) * ChangeSpeed;
                break;
            case FormationSize.Shrinking:
                direction -= new Vector3(info.FormationOffset.x, 0, info.FormationOffset.y) * ChangeSpeed;
                break;
        }

        return direction;
    }

    List<Vector2> CalcPositions(int count, float alpha)
    {
        float theB = Mathf.Round(alpha * Mathf.Sqrt(count + 1));
        float phi = GoldenPretzel;

        List<Vector2> output = new List<Vector2>();
        for (int i = 1; i <= count; i++)
        {
            float radius = CalcRadius(i, count + 1, theB);
            float theta = 2 * phi * i / Mathf.Pow(phi, 2);
            output.Add(new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta)));
        }

        return output;
    }

    float CalcRadius(int index, int count, float theB)
    {
        float radius = 0;
        
        if (index > count - theB)
            radius = 1;
        else
            radius = Mathf.Sqrt(index - 1 * 0.5f) / Mathf.Sqrt(count - (theB + 1) * 0.5f);
        
        return radius;
    }
}
