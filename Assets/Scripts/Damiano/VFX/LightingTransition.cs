using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using MEC;

public class LightingTransition : MonoBehaviour
{
    [Header("Directional Light")]
    [SerializeField] Light directional;
    [SerializeField] Gradient directionalGradient;

    [Header("Skybox")]
    [SerializeField] Material skybox;
    [SerializeField] Gradient skyboxGradient;
    //[SerializeField] float exposurePrey;
    //[SerializeField] float exposureHunter;

    [Header("Parameters")]
    [SerializeField] float duration = 1f;
    [SerializeField] bool testMode = false;

    private void Start()
    {
        SetLighting(0f);

        if(testMode)
            Timing.RunCoroutine(_Test());
    }

    IEnumerator<float> _Test()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(duration + 2f);
            ToHunter();
            yield return Timing.WaitForSeconds(duration + 2f);
            ToPrey();
        }
    }

    public void ToPrey()
    {
        string timingID = "Backward" + gameObject.GetInstanceID();
        Timing.KillCoroutines(timingID);
        Timing.RunCoroutine(_Transition(duration, false), timingID);
    }

    public void ToHunter()
    {
        string timingID = "Forward" + gameObject.GetInstanceID();
        Timing.KillCoroutines(timingID);
        Timing.RunCoroutine(_Transition(duration, true), timingID);
    }

    IEnumerator<float> _Transition(float duration, bool isForward)
    {
        float speed = 1 / duration;
        float interpolation = 0f;
        while (interpolation < 1f)
        {
            SetLighting(isForward ? interpolation : 1 - interpolation);
            interpolation += speed * Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        interpolation = 1f;
        SetLighting(isForward ? interpolation : 1 - interpolation);
    }

    void SetLighting(float parameter)
    {
        directional.color = directionalGradient.Evaluate(parameter);

        skybox.SetColor("Tint Color", skyboxGradient.Evaluate(parameter));
        //skybox.SetFloat("Exposure", (exposureHunter - exposurePrey) * parameter + exposurePrey);
    }
}
