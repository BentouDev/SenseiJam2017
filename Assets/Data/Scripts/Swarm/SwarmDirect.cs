using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmDirect : SwarmState
{
	public string LookX = "Look X";
	public string LookY = "Look Y";

	private Vector3 FireDirection; 

	public override Vector3 CalcPawnFireDirection(SwarmController.PawnInfo info)
	{
		var x = Input.GetAxis(LookX);
		var y = Input.GetAxis(LookY);
		
		FireDirection = new Vector3(x, 0, y).normalized;

		return FireDirection;
	}
}
