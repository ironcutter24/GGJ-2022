using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Transform player;
	public float speed,speedRot;
	
    // Start is called before the first frame update
    void Start()
    {
	    player = GetComponent<Transform>();
    }
    // Update is called once per frame
	void FixedUpdate()
    {
	    var x = Input.GetAxis("Horizontal") * Time.deltaTime * speedRot;
	    var z = Input.GetAxis("Vertical") * Time.deltaTime *speed;
	    player.Rotate(0,x,0);
	    player.Translate(0,0,z);
    }
}
