using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacheteProjectile : DissolveProjectile
{
    [SerializeField] GameObject graphics;
    [SerializeField] float angularSpeed;

    protected override void OnEnable()
    {
        base.OnEnable();
        graphics.transform.rotation = Quaternion.identity;
    }

    Vector3 newRotation;
    protected override void Update()
    {
        base.Update();

        if (isMoving)
            graphics.transform.Rotate(angularSpeed * Time.deltaTime, 0f, 0f);
    }
}
