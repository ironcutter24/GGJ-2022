using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] SphereCollider sphereCollider;

    [Header("Engaging")]
    [SerializeField] float disengageDistance;
    [SerializeField] float engageDistancePassive;
    [SerializeField] float engageDistanceVision;
    [SerializeField] float attackDistance;
    
    [SerializeField]
    [Range(0f, 180f)]
    float fieldOfView = 60f;

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
        sphereCollider.radius = disengageDistance;
    }

#if UNITY_EDITOR
    private void Update()
    {
        sphereCollider.radius = disengageDistance;
    }
#endif

    #region Patrol state

    public Vector3 PeekNextWaypoint()
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

	public abstract void Attack();

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
        return agent.remainingDistance < .5f;
    }

    private void UpdateDistanceFromPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.Pos);
    }

    public bool CanSeePlayer()
    {
        if(distanceFromPlayer < disengageDistance)
        {
            if(distanceFromPlayer < engageDistanceVision)
            {
                if(distanceFromPlayer < engageDistancePassive)
                {
                    return true;
                }

                if (IsInFieldOfView(Controller3D.Instance.Pos))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool IsInFieldOfView(Vector3 position)
    {
        return Vector3.Angle(UMath.GetXZ(position) - UMath.GetXZ(transform.position), UMath.GetXZ(transform.forward)) < fieldOfView * .5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, .35f);
        Gizmos.DrawSphere(transform.position, attackDistance);

        Gizmos.color = new Color(1f, 1f, 0f, .2f);
        Gizmos.DrawSphere(transform.position, engageDistancePassive);

        Gizmos.color = new Color(1f, 1f, 1f, .15f);
        Gizmos.DrawSphere(transform.position, engageDistanceVision);

        Gizmos.color = new Color(1f, 1f, 1f, .1f);
        Gizmos.DrawSphere(transform.position, disengageDistance);
    }
}
