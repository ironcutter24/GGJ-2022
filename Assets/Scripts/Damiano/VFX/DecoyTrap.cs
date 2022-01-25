using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTrap : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        anim.SetFloat("Horizontal", 0f);
        anim.SetFloat("Vertical", 0f);
        anim.SetFloat("MoveSpeed", 0f);
    }

    public void Activate()
    {

    }
}
