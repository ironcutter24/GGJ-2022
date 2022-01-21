using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOrigin : MonoBehaviour
{
    Projectile currentSword;

    private void Start()
    {
        FloatingSwords.Instance.OnReload += GenerateSwordAfter;
        TryGenerateSword();
    }

    private void OnDestroy()
    {
        FloatingSwords.Instance.OnReload -= GenerateSwordAfter;
    }

    public void ShootAt(Vector3 targetPosition)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, targetPosition - transform.position);
        currentSword.transform.rotation = rotation;
        currentSword.StartMoving();

        currentSword = null;
    }

    public void GenerateSwordAfter(float delay = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(_GenerateSwordAfter(delay));
    }

    IEnumerator _GenerateSwordAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        TryGenerateSword();
    }

    void TryGenerateSword(float delay = 0f)
    {
        if (currentSword == null)
            currentSword = ObjectPooler.Spawn("SwordProjectile", transform.position, Quaternion.identity, this.transform);
    }
}
