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

    private void Awake()
    {
        PlayerState.OnStateTransition += SetLighting;
        SetLighting(0f);
    }

    private void OnDestroy()
    {
        PlayerState.OnStateTransition -= SetLighting;
    }

    void SetLighting(float parameter)
    {
        directional.color = directionalGradient.Evaluate(parameter);

        skybox.SetColor("Tint Color", skyboxGradient.Evaluate(parameter));
        //skybox.SetFloat("Exposure", (exposureHunter - exposurePrey) * parameter + exposurePrey);
    }
}
