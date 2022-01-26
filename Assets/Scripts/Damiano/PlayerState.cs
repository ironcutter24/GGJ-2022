using System;
using System.Collections;
using System.Collections.Generic;
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

    ///<summary>
    ///<para>Parameter is interpolation between 0f (Prey) and 1f (Hunter)</para>
    ///</summary>
    public static Action<float> OnStateTransition;
    public static Action OnSwitchToHunter;
    public static Action OnSwitchToPrey;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ChangeHunterState();
    }

    void ChangeHunterState()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        _isHunter = !_isHunter;

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
}
