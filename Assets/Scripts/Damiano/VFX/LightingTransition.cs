using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class LightingTransition : MonoBehaviour
{
    [Header("Directional Light")]
    [SerializeField] Light directional;
    [SerializeField] Gradient directionalGradient;

    [Header("Skybox")]
    [SerializeField] Material skybox;
    [SerializeField] Gradient skyboxGradient;
    [SerializeField] float exposurePrey;
    [SerializeField] float exposureHunter;

    private void Start()
    {
        //ToPrey();

        ToHunter();
    }

    public void ToPrey()
    {
        StopAllCoroutines();
        StartCoroutine(_Transition(false));
    }

    public void ToHunter()
    {
        StopAllCoroutines();
        StartCoroutine(_Transition(true));
    }

    IEnumerator<float> _Transition(bool isForward)
    {
        float parameter = 0f;

        while (parameter < 1f)
        {
            SetLighting(isForward ? parameter : 1 - parameter);

            parameter += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        parameter = 1f;
        SetLighting(isForward ? parameter : 1 - parameter);
    }

    void SetLighting(float parameter)
    {
        directional.color = directionalGradient.Evaluate(parameter);

        skybox.SetColor("Tint Color", skyboxGradient.Evaluate(parameter));
        skybox.SetFloat("Exposure", (exposureHunter - exposurePrey) * parameter + exposurePrey);
    }
}
