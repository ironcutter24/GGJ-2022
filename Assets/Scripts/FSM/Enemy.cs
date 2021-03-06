using System.Collections;
using System.Collections.Generic;
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
    [Range(0f, 360f)]
    float fieldOfView = 60f;

    [Header("Speed")]
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float walkAngularSpeed = 90f;

    [SerializeField] float runSpeed = 2f;
    [SerializeField] float runAngularSpeed = 120f;

    [Header("Stats")]
    [SerializeField] int maxHealth = 3;
    private int _health;

    [SerializeField] float stunDuration = 1f;

    [Header("Spike Enemy")]
    //[SerializeField] public SpikeCoffin spikeCoffin;
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

    void OnDestroy()
    {
        SetIsNearPlayer(false);
    }


    #region Movement

    public abstract void SetDestination(Vector3 targetPosition);

    public bool HasReachedDestination()
    {
        if(agent.pathPending)
            return false;

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

    public Vector3 GetTeleportDestination()
    {
        throw new System.NotImplementedException();
    }

    public void SetSpeedToWalk()
    {
        agent.speed = walkSpeed;
        agent.angularSpeed = walkAngularSpeed;
    }

    public void SetSpeedToRun()
    {
        agent.speed = runSpeed;
        agent.angularSpeed = runAngularSpeed;
    }

    #endregion

    #region Sight

    bool _isEngaged = false;
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
                    if (!_isEngaged)
                        PlayerState.EngagedEnemies++;

                    _isEngaged = true;
                    return true;
                }
            }
        }
        if (_isEngaged)
            PlayerState.EngagedEnemies--;

        _isEngaged = false;
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

    bool _isNearPlayer = false;
    void SetIsNearPlayer(bool state)
    {
        if (state)
        {
            if (!_isNearPlayer)
                PlayerState.AddNearEnemy(this);
        }
        else
        {
            if (_isNearPlayer)
                PlayerState.RemoveNearEnemy(this);
        }
    }

    private void UpdateDistanceFromPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.Pos);
    }

    #endregion

    #region Trigger events

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            SetIsNearPlayer(true);
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
            SetIsNearPlayer(false);
        }
    }

    #endregion



    #region Patrol state

    public Vector3 GetNearestWaypoint()
    {
        float nearestWaypointDistance = Mathf.Infinity;
        int nearestWaypointIndex = 0;

        for (int i = 0; i < waypoints.Count; i++)
        {
            float distance = UMath.DistanceXZ(waypoints[i].position, transform.position);

            if (distance < nearestWaypointDistance)
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

    private bool _isAttacking;
    public bool IsAttacking { get { return _isAttacking; } }

    public virtual void StartAttack()
    {
        _isAttacking = true;
    }

    public virtual void StopAttack()
    {
        _isAttacking = false;
    }

    #endregion

    #region Stunned state

    float startAlpha = 1f;
    public void ApplyDamage(AttackMessage attack)
    {
        _health = Mathf.Clamp(_health - attack.damage, 0, maxHealth);

        PlayerState.RecordSuccessfulAttack(attack);

        if (attack.source.GetComponent<DecoyTrap>() != null)
            _isStunned = true;

        if (_health <= 0f)
        {
            if (_isEngaged)
                PlayerState.EngagedEnemies--;

            SetIsNearPlayer(false);

            AudioManager.SpikeDeath();

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

    public void StartStunCountdown()
    {
        StartCoroutine(_StunCountdown(stunDuration));
    }

    IEnumerator _StunCountdown(float duration)
    {
        yield return new WaitForSeconds(duration);

        _isStunned = false;
    }

    bool _isStunned;

    public bool IsStunned
    {
        get { return _isStunned; }
    }

    #endregion

    #region RunAway state

    public Vector3 GetNearestCoffin()
    {
        float shortestSqrDistance = Mathf.Infinity;
        float sqrDist;
        Vector3 nearestCoffin = Vector3.zero;

        foreach (SpikeCoffin coffin in SpikeCoffin.InScene)
        {
            sqrDist = UMath.SqrDistanceXZ(coffin.transform.position, this.transform.position);

            if (sqrDist < shortestSqrDistance)
            {
                nearestCoffin = coffin.transform.position;
                shortestSqrDistance = sqrDist;
            }
        }
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = nearestCoffin;
        cube.transform.localScale = new Vector3(.2f, 20f, .2f);

        return nearestCoffin;
    }

    #endregion


    public void SetCollisionsAndGraphics(bool state)
    {
        graphics.SetActive(state);
        hitCollider.enabled = state;
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
