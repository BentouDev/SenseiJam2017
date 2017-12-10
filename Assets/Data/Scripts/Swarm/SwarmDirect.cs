using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmDirect : SwarmState
{
	public PrefabPool ProjectilePrefab;
	
	public string LookX = "Look X";
	public string LookY = "Look Y";
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
		var x = Input.GetAxis(LookX);
		var y = Input.GetAxis(LookY);
		
		if (Mathf.Abs(x) > 0.01f || Mathf.Abs(y) > 0.01f)
			Swarm.FireDirection = new Vector3(x, 0, y).normalized;

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
