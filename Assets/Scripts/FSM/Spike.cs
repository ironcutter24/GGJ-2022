using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Enemy
{
	//[SerializeField] Rigidbody rb;

	//void FixedUpdate()
	//{
	//	Vector3 moveDir = agent.nextPosition - transform.position;
	//	moveDir.y = 0f;
	//	moveDir.Normalize();

	//	rb.MovePosition(rb.position + moveDir * agent.speed * Time.deltaTime);
	//}

	public override void SetDestination(Vector3 targetPosition)
	{
		agent.SetDestination(targetPosition);
	}

	public override void StartAttack()
	{
		base.StartAttack();
		anim.SetTrigger("AttackTrigger");
		AudioManager.SpikeMeleeAttack();
	}

	public override void StopAttack()
    {
		base.StopAttack();
	}
}
