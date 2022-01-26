using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance FMODEventInstance;

    [SerializeField] FMODUnity.EventReference fmodEvent;

    [SerializeField]
    [Range(0f, 2f)]
    private float playerState, combatEngaged, dangerLevel;

    [SerializeField]
    [Range(0, 1)]
    int explorationStart = 0;

    private void Awake()
    {
        PlayerState.OnStateTransition += ApplyInterpolation;
    }

    private void OnDestroy()
    {
        PlayerState.OnStateTransition -= ApplyInterpolation;
    }

    void Start()
    {
        FMODEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        FMODEventInstance.start();
    }

    void Update()
    {
        FMODEventInstance.setParameterByName("Switch WOLF", playerState);
        FMODEventInstance.setParameterByName("Combat", combatEngaged);
        FMODEventInstance.setParameterByName("Danger Proximity", dangerLevel);
        FMODEventInstance.setParameterByName("Start", explorationStart);
    }

    void ApplyInterpolation(float interpolation)
    {
        //FMODEventInstance.setParameterByName("Switch WOLF", interpolation * 2);
    }
}
