using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmRockets : SwarmState
{
	public float CameraMin;
	public float CameraMax;
	
	public float Power = 2.5f;

	public Transform Cursor;

	private Vector3 Direction;

	public override float GetCameraMax()
	{
		return CameraMax;
	}

	public override Vector3 CalcMasterMovement()
	{
		return Vector3.zero;
	}

	public override float GetCameraMin()
	{
		return CameraMin;
	}

	public override Vector3 CalcPawnMovement(SwarmController.PawnInfo info)
	{
		return Swarm.CalcDefaultMovement(info);
	}

	public override Vector3 CalcPawnFireDirection(SwarmController.PawnInfo info)
	{
		Direction = LimitFlatDiagonalVector(Swarm.LookDirection, 1);
		
		if (Mathf.Abs(Swarm.LookDirection.x) > 0.01f || Mathf.Abs(Swarm.LookDirection.z) > 0.01f)
			Swarm.FireDirection = Swarm.LookDirection.normalized;
		
		return Swarm.FireDirection;
	}

	protected override void OnTick()
	{
		Cursor.position = Swarm.Pawn.transform.position + Direction * (Swarm.TargetRadius * Power);
	}
	
	internal Vector3 LimitFlatDiagonalVector(Vector3 vector, float maxLength)
	{
		float pythagoras = ((vector.x * vector.x) + (vector.z * vector.z));
		if (pythagoras > (maxLength * maxLength))
		{
			float magnitude = Mathf.Sqrt(pythagoras);
			float multiplier = maxLength / magnitude;
			vector.x *= multiplier;
			vector.y *= multiplier;
		}

		return vector;
	}
}
