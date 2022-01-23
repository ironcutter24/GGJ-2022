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
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;

    private bool isHunter = false;
    public static bool IsPrey { get { return !_instance.isHunter; } }
    public static bool IsHunter { get { return _instance.isHunter; } }

    enum State { Moving, Dashing }
    State state = State.Moving;

    public Vector3 Pos { get { return rb.position; } }

    Vector3 move = Vector3.zero;
    public Vector3 Move { get { return _instance.move; } }

    Vector3 lookDirection;
    public Vector3 LookDir { get { return lookDirection; } }

    private void Update()
    {
        move = GetDirectionalInput();
        lookDirection = Utility.UMath.GetXZ(MouseRaycaster.Hit.point - rb.position).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(_Dash(dashDuration));
    }

    void FixedUpdate()
    {
        if(state == State.Moving)
        {
            rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);
            rb.MoveRotation(Quaternion.LookRotation(lookDirection, Vector3.up));
            SetAnimation();
        }
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

    IEnumerator _Dash(float duration)
    {
        //Physics.IgnoreLayerCollision(0, 0, true);  // Disable collision with enemies
        state = State.Dashing;

        Vector3 dashDirection = move;

        float timer = duration;
        while(timer > 0f)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //Physics.IgnoreLayerCollision(0, 0, false);  // Enable collision with enemies
        state = State.Moving;
    }
}
