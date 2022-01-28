using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemy : MonoBehaviour, ITargetable
{
    [SerializeField] int health = 10;

    void ITargetable.ApplyDamage(AttackMessage attack)
    {
        health -= attack.damage;
        Debug.Log("SampleEnemy hit! Health: " + health);
    }
}
