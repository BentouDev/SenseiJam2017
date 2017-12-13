using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmDirect : SwarmState
{
	public PrefabPool ProjectilePrefab;
	
	public string Shoot = "Shoot";

	public override Vector3 CalcMasterMovement()
	{
		return Swarm.SwarmDirection;
	}

	public override Vector3 CalcPawnMovement(SwarmController.PawnInfo info)
	{
		return Swarm.CalcDefaultMovement(info);
	}

	public override Vector3 CalcPawnFireDirection(SwarmController.PawnInfo info)
	{	
		if (Mathf.Abs(Swarm.LookDirection.x) > 0.01f || Mathf.Abs(Swarm.LookDirection.z) > 0.01f)
			Swarm.FireDirection = Swarm.LookDirection.normalized;

		return Swarm.FireDirection;
	}

	protected override void OnTick()
	{
		if (Input.GetButton(Shoot))
		{
			foreach (SwarmController.PawnInfo info in Swarm.Pawns)
			{
				info.Model.WalkingShooting.Shoot(ProjectilePrefab);
			}
		}
	}
}
