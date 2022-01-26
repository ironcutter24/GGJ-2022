using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
	[SerializeField] GameObject trapPrefab;
	[SerializeField] int trapsMaxNumber = 2;

	[SerializeField] Vector3 trapOffset;
    Queue<PlayerGhost> activeTraps = new Queue<PlayerGhost>();

    private void Update()
    {
	    if(Input.GetKeyDown(KeyCode.LeftShift) && PlayerState.IsPrey)
		    PlaceTrap();
    }

	void PlaceTrap()
	{
		var temp = PlayerGhostPooler.Spawn("DecoyTrap", transform.position + trapOffset, transform.rotation, null);
        activeTraps.Enqueue(temp);

		if(activeTraps.Count > trapsMaxNumber)
        {
			activeTraps.Dequeue().Dissolve();
        }
    }
}
