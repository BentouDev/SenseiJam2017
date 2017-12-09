using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmDirect : SwarmState
{
	public string LookX = "Look X";
	public string LookY = "Look Y";

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
}
