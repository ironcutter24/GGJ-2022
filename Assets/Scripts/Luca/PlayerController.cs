using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;

    Transform player;
    public float speedB, speedRotB;
    public float speedG, speedRotG;
    bool typePlayer;

    void Start()
    {
        player = GetComponent<Transform>();
        typePlayer = false;
    }

    Vector3 move;
    private void Update()
    {
        move = Vector3.zero;

        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");
        move.Normalize();

        anim.SetFloat("Horizontal", move.x);
        anim.SetFloat("Vertical", move.z);
        anim.SetFloat("MoveSpeed", move.magnitude);
    }

    void FixedUpdate()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * speedRotG;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * speedG;
        //player.Rotate(0, x, 0);
        //player.Translate(0, 0, z);

        player.Translate(move * speedG * Time.deltaTime);

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
