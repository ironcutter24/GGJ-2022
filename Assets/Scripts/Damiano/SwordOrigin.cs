using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOrigin : MonoBehaviour
{
    Projectile currentSword;

    private void Start()
    {
        FloatingSwords.Instance.OnReload += GenerateSword;
        GenerateSword();
    }

    private void OnDestroy()
    {
        FloatingSwords.Instance.OnReload -= GenerateSword;
    }

    public void ShootAt(Vector3 targetPosition)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, targetPosition - transform.position);
        currentSword.transform.rotation = rotation;
        currentSword.StartMoving();

        currentSword = null;
    }

    void GenerateSword(float delay = 0f)
    {
        if (currentSword == null)
            currentSword = ObjectPooler.Spawn("SwordProjectile", transform.position, Quaternion.identity, this.transform);
    }
}
