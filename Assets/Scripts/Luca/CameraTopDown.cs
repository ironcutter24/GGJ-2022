using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Utility.Patterns;

public class CameraTopDown : Singleton<CameraTopDown>
{
    [SerializeField] float speed = 10f;

    [Header("Bounds")]
    [SerializeField] float maxDistanceX;
    [SerializeField] float maxDistanceZ;

    [Header("Positioning")]
    [SerializeField] float heightPrey;
    [SerializeField] float heightHunter;
    [SerializeField] float verticalOffset;

    private float HeightFromPlayer { get { return PlayerState.IsHunter ? heightHunter : heightPrey; } }

    Controller3D player;

    Vector3 TargetPosition { get { return player.Pos + GetOffsetVector() + Vector3.forward * verticalOffset; } }

    private void Start()
    {
        player = Controller3D.Instance;
        transform.position = KeepTransformY(player.Pos);
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, speed * Time.deltaTime);
    }

    Vector3 KeepTransformY(Vector3 source)
    {
        return UMath.NewVector(source.x, transform.position.y, source.z);
    }

    Vector3 GetOffsetVector()
    {
        var app = UMath.AxisProduct(UMath.GetXZ(MouseRaycaster.Hit.point - player.Pos).normalized, UMath.NewVector(maxDistanceX, 0f, maxDistanceZ));
        app.y = HeightFromPlayer;
        return app;
    }
}
