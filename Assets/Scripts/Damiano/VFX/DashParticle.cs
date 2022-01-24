using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class DashParticle : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRend;
    [SerializeField] public float transitionDuration = 1f;

    private void Start()
    {
        OnEnable();
        color = meshRend.material.color;
    }

    private void OnEnable()
    {
        StartCoroutine(_AlphaDecay(transitionDuration));
    }

    IEnumerator<float> _AlphaDecay(float duration/*, System.Func<> callback*/)
    {
        float speed = 1 / duration;
        float interpolation = 0f;
        while (interpolation < 1f)
        {
            SetAlpha(1 - interpolation);
            interpolation += speed * Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        interpolation = 1f;
        SetAlpha(1 - interpolation);

        gameObject.SetActive(false);
    }

    Color color;
    void SetAlpha(float alpha)
    {
        color.a = alpha;
        meshRend.material.color = color;
    }
}
