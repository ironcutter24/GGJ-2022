using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;

    [SerializeField] float moveSpeed;

    public Vector3 Pos { get { return rb.position; } }

    Vector3 move = Vector3.zero;
    public Vector3 Move { get { return _instance.move; } }

    Vector3 lookDirection;
    public Vector3 LookDir { get { return lookDirection; } }

    private void Update()
    {
        move = GetDirectionalInput();

        anim.SetFloat("Horizontal", move.x);
        anim.SetFloat("Vertical", move.z);
        anim.SetFloat("MoveSpeed", move.magnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);

        lookDirection = Utility.UMath.GetXZ(MouseRaycaster.Hit.point - rb.position).normalized;
        rb.MoveRotation(Quaternion.LookRotation(lookDirection, Vector3.up));
    }

    Vector3 GetDirectionalInput()
    {
        Vector3 input = Vector3.zero;
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");

        if (!IsCircaZero(input.x) || !IsCircaZero(input.z))
            return input.normalized;
        else
            return Vector3.zero;


        bool IsCircaZero(float value) { return Mathf.Approximately(value, 0f); }
    }
}
