using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
	[SerializeField] GameObject trapPrefab;
	[SerializeField] int trapsMaxNumber = 2;

	[SerializeField] Vector3 trapOffset;
    Queue<GameObject> activeTraps = new Queue<GameObject>();

    private void Update()
    {
	    if(Input.GetKeyDown(KeyCode.LeftShift) && PlayerState.IsPrey)
		    PlaceTrap();
    }

	void PlaceTrap()
	{
		var temp = Instantiate(trapPrefab, transform.position + trapOffset, transform.rotation, null);
        activeTraps.Enqueue(temp);

		if(activeTraps.Count > trapsMaxNumber)
        {
			//var temp = activeTraps.Peek();

			//temp. disable
        }
    }
}
