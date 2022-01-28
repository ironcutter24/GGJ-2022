using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTrap : PlayerGhost
{
    [Header("DecoyTrap")]
    [SerializeField] GameObject particles;
    [SerializeField] int trapDamage;

    private TrapPlacer placer;

    public void SetPlacer(TrapPlacer placer)
    {
        this.placer = placer;
    }

    protected override void Start()
    {
        base.Start();
        particles.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Entered decoy: " + other.gameObject.name);

            AttackMessage attackMessage = new AttackMessage(trapDamage, gameObject, AttackMessage.Type.Melee);
            other.GetComponent<Enemy>().ApplyDamage(attackMessage);
            particles.SetActive(true);
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
