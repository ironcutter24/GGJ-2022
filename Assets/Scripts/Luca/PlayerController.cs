using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform player;
    public float speedB, speedRotB;
    public float speedG, speedRotG;
    bool typePlayer;

    void Start()
    {
        player = GetComponent<Transform>();
        typePlayer = false;
    }
    
    void FixedUpdate()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * speedRotG;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * speedG;
        player.Rotate(0, x, 0);
        player.Translate(0, 0, z);

        if (Input.GetKey(KeyCode.Space))
        {
            typePlayer = true;
            var xb = Input.GetAxis("Horizontal") * Time.deltaTime * speedRotB;
            var zb = Input.GetAxis("Vertical") * Time.deltaTime * speedB;
            player.Rotate(0, xb, 0);
            player.Translate(0, 0, zb);
        }
    }
}
