using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityDam;
using UtilityDam.Patterns;

public class FloatingSwords : Singleton<FloatingSwords>
{
    [SerializeField] LayerMask layerMask;

    [SerializeField] List<SwordOrigin> swordOrigins = new List<SwordOrigin>();
    int currentOrigin = 0;

    [SerializeField] float shootHeight = 1f;
    [SerializeField] float recoilTime = .1f;
    [SerializeField] float reloadTime = 1.2f;

    private Vector3 targetPosition;
    public static Vector3 TargetPosition { get { return _instance.targetPosition; } }

    public System.Action<float> OnReload;

    [SerializeField] float minAimDistance = 1f;
    float recoilTimer;
    float reloadTimer;
    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            targetPosition = ClampMinRadius(hit.point + Vector3.up * shootHeight);

            if (Input.GetMouseButton(0) && recoilTimer <= 0f && reloadTimer <= 0f)
            {
                ShootAt(targetPosition);
                recoilTimer = recoilTime;
            }
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

    void ApplyLookDirection()
    {
        if (targetPosition == null) return;

        Vector3 samePlaneTarget = Vector3.right * targetPosition.x + Vector3.up * transform.position.y + Vector3.forward * targetPosition.z;
        transform.LookAt(samePlaneTarget);
    }

    void ShootAt(Vector3 targetPosition)
    {
        swordOrigins[currentOrigin].ShootAt(targetPosition);
        currentOrigin++;

        if (currentOrigin >= swordOrigins.Count)
        {
            currentOrigin = 0;
            reloadTimer = reloadTime;
            if (OnReload != null)
                OnReload(reloadTime - recoilTime);
        }
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

            yield return new WaitForSeconds(recoilTime);
        }
        yield return new WaitForSeconds(reloadTime - recoilTime);

        if (OnReload != null)
            OnReload(0f);

        yield return new WaitForSeconds(recoilTime);

        isShooting = false;
    }

    /*
    void ShootAt(Vector3 targetPosition)
    {
        currentOrigin += 2;

        if (currentOrigin >= swordOrigins.Count)
            currentOrigin -= swordOrigins.Count;

        //currentOrigin = Random.Range(0, swordOrigins.Count);
        swordOrigins[currentOrigin].ShootAt(targetPosition);
    }
    */
}
