using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;

    [Header("Engaging")]
    [SerializeField] float disengageDistance;
    [SerializeField] float engageDistanceNear;
    [SerializeField] float engageDistanceFar;
    [SerializeField] float attackDistance;
    
    [SerializeField]
    [Range(0f, 180f)]
    float fieldOfView;

    [Header("Stats")]
    [SerializeField] float maxHealth = 100f;
    private float _health;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackSpeed = 1f;

    Controller3D player;

    float distanceFromPlayer = Mathf.Infinity;

    private void Start()
    {
        player = Controller3D.Instance;
    }

    private void UpdateDistanceFromPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.Pos);
    }

    public void SetDestination(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    public bool HasReachedDestination()
    {
        return agent.hasPath;
    }

    public bool IsPlayerInAttackRange()
    {
        return distanceFromPlayer < attackDistance;
    }

    public void Attack()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UpdateDistanceFromPlayer();
        }
    }
}
