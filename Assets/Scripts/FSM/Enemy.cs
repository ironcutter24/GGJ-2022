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
    [SerializeField] GameObject graphics;
    [SerializeField] Collider hitCollider;
    [SerializeField] MaterialMimic materialMimic;

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
    [SerializeField] int maxHealth = 3;
    private int _health;

    [Header("Spike Enemy")]
    [SerializeField] public GameObject spikeCoffin;
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    private int currentWaypoint;
    public bool HasWaypoints { get { return waypoints.Count > 0; } }

    float distanceFromPlayer = Mathf.Infinity;
    public float DistanceFromPlayer { get { return distanceFromPlayer; } }

    Controller3D player;

    private void Start()
    {
        player = Controller3D.Instance;
        _health = maxHealth;

        PlayerState.AddActiveEnemy();
    }

    float startAlpha = 1f;
    public void ApplyDamage(AttackMessage attack)
    {
        _health = Mathf.Clamp(_health - attack.damage, 0, maxHealth);

        PlayerState.RecordSuccessfulAttack(attack);

        if (_health <= 0f)
        {
            if(CanSeePlayer())
                PlayerState.EngagedEnemies--;

            PlayerState.RemoveNearEnemy(this);

            StartCoroutine(_Death(.5f));
        }
        
        IEnumerator _Death(float duration)
        {
            float speed = 1 / duration;
            float interpolation = 0f;
            while (interpolation < 1f)
            {
                SetAlpha(Mathf.Lerp(startAlpha, 0f, interpolation));
                interpolation += speed * Time.deltaTime;
                yield return null;
            }
            interpolation = 1f;
            SetAlpha(startAlpha);

            PlayerState.RemoveActiveEnemy();
            Destroy(this.gameObject);


            void SetAlpha(float alpha)
            {
                materialMimic.Mat.SetFloat("_CutoffHeight", alpha * 5);
            }
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

        if (collider.gameObject.CompareTag("PlayerDecoy"))
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

    #region Movement

    public abstract void SetDestination(Vector3 targetPosition);

    public bool HasReachedDestination()
    {
        return agent.remainingDistance < agent.stoppingDistance;
    }

    public bool IsMovementPaused { get { return agent.isStopped; } }

    public void PauseMovement()
    {
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
    }

    #endregion

    private void UpdateDistanceFromPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.Pos);
    }

    public void SetCollisionsAndGraphics(bool state)
    {
        graphics.SetActive(state);
        hitCollider.enabled = state;
    }

    #region Sight

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

    #endregion

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
