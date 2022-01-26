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

    [Header("Path")]
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    private int currentWaypoint;

    float distanceFromPlayer = Mathf.Infinity;

    Controller3D player;

    private void Start()
    {
        player = Controller3D.Instance;
    }

    #region Patrol state

    public void PatrolInit()
    {
        SetDestination(PeekNextWaypoint());
    }

    public void PatrolUpdate()
    {
        if (HasReachedDestination())
            SetDestination(PeekNextWaypoint());
    }

    Vector3 PeekNextWaypoint()
    {
        Vector3 temp = waypoints[currentWaypoint].position;

        currentWaypoint++;

        if (currentWaypoint >= waypoints.Count)
            currentWaypoint -= waypoints.Count;

        return temp;
    }

    #endregion

    #region Chase state

    public void ChaseUpdate()
    {

    }

    #endregion

    #region Attack state

    public bool IsPlayerInAttackRange()
    {
        return distanceFromPlayer < attackDistance;
    }

    public void Attack()
    {

    }

    #endregion

    #region Trigger events

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            UpdateDistanceFromPlayer();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            distanceFromPlayer = Mathf.Infinity;
        }
    }

    #endregion

    public void SetDestination(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    public bool HasReachedDestination()
    {
        return agent.hasPath;
    }

    private void UpdateDistanceFromPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.Pos);
    }
}
