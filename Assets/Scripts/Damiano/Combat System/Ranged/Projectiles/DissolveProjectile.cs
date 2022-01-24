using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class DissolveProjectile : Projectile
{
    float fadeDuration = .4f;
    [SerializeField] MeshRenderer meshRend;

    private void Awake()
    {
        DeactivateForPooling();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        Timing.KillCoroutines("Forward" + gameObject.GetInstanceID());
        Timing.RunCoroutine(_Transition(true), "Forward" + gameObject.GetInstanceID());
    }

    protected virtual void Update()
    {
        if (!isMoving)
            transform.LookAt(FloatingSwords.TargetPosition);
    }

    IEnumerator<float> _Transition(bool isForward)
    {
        float delta = 1 / fadeDuration;

        float timeInLoop = 0f;
        float parameterValue = 0f;
        float outputValue;

        while (timeInLoop < fadeDuration)
        {
            yield return Timing.WaitForOneFrame;

            parameterValue += delta * Time.deltaTime;
            outputValue = isForward ? parameterValue : 1 - parameterValue;
            meshRend.material.SetFloat("_CutoffHeight", outputValue * 15f - 5f);

            timeInLoop += Time.deltaTime;
        }
    }

    public override void Discharge()
    {
        Timing.RunCoroutine(_Discharge());
    }

    IEnumerator<float> _Discharge()
    {
        Timing.KillCoroutines("Backward" + gameObject.GetInstanceID());


        var handle = Timing.RunCoroutine(_Transition(false), "Backward" + gameObject.GetInstanceID());
        yield return Timing.WaitUntilDone(handle);
        DeactivateForPooling();
        yield break;
    }

    protected override void DeactivateForPooling()
    {
        base.DeactivateForPooling();
        meshRend.material.SetFloat("_CutoffHeight", -5f);
    }
}
