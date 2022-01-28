using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : Enemy
{
  
    public override void SetDestination(Vector3 targetPosition)
    {
	    agent.SetDestination(targetPosition);
    }
    public override void Attack()
    {
	    System.Console.Write("JUMPER ATTACK!");
    }
}
