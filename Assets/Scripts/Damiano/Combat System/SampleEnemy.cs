using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemy : MonoBehaviour, ITargetable
{
    [SerializeField] float health = 100f;

    void ITargetable.ApplyDamage(int amount)
    {
        health -= amount;
        Debug.Log("SampleEnemy hit! Health: " + health);
    }
}
