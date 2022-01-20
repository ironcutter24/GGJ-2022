using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile : Projectile
{
    private void Update()
    {
        if (!isMoving)
        {
            transform.LookAt(FloatingSwords.TargetPosition);
        }
    }
}
