using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Enemy
{
	public override void Attack()
	{
		System.Console.Write("SPIKE ATTACK!");
	}

	public override void SetDestination(Vector3 targetPosition)
	{
		agent.SetDestination(targetPosition);
	}
}
