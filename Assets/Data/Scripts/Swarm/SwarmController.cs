using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;

public class SwarmController : Framework.Controller
{
    [System.Serializable]
    public struct PawnInfo
    {
        [SerializeField] public StatePawn Pawn;
        [SerializeField] public MechaModel Model;
        [SerializeField] public Vector2 FormationOffset;
    }
    
    [SerializeField]
    public List<PawnInfo> Pawns = new List<PawnInfo>();
    private List<StatePawn> ToDelete = new List<StatePawn>();
    private List<SwarmState> AllStates = new List<SwarmState>();
    public SwarmState StartingState;

    [Header("Positioning")]
    public Vector3 GroupCenter;

    public float Alpha = 0;
    public float TargetRadius= 15;
    public float MinRadius = 5;
    public float MaxRadius = 45;
    public float ChangeSpeed = 2;

    [Header("Camera")] 
    public ActionCamera ActionCam;
    public float MinCameraDistance;
    public float MaxCameraDistance;

    [Header("State")]
    public SwarmState CurrentState;

    public SwarmGUI GUI;

    [Header("Input")] 
    public float InputThreshold = 0.01f;
    public string MoveX = "Horizontal";
    public string MoveY = "Vertical";
    public string LookX = "Look X";
    public string LookY = "Look Y";
    public string SquadRadius = "SquadRadius";

    private Vector2 MouseInput;
    private Vector2 PadInput;

    [HideInInspector]
    public Vector3 SwarmDirection;
    public Vector3 FireDirection  { get; set; }
    public Vector3 LookDirection { get; set; }

    private int LastUnitCount;
    private static readonly float GoldenPretzel = (Mathf.Sqrt(5) + 1) * 0.5f;

    protected override void OnInit()
    {
        Pawns.AddRange(GetComponentsInChildren<StatePawn>().Select(p => new PawnInfo()
        {
            Pawn = p, 
            Model = p.GetComponentInChildren<MechaModel>()
        }));
        
        AllStates.AddRange(GetComponentsInChildren<SwarmState>());

        if (StartingState)
            CurrentState = StartingState;
        else
            CurrentState = AllStates.FirstOrDefault();

        foreach (SwarmState state in AllStates)
        {
            state.Init(this);
        }
                
        if (CurrentState)
            CurrentState.Begin();

        var poses = CalcPositions(Pawns.Count, Alpha);
        for (int index = 0; index < Pawns.Count; index++)
        {
            var info = Pawns[index];
            info.FormationOffset.Set(poses[index].x, poses[index].y);
            info.Pawn.transform.position = Pawn.transform.position + new Vector3(info.FormationOffset.x, 0, info.FormationOffset.y) * TargetRadius;
            info.Pawn.Init();

            Pawns[index] = info;
        }

        LastUnitCount = Pawns.Count;
        
        GUI.OnUpdatePositioning();
    }

    protected override void OnFixedTick()
    {
        foreach (PawnInfo info in Pawns)
        {
            info.Pawn.FixedTick();
        }
        
        Pawn.FixedTick();
    }

    protected override void OnLateTick()
    {
        ActionCam.RotatedOffset.z = Mathf.Lerp
        (
            CurrentState ? CurrentState.GetCameraMin() : MinCameraDistance, 
            CurrentState ? CurrentState.GetCameraMax() : MaxCameraDistance,
            Mathf.InverseLerp(MinRadius, MaxRadius, TargetRadius)
        );

        foreach (PawnInfo info in Pawns)
        {
            info.Pawn.LateTick();
        }
        
        Pawn.LateTick();
    }

    private void GatherPadInput()
    {
        PadInput.x = Input.GetAxis(LookX);
        PadInput.y = Input.GetAxis(LookY);
    }

    private void GatherMouseInput()
    {
        var screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        var mousePos = Input.mousePosition;
        var mouseDiff = mousePos - screenCenter;

        MouseInput = mouseDiff.normalized;        
    }

    private void GatherInput()
    {
        var radiusChange = Input.GetAxis(SquadRadius);
        if (Mathf.Abs(radiusChange) > 0.01f)
            TargetRadius = Mathf.Clamp(TargetRadius + radiusChange * ChangeSpeed * Time.deltaTime, MinRadius, MaxRadius);

        SwarmDirection.x = Input.GetAxis(MoveX);
        SwarmDirection.z = Input.GetAxis(MoveY);

        GatherMouseInput();
        GatherPadInput();

        if (PadInput.magnitude < InputThreshold && MouseInput.magnitude > InputThreshold)
        {
            LookDirection = new Vector3(MouseInput.x, 0, MouseInput.y);
        }
        else
        {
            LookDirection = new Vector3(PadInput.x, 0, PadInput.y);
        }
    }

    protected override void OnProcessControll()
    {
        // Input
        if (Enabled)
        {
            GatherInput();
        }

        if (CurrentState)
            CurrentState.Tick();

        // Update
        foreach (PawnInfo info in Pawns)
        {
            if (!info.Pawn.IsAlive())
                ToDelete.Add(info.Pawn);
            else
            {
                var move = CalcMovement(info);
                if (move.magnitude > 0.1f)
                    info.Pawn.ProcessMovement(move);
                else
                    info.Pawn.ProcessMovement(Vector3.zero);

                if (CurrentState)
                    info.Model.SetDirection(CurrentState.CalcPawnFireDirection(info));
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

        if (LastUnitCount != Pawns.Count)
        {
            LastUnitCount = Pawns.Count;
            UpdatePositioning();
        }
        
        // Update Group Center
        Pawn.ProcessMovement(CurrentState ? CurrentState.CalcMasterMovement() : SwarmDirection);
        Pawn.Tick();
    }

    public void UpdatePositioning()
    {
        GUI.OnUpdatePositioning();
        
        var poses = CalcPositions(Pawns.Count, Alpha);
        for (int index = 0; index < Pawns.Count; index++)
        {
            var info = Pawns[index];
            info.FormationOffset.Set(poses[index].x, poses[index].y);
         
            Pawns[index] = info;
        }
    }

    public Vector3 CalcDefaultMovement(PawnInfo info)
    {
        var displacement = info.Pawn.transform.position - Pawn.transform.position;
        var targetDiff   = new Vector3(info.FormationOffset.x, 0, info.FormationOffset.y) * TargetRadius;
        Vector3 direction = targetDiff - displacement;

        return direction;
    }
    
    Vector3 CalcMovement(PawnInfo info)
    {
        if (CurrentState)
            return CurrentState.CalcPawnMovement(info);

        return CalcDefaultMovement(info);
    }

    // Swarm positioning
    List<Vector2> CalcPositions(int count, float alpha)
    {
        float theB = Mathf.Round(alpha * Mathf.Sqrt(count + 1));
        float phi = GoldenPretzel;

        List<Vector2> output = new List<Vector2>();
        for (int i = 1; i <= count; i++)
        {
            float radius = CalcRadius(i, count + 1, theB);
            // float theta = 2 * phi * i / Mathf.Pow(phi, 2);
            float theta = Mathf.Deg2Rad * -i * 360 * phi;
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

    public void SwitchState(SwarmState swarmState)
    {
        if (CurrentState == swarmState)
            return;
        
        if (CurrentState)
            CurrentState.End();

        CurrentState = swarmState;

        if (CurrentState)
            CurrentState.Begin();
    }
}
