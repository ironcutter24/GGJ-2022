using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer meshRend;
    [SerializeField] public float transitionDuration = 1f;

    [SerializeField] protected Animator anim;
    Color color;
    float startAlpha;

    protected virtual void Start()
    {
        color = meshRend.material.color;
        startAlpha = color.a;
        OnEnable();
    }

    protected virtual void OnEnable()
    {
        anim.SetFloat("Horizontal", 0f);
        anim.SetFloat("Vertical", 0f);
        anim.SetFloat("MoveSpeed", 0f);
    }

    public void Dissolve()
    {
        StopAllCoroutines();
        StartCoroutine(_AlphaDecay(transitionDuration));


        IEnumerator _AlphaDecay(float duration)
        {
            float speed = 1 / duration;
            float interpolation = 0f;
            while (interpolation < 1f)
            {
                SetAlpha(Mathf.Lerp(startAlpha, 0f, interpolation));
                interpolation += speed * Time.deltaTime;
                yield return null;
            }
            interpolation = 1f;
            SetAlpha(startAlpha);

            gameObject.SetActive(false);
        }
    }

    void SetAlpha(float alpha)
    {
        color.a = alpha;
        meshRend.material.color = color;
    }
}
