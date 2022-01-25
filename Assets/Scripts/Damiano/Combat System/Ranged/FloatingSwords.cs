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
    public static Vector3 TargetPosition { get { return _instance.targetPosition != null ? _instance.targetPosition : Vector3.zero; } }

    public static System.Action OnReload;
    public static System.Action OnDischarge;

    Timer recoilTimer = new Timer();
    Timer reloadTimer = new Timer();

    protected override void Awake()
    {
        base.Awake();

        Controller3D.OnSwitchToPrey += ToPrey;
        Controller3D.OnSwitchToHunter += ToHunter;
    }

    private void OnDestroy()
    {
        Controller3D.OnSwitchToPrey -= ToPrey;
        Controller3D.OnSwitchToHunter -= ToHunter;
    }

    private void Start()
    {
        recoilTimer.Set(recoilDuration);
        Util.TryAction(OnReload);
    }

    [SerializeField] float minAimDistance = 1f;
    private void Update()
    {
        if (!MouseRaycaster.Error)
        {
            targetPosition = ClampMinRadius(MouseRaycaster.Hit.point + Vector3.up * shootHeight);

            if (Input.GetMouseButton(1) && Controller3D.IsHunter && recoilTimer.IsExpired && reloadTimer.IsExpired)
                ShootAt(targetPosition);
        }
        ApplyLookDirection();
    }

    void ToPrey()
    {
        OnDischarge();
    }

    void ToHunter()
    {
        Util.TryAction(OnReload);
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

        Vector3 samePlaneTarget = UMath.NewVector(targetPosition.x, transform.position.y, targetPosition.z);

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
            reloadTimer.Set(reloadDuration);

            //TryAction(OnReload, reloadDuration - recoilDuration);
            StartCoroutine(_ScheduleReload(reloadDuration - recoilDuration));
        }
        else
            recoilTimer.Set(recoilDuration);
    }

    IEnumerator _ScheduleReload(float timeOffset)
    {
        yield return new WaitForSeconds(timeOffset);
        Util.TryAction(OnReload);
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

        Util.TryAction(OnReload);

        yield return new WaitForSeconds(recoilDuration);

        isShooting = false;
    }
}
