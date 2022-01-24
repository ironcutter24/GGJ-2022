using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
    [SerializeField] GameObject trapPrefab;

    Queue<GameObject> activeTraps = new Queue<GameObject>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && PlayerController.IsPrey)
        {
            Debug.Log("Place trap not implemented");
        }
    }

    void PlaceTrap()
    {
        var temp = Instantiate(trapPrefab, transform.position, Quaternion.identity, null);

        activeTraps.Enqueue(temp);


    }
}
