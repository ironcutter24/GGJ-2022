using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTrap : PlayerGhost
{
    [Header("DecoyTrap")]
    [SerializeField] GameObject particles;
    [SerializeField] float trapDamage;

    protected override void Start()
    {
        base.Start();
        particles.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered decoy: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().ApplyDamage(trapDamage);
            particles.SetActive(true);
        }
    }
}
