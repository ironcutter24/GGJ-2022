using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
	[SerializeField] GameObject trapPrefab;
	public Vector3 trapOffset;
    Queue<GameObject> activeTraps = new Queue<GameObject>();

    private void Update()
    {
	    if(Input.GetKeyDown(KeyCode.Mouse1) && PlayerController.IsPrey)
        {
		    //Debug.Log("Place trap not implemented");
		    PlaceTrap();
		   
        }
	    
    }

	void PlaceTrap()
	{
		
		var temp = Instantiate(trapPrefab, transform.position + trapOffset, Quaternion.identity, null);

        activeTraps.Enqueue(temp);
    }
}
