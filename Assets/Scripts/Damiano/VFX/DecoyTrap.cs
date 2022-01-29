using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTrap : PlayerGhost
{
    [Header("DecoyTrap")]
    [SerializeField] int trapDamage;

    [Header("Particles")]
    [SerializeField] GameObject groundParticles;
    [SerializeField] GameObject airParticles;

    private TrapPlacer placer;

    public void SetPlacer(TrapPlacer placer)
    {
        this.placer = placer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Entered decoy: " + other.gameObject.name);

            AttackMessage attackMessage = new AttackMessage(trapDamage, gameObject, AttackMessage.Type.Melee);
            other.GetComponent<Enemy>().ApplyDamage(attackMessage);

            Instantiate(airParticles, transform.position, Quaternion.identity);
            Instantiate(groundParticles, transform.position, Quaternion.identity);

            Dissolve();
            placer.RemoveFromPlacer(this);
        }
    }

    /*
    void ITargetable.ApplyDamage(AttackMessage attack)
    {
        if (attack.source.CompareTag("Enemy"))
        {
            if (attack.type == AttackMessage.Type.Melee)
            {
                AttackMessage attackMessage = new AttackMessage(trapDamage, gameObject, AttackMessage.Type.Melee);
                attack.source.GetComponent<ITargetable>().ApplyDamage(attackMessage);
                particles.SetActive(true);
                Dissolve();
                placer.RemoveFromPlacer(this);
            }
        }
    }
    */
}
