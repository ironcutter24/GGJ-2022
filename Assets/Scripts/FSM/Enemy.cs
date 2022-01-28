using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Utility;

public abstract class Enemy : MonoBehaviour, ITargetable
{
    [Header("Components")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] SphereCollider nearFieldCollider;
    [SerializeField] protected GameObject graphics;

    [Header("Engaging")]
    [SerializeField] float disengageDistance;
    [SerializeField] float engageDistancePassive;
    [SerializeField] float engageDistanceVision;
    [SerializeField] float attackDistance;
    [SerializeField] LayerMask blockView;

    public float DangerDistanceMin { get { return engageDistanceVision; } }

    public float DangerDistanceMax { get { return nearFieldCollider.radius; } }

    [SerializeField]
    [Range(0f, 180f)]
    float fieldOfView = 60f;

    [Header("Stats")]
    [SerializeField] float maxHealth = 100f;
    private float _health;
    //[SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackSpeed = 1f;

    [Header("Path")]
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    private int currentWaypoint;
    public bool HasWaypoints { get { return waypoints.Count > 0; } }

    [Header("Spike Enemy")]
    [SerializeField] public GameObject spikeLair;

    float distanceFromPlayer = Mathf.Infinity;
    public float DistanceFromPlayer { get { return distanceFromPlayer; } }

    Controller3D player;

    private void Start()
    {
        player = Controller3D.Instance;
        _health = maxHealth;
    }

    public void ApplyDamage(int amount)
    {
        _health -= amount;

        if (_health <= 0f)
        {
            StartCoroutine(_Death());
        }

        IEnumerator _Death()
        {
            // Material dissolve transition

            Destroy(this.gameObject);
            yield break;
        }
    }

    #region Patrol state

    public Vector3 GetNearestWaypoint()
    {
        float nearestWaypointDistance = Mathf.Infinity;
        int nearestWaypointIndex = 0;

        for(int i = 0; i < waypoints.Count; i++)
        {
            float distance = UMath.DistanceXZ(waypoints[i].position, transform.position);

            if(distance < nearestWaypointDistance)
            {
                nearestWaypointIndex = i;
                nearestWaypointDistance = distance;
            }
        }

        currentWaypoint = nearestWaypointIndex - 1;
        return GetNextWaypoint();
    }

    public Vector3 GetNextWaypoint()
    {
        currentWaypoint++;

        if (currentWaypoint >= waypoints.Count)
            currentWaypoint -= waypoints.Count;

        return waypoints[currentWaypoint].position;
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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerState.AddNearEnemy(this);
        }
    }

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
            PlayerState.RemoveNearEnemy(this);
        }
    }

    #endregion

    public void EnableGraphics()
    {
        graphics.SetActive(true);
    }

    public void DisableGraphics()
    {
        graphics.SetActive(false);
    }

    public abstract void SetDestination(Vector3 targetPosition);

    public bool HasReachedDestination()
    {
        return agent.remainingDistance < agent.stoppingDistance;
    }

    private void UpdateDistanceFromPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.Pos);
    }

    public bool CanSeePlayer(bool isChasing = false)
    {
        if (distanceFromPlayer < disengageDistance)
        {
            if (isChasing && IsInFieldOfView(Controller3D.Instance.Pos))
                return true;

            if (distanceFromPlayer < engageDistanceVision)
            {
                if (distanceFromPlayer < engageDistancePassive || IsInFieldOfView(Controller3D.Instance.Pos))
                {
                    PlayerState.EngagedEnemies++;
                    return true;
                }
            }
        }
        PlayerState.EngagedEnemies--;
        return false;
    }

    bool IsInFieldOfView(Vector3 position)
    {
        if (Vector3.Angle(UMath.GetXZ(position) - UMath.GetXZ(transform.position), UMath.GetXZ(transform.forward)) < fieldOfView * .5f)
        {
            if (!Physics.Raycast(transform.position, Controller3D.Instance.Pos - transform.position, Mathf.Infinity, blockView))
            {
                Debug.DrawRay(transform.position, Controller3D.Instance.Pos - transform.position, Color.green);
                return true;
            }
        }
        Debug.DrawRay(transform.position, Controller3D.Instance.Pos - transform.position, Color.red);
        return false;
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
