using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [SerializeField] Enemy actor;
    [SerializeField] Transform attackBounds;
    [SerializeField] int attackDamage;

    private void Start()
    {
        attackBounds.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void AnimationHasEnded()
    {
        actor.ResumeMovement();
    }

    public void AttackAnimationEvent()
    {
        Collider[] colliders = GetCollidersInAttackBounds();

        foreach(Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Player") || c.gameObject.CompareTag("PlayerDecoy"))
            {
                AttackMessage attackMessage = new AttackMessage(attackDamage, gameObject, AttackMessage.Type.Melee);
                c.GetComponent<ITargetable>().ApplyDamage(attackMessage);
            }
        }
    }

    private Collider[] GetCollidersInAttackBounds()
    {
        return Physics.OverlapBox(transform.position + transform.forward * attackBounds.localPosition.z, attackBounds.localScale, transform.rotation);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.matrix = attackBounds.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, attackBounds.localScale);
    }
    */
}
