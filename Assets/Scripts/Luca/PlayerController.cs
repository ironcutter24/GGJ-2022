using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
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
        lookDirection = Utility.UMath.GetXZ(MouseRaycaster.Hit.point - rb.position).normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);
        rb.MoveRotation(Quaternion.LookRotation(lookDirection, Vector3.up));
        SetAnimation();
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

    void SetAnimation()
    {
        Vector3 relativeMove = transform.InverseTransformDirection(move);

        anim.SetFloat("Horizontal", relativeMove.x);
        anim.SetFloat("Vertical", relativeMove.z);
        anim.SetFloat("MoveSpeed", move.magnitude);
    }
}
