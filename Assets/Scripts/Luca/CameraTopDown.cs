using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Utility.Patterns;

//[ExecuteInEditMode]
public class CameraTopDown : Singleton<CameraTopDown>
{
    [SerializeField] PlayerController player;
    [SerializeField] float maxDistanceX;
    [SerializeField] float maxDistanceZ;
    [SerializeField] float verticalOffset;
    [SerializeField] float speed = 10f;

    Vector3 TargetPosition { get { return player.Pos + GetOffsetVector() + Vector3.forward * verticalOffset; } }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, KeepTransformY(TargetPosition), speed * Time.deltaTime);
    }

    Vector3 KeepTransformY(Vector3 source)
    {
        return UMath.NewVector(source.x, transform.position.y, source.z);
    }

    Vector3 GetOffsetVector()
    {
        return UMath.AxisProduct(UMath.GetXZ(MouseRaycaster.Hit.point - player.Pos).normalized, UMath.NewVector(maxDistanceX, 0f, maxDistanceZ));
    }
}
