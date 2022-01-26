using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

public abstract class Enemy : MonoBehaviour, ITargetable
{
    [Header("Components")]
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] SphereCollider nearFieldCollider;
    [SerializeField] GameObject graphics;

    [Header("Engaging")]
    [SerializeField] float disengageDistance;
    [SerializeField] float engageDistancePassive;
    [SerializeField] float engageDistanceVision;
    [SerializeField] float attackDistance;
    
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

    void ITargetable.ApplyDamage(float amount)
    {
        _health -= amount;

        if(_health <= 0f)
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

    public Vector3 PeekNextWaypoint()
    {
        Vector3 temp = waypoints[currentWaypoint].position;

        currentWaypoint++;

        if (currentWaypoint >= waypoints.Count)
            currentWaypoint -= waypoints.Count;

        return temp;
    }

    #endregion

    #region Attack state

    public bool IsPlayerInAttackRange()
    {
        return distanceFromPlayer < attackDistance;
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }

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
                if(distanceFromPlayer < engageDistancePassive || IsInFieldOfView(Controller3D.Instance.Pos))
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
