﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Enemy
{
	public override void SetDestination(Vector3 targetPosition)
	{
		agent.SetDestination(targetPosition);
	}

	public override void Attack()
	{
		PauseMovement();
		anim.SetTrigger("AttackTrigger");
	}
}
