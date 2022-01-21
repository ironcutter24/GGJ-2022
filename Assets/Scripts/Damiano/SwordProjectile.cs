using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile : Projectile
{
    float fadeDuration = .4f;
    [SerializeField] MeshRenderer meshRend;

    protected override void OnEnable()
    {
        base.OnEnable();

        StopCoroutine(_FadeIn());
        StartCoroutine(_FadeIn());
    }

    private void Update()
    {
        if (!isMoving)
            transform.LookAt(FloatingSwords.TargetPosition);
    }

    IEnumerator _FadeIn()
    {
        float delta = 1 / fadeDuration;

        float timeInLoop = 0f;
        float parameterValue = 0f;

        while (timeInLoop < fadeDuration)
        {
            yield return new WaitForEndOfFrame();

            parameterValue += delta * Time.deltaTime;

            meshRend.material.SetFloat("_CutoffHeight", parameterValue * 15f - 5f);

            timeInLoop += Time.deltaTime;
        }
    }

    protected override void DeactivateForPooling()
    {
        base.DeactivateForPooling();

        meshRend.material.SetFloat("_CutoffHeight", -5f);
    }
}
