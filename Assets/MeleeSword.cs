using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSword : MonoBehaviour
{
    [SerializeField] Animator anim;

    enum State { ForwardSlash, BackwardSlash, Lunge, Idle, Stunned }
    State state = State.Idle;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check combo timing
            // Trigger attack animation
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var temp = collision.gameObject.GetComponent<ITargetable>();
        if (temp == null) return;


    }

    public void OpenComboTimeFrame()
    {

    }

    public void CloseComboTimeFrame()
    {

    }
}
