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
    }

    protected bool isMoving = false;
    private void FixedUpdate()
    {
        if (!isMoving) return;

        Vector3 newPosition = this.rb.position + this.transform.forward * speed * Time.deltaTime;
        Vector3 move = newPosition - this.rb.position;

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
    }

    public void StopMoving() { isMoving = false; }
}
