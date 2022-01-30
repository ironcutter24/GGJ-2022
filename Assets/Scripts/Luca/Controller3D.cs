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
    [SerializeField] int healthRegenAmount;

    #region Variables

    private bool _isInvisible;
    public static bool IsInvisible { get { return _instance._isInvisible; } }
    
    [SerializeField] int maxHealth;
    private int _health;
    public int Health { get { return _health; } }

    private enum State { Moving, Dashing, Disabled }
    private State state = State.Moving;

    public Vector3 Pos { get { return _instance.rb.position; } }

    private Vector3 move = Vector3.zero;
    public Vector3 Move { get { return _instance.move; } }

    private Vector3 lookDirection;
    public Vector3 LookDir { get { return lookDirection; } }

    private Vector3 spawnPosition = Vector3.zero;
    public Vector3 SpawnPosition { get { return spawnPosition; } }

    #endregion

    Timer dashTimer = new Timer();

    protected override void Awake()
	{
        base.Awake();
		PlayerState.OnSwitchToHunter += OnSwitchToHunter;
        
        _health = maxHealth;
        spawnPosition = rb.position;
    }

    private void Start()
    {
        MusicManager.SetMusicalTheme(MusicManager.Theme.Exploration);
    }

    private void OnDestroy()
    {
        PlayerState.OnSwitchToHunter -= OnSwitchToHunter;
    }

    void OnSwitchToHunter()
    {
	    dashesLeft = dashNumber;
	    
	    if(HUD.Instance != null)
		    HUD.Instance.setResourceMax(dashNumber);
    }

    public static void DisableController()
    {
        _instance.state = State.Disabled;

        _instance.anim.SetFloat("Horizontal", 0f);
        _instance.anim.SetFloat("Vertical", 0f);
        _instance.anim.SetFloat("MoveSpeed", 0f);
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
            
	        if(HUD.Instance != null)
	        	HUD.Instance.setResourceCurrent(dashesLeft);
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //    StartCoroutine(_Invisibility());
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

	public void ApplyDamage(AttackMessage attack)
    {
	    _health = Mathf.Clamp(_health - attack.damage, 0, maxHealth);

        if(HUD.Instance != null)
            HUD.Instance.SetHealthBar(_health);

        Debug.Log("Player hit! Health: " + _health);

        if(_health <= 0)
        {
            rb.MovePosition(spawnPosition);
            _health = maxHealth;
            HUD.Instance.SetHealthBar(_health);
        }
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
