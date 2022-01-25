using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] NavMeshAgent agent;

    [Header("Stats")]
    [SerializeField] float maxHealth;
    private float _health;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackSpeed;

    public abstract void TestMethod();

    public void SetDestination(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    public bool HasReachedDestination()
    {
        return agent.hasPath;
    }

    public bool IsPlayerInRange()
    {
        return true;
    }

    public void Attack()
    {

    }
}
