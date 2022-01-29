using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MEC;
using Utility;
using Utility.Patterns;

public class PlayerState : Singleton<PlayerState>
{
    [SerializeField] float transitionDuration = 1f;

    private bool _isHunter = false;
    public static bool IsPrey { get { return !_instance._isHunter; } }
    public static bool IsHunter { get { return _instance._isHunter; } }

    private List<Enemy> _nearEnemies = new List<Enemy>();
    private int _engagedEnemies;
    public static bool IsInCombat { get { return _instance._engagedEnemies > 0; } }

    public static int EngagedEnemies
    {
        get
        {
            return _instance._engagedEnemies;
        }
        set
        {
            _instance._engagedEnemies = (value >= 0 ? value : 0);
            if(_instance._engagedEnemies > 0)
                OnBattleEngaged();
            else
                OnBattleDisengaged();
        }
    }

    ///<summary>
    ///<para>Parameter is interpolation between 0f (Prey) and 1f (Hunter)</para>
    ///</summary>
    public static Action<float> OnStateTransition;

    public static Action OnSwitchToHunter;
    public static Action OnSwitchToPrey;

    public static Action OnBattleEngaged;
    public static Action OnBattleDisengaged;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ChangeHunterState();

        if(_nearEnemies.Count > 0)
        {
            Enemy e = _nearEnemies.OrderBy(x => x.DistanceFromPlayer).First();
            AudioManager.SetDangerProximity(UMath.Normalize(e.DistanceFromPlayer, e.DangerDistanceMax, e.DangerDistanceMin));
        }
    }

    private int _successfulAttacksHunter = 0;
    private int _successfulAttacksPrey = 0;
    public bool HasBeenMostlyHunter { get { return _successfulAttacksHunter > _successfulAttacksPrey; } }

    public static void RecordSuccessfulAttack(AttackMessage attackData)
    {
        var proj = attackData.source.GetComponent<Projectile>();

        if (proj == null)
            _instance._successfulAttacksPrey++;
        else
            _instance._successfulAttacksHunter++;
    }

    void ChangeHunterState()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        _isHunter = !_isHunter;
	   
	    if(HUD.Instance != null)
		    HUD.Instance.isPrey = !_instance._isHunter;

        if (_isHunter)
        {
            Util.TryAction(OnSwitchToHunter);
            Timing.RunCoroutine(_Transition(transitionDuration, true));
        }
        else
        {
            Util.TryAction(OnSwitchToPrey);
            Timing.RunCoroutine(_Transition(transitionDuration, false));
        }
    }

    bool isTransitioning = false;
    IEnumerator<float> _Transition(float duration, bool isForward)
    {
        float speed = 1 / duration;
        float interpolation = 0f;
        while (interpolation < 1f)
        {
            ApplyInterpolation();
            interpolation += speed * Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        interpolation = 1f;
        ApplyInterpolation();

        isTransitioning = false;

        void ApplyInterpolation() { Util.TryAction(OnStateTransition, isForward ? interpolation : 1 - interpolation); }
    }

    #region Enemy proximity

    public static void AddNearEnemy(Enemy enemy) { _instance._nearEnemies.Add(enemy); }

    public static void RemoveNearEnemy(Enemy enemy) { _instance._nearEnemies.Remove(enemy); }

    #endregion
}
