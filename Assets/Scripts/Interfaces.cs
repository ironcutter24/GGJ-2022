using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    void ApplyDamage(AttackMessage attack);
}

public class AttackMessage
{
    public int damage;
    public GameObject source;
    public Type type;

    public enum Type { Melee, Ranged };

    public AttackMessage(int damage, GameObject source, Type type)
    {
        this.damage = damage;
        this.source = source;
        this.type = type;
    }
}
