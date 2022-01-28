using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTrap : PlayerGhost
{
    [Header("DecoyTrap")]
    [SerializeField] GameObject particles;
    [SerializeField] float trapDamage;

    private void Awake()
    {
        particles.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered decoy: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().ApplyDamage(trapDamage);
            particles.SetActive(true);
            Dissolve();
        }
    }
}
