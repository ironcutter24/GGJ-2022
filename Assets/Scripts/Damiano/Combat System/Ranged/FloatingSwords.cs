using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Utility.Patterns;

public class FloatingSwords : Singleton<FloatingSwords>
{
    [SerializeField] List<SwordOrigin> swordOrigins = new List<SwordOrigin>();
    int currentOrigin = 0;

    [SerializeField] float shootHeight = 1f;
    [SerializeField] float recoilDuration = .1f;
    public static float RecoilDuration { get { return _instance.recoilDuration; } }
    [SerializeField] float reloadDuration = 1.2f;

    private Vector3 targetPosition;
    public static Vector3 TargetPosition { get { return _instance.targetPosition; } }

    public static System.Action<float> OnReload;

    private void Start()
    {
        recoilTimer = recoilDuration;
        OnReload(0f);
    }

    [SerializeField] float minAimDistance = 1f;
    float recoilTimer;
    float reloadTimer;
    private void Update()
    {
        if (!MouseRaycaster.Error)
        {
            targetPosition = ClampMinRadius(MouseRaycaster.Hit.point + Vector3.up * shootHeight);

            if (Input.GetMouseButton(1) && recoilTimer <= 0f && reloadTimer <= 0f)
                ShootAt(targetPosition);
        }

        ApplyLookDirection();
        recoilTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;
    }

    Vector3 ClampMinRadius(Vector3 position)
    {
        if (UMath.DistanceXZ(transform.position, position) < minAimDistance)
        {
            return UMath.GetXZ(transform.position) + UMath.GetDirectionXZ(transform.position, position) * minAimDistance;
        }
        return position;
    }

    [SerializeField] bool doesRotateX = false;
    void ApplyLookDirection()
    {
        if (targetPosition == null) return;

        Vector3 samePlaneTarget = Vector3.right * targetPosition.x + Vector3.up * transform.position.y + Vector3.forward * targetPosition.z;

        transform.LookAt(doesRotateX ? targetPosition : samePlaneTarget);
    }
    /*
    void ShootAt(Vector3 targetPosition)
    {
        currentOrigin++;

        if (currentOrigin >= swordOrigins.Count)
            currentOrigin -= swordOrigins.Count;

        //currentOrigin = Random.Range(0, swordOrigins.Count);
        swordOrigins[currentOrigin].ShootAt(targetPosition);
        swordOrigins[currentOrigin].GenerateSwordAfter(recoilTime);

        recoilTimer = recoilTime;
    }
    */
    void ShootAt(Vector3 targetPosition)
    {
        swordOrigins[currentOrigin].ShootAt(targetPosition);
        currentOrigin++;

        if (currentOrigin >= swordOrigins.Count)
        {
            currentOrigin = 0;
            reloadTimer = reloadDuration;
            if (OnReload != null)
                OnReload(reloadDuration - recoilDuration);
        }
        else
            recoilTimer = recoilDuration;
    }
    
    bool isShooting = false;
    IEnumerator _ShootingPattern()
    {
        isShooting = true;

        currentOrigin = 0;
        while (currentOrigin <= swordOrigins.Count)
        {
            swordOrigins[currentOrigin].ShootAt(targetPosition);
            currentOrigin++;

            yield return new WaitForSeconds(recoilDuration);
        }
        yield return new WaitForSeconds(reloadDuration - recoilDuration);

        if (OnReload != null)
            OnReload(0f);

        yield return new WaitForSeconds(recoilDuration);

        isShooting = false;
    }
}
