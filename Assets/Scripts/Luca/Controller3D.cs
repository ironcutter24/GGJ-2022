using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;
using Utility.Patterns;
using MEC;

public class Controller3D : Singleton<Controller3D>, ITargetable
{
    [Header("Components")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    
    [Header("Movement")]
    [SerializeField] float preySpeed;
    [SerializeField] float hunterSpeed;
    public float MoveSpeed { get { return PlayerState.IsHunter ? hunterSpeed : preySpeed; } }

    [Header("Prey")]
    [SerializeField] float invisibilityDuration;

    [Header("Hunter")]
    [SerializeField] int dashNumber;
    private int dashesLeft;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float healthRegenAmount;

    #region Variables

    private bool _isInvisible;
    public static bool IsInvisible { get { return _instance._isInvisible; } }
    
    [SerializeField] float maxHealth;
    private float _health;

    private enum State { Moving, Dashing }
    private State state = State.Moving;

    public Vector3 Pos { get { return _instance.rb.position; } }

    private Vector3 move = Vector3.zero;
    public Vector3 Move { get { return _instance.move; } }

    private Vector3 lookDirection;
    public Vector3 LookDir { get { return lookDirection; } }

    #endregion

    Timer dashTimer = new Timer();

    protected override void Awake()
	{
        _health = maxHealth;
        StartCoroutine(DelayedDamage(2));//TODO remove
        StartCoroutine(DelayedDamage(2));
        StartCoroutine(DelayedDamage(2)); 
        base.Awake();
        PlayerState.OnSwitchToHunter += OnSwitchToHunter;
    }

    private void OnDestroy()
    {
        PlayerState.OnSwitchToHunter -= OnSwitchToHunter;
    }

    void OnSwitchToHunter()
    {
        dashesLeft = dashNumber;
    }

    private void Update()
    {
        move = GetDirectionalInput();
        lookDirection = Utility.UMath.GetXZ(MouseRaycaster.Hit.point - rb.position).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerState.IsHunter && dashTimer.IsExpired && dashesLeft > 0 && move != Vector3.zero)
        {
            StartCoroutine(_Dash(dashDuration));
            dashTimer.Set(dashDuration);
            dashesLeft--;
        }

        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(_Invisibility());
    }

    void FixedUpdate()
    {
        if(state == State.Moving)
        {
            rb.MovePosition(rb.position + move * MoveSpeed * Time.deltaTime);
            rb.MoveRotation(Quaternion.LookRotation(lookDirection, Vector3.up));
            SetAnimation();
        }
    }

	public void ApplyDamage(float amount)
    {
	    _health -= amount;
        Singleton<HUD>._instance.SetHealthBar(_health);
    }
    
	IEnumerator DelayedDamage(float time) //TODO remove
	{
		yield return new WaitForSeconds(time);
		ApplyDamage(1);
	}


    IEnumerator _Invisibility()
    {
        _isInvisible = true;

        yield return new WaitForSeconds(invisibilityDuration);

        _isInvisible = false;
    }

    public void RegenHealth()
    {
        _health += healthRegenAmount;
    }

    #region Motion-related

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

    [SerializeField] DashTrail dashTrail;
    IEnumerator<float> _Dash(float duration)
    {
        //Physics.IgnoreLayerCollision(0, 0, true);  // Disable collision with enemies
        state = State.Dashing;

        Vector3 dashDirection = move;
        dashTrail.Init(transform.position, transform.rotation, dashDirection);

        float timer = duration;
        float deltaSpace;
        float distanceTraveled = 0f;
        while (timer > 0f)
        {
            deltaSpace = dashSpeed * Time.deltaTime;

            distanceTraveled += deltaSpace;
            dashTrail.Process(distanceTraveled);

            rb.MovePosition(rb.position + dashDirection * deltaSpace);
            timer -= Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        //Physics.IgnoreLayerCollision(0, 0, false);  // Enable collision with enemies
        state = State.Moving;
    }

    #endregion

    
    void SetAnimation()
    {
        Vector3 relativeMove = transform.InverseTransformDirection(move);

        anim.SetFloat("Horizontal", relativeMove.x);
        anim.SetFloat("Vertical", relativeMove.z);
        anim.SetFloat("MoveSpeed", move.magnitude);
    }
}
