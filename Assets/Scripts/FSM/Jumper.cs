using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : Enemy
{
    #region Movement

    public override void SetDestination(Vector3 targetPosition)
	{
		agent.SetDestination(targetPosition);
		PauseMovement();

		//StartCoroutine(_TeleportTo(targetPosition));
	}

	IEnumerator _TeleportTo(Vector3 destination)
    {
		// Fade out

		// Move

		// Fade in

		yield return null;
    }

    #endregion

    public override void StartAttack()
	{
		throw new System.NotImplementedException();
	}
}
