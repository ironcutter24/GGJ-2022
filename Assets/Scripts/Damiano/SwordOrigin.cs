using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOrigin : MonoBehaviour
{
    Projectile currentSword;

    private void Start()
    {
        GenerateSword();
    }

    public void ShootAt(Vector3 targetPosition)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, targetPosition - transform.position);
        currentSword.transform.rotation = rotation;

        currentSword.StartMoving();

        GenerateSword();
    }

    void GenerateSword()
    {
        currentSword = ObjectPooler.Spawn("SwordProjectile", transform.position, Quaternion.identity, this.transform);
    }
}
