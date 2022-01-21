using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 1f;
    [SerializeField] float damage = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        OnEnable();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    Vector3 newPosition;
    Vector3 move;
    protected bool isMoving = false;
    private void FixedUpdate()
    {
        if (!isMoving) return;

        newPosition = this.rb.position + this.transform.forward * speed * Time.deltaTime;
        move = newPosition - this.rb.position;

        RaycastHit hit;
        if (Physics.Raycast(this.rb.position, move, out hit, move.magnitude))
        {
            var target = hit.transform.gameObject.GetComponent<ITargetable>();

            if(target != null)
                target.ApplyDamage(damage);

            StopMoving();
            this.gameObject.SetActive(false);
        }
        else
        {
            rb.MovePosition(newPosition);
        }
    }

    public void StartMoving()
    {
        transform.parent = null;
        isMoving = true;

        StopAllCoroutines();
        StartCoroutine(_DisablingCountdown());
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    IEnumerator _DisablingCountdown()
    {
        while (gameObject.activeInHierarchy && isMoving)
        {
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
    }
}
