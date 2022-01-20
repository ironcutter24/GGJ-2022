using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CameraTopDown : MonoBehaviour
{
	public Transform player;
	public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
	    if(player){
		    transform.position = Vector3.Lerp(player.position+offset,transform.position,0.5f * Time.deltaTime);
	    }
    }
}
