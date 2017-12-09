using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSpread : SwarmState
{
	public override Vector3 CalcPawnMovement(SwarmController.PawnInfo info)
	{
		return Swarm.CalcDefaultMovement(info);
	}

	public override Vector3 CalcPawnFireDirection(SwarmController.PawnInfo info)
	{
		return new Vector3(info.FormationOffset.x, 0, info.FormationOffset.y).normalized;
	}
}
