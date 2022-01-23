using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth;
    private float health;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackSpeed;

    public abstract void TestMethod();
}
