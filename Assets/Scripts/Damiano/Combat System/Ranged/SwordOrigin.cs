using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOrigin : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRend;
    [SerializeField] string poolingID;

    Projectile currentSword;

    private void Awake()
    {
        FloatingSwords.OnReload += TryGenerateSword;
        FloatingSwords.OnDischarge += TryDischargeSword;
        meshRend.enabled = false;
    }

    private void OnDestroy()
    {
        FloatingSwords.OnReload -= TryGenerateSword;
        FloatingSwords.OnDischarge -= TryDischargeSword;
    }

    public void ShootAt(Vector3 targetPosition)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, targetPosition - transform.position);
        currentSword.transform.rotation = rotation;
        currentSword.StartMoving();
        currentSword = null;
    }

    void GenerateSwordAfter(float delay)
    {
        StopAllCoroutines();
        StartCoroutine(_GenerateSwordAfter(delay));

        IEnumerator _GenerateSwordAfter(float delay)
        {
            yield return new WaitForSeconds(delay);
            TryGenerateSword();
        }
    }

    void TryGenerateSword()
    {
        if (currentSword == null && Controller3D.IsHunter)
            currentSword = ProjectilePooler.Spawn(poolingID, transform.position, Quaternion.identity, this.transform);
    }

    void TryDischargeSword()
    {
        if (currentSword != null)
        {
            currentSword.Discharge();
            currentSword = null;
        }
    }
}
