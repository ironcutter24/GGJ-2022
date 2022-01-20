using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityDam.Patterns;

public class FloatingSwords : Singleton<FloatingSwords>
{
    [SerializeField] LayerMask layerMask;

    [SerializeField] List<SwordOrigin> swordOrigins = new List<SwordOrigin>();
    int currentOrigin = 0;

    [SerializeField] float shootHeight = 1f;
    [SerializeField] float recoilTime = .1f;

    private Vector3 targetPosition;
    public static Vector3 TargetPosition { get { return _instance.targetPosition; } }

    float recoilTimer;
    private void Update()
    {
        recoilTimer -= Time.deltaTime;
        if (recoilTimer > 0f) return;


        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            targetPosition = hit.point + Vector3.up * shootHeight;

            Vector3 samePlaneTarget = Vector3.right * targetPosition.x + Vector3.up * transform.position.y + Vector3.forward * targetPosition.z;
            transform.LookAt(samePlaneTarget);

            if (Input.GetMouseButton(0))
            {
                ShootAt(targetPosition);
            }

            recoilTimer = recoilTime;
        }
	}

    void ShootAt(Vector3 targetPosition)
    {
        currentOrigin += 2;

        if(currentOrigin >= swordOrigins.Count)
            currentOrigin -= swordOrigins.Count;

        //currentOrigin = Random.Range(0, swordOrigins.Count);
        swordOrigins[currentOrigin].ShootAt(targetPosition);
    }
}
