using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTrap : PlayerGhost
{
    [Header("DecoyTrap")]
    [SerializeField] GameObject particles;

    public void Activate()
    {
        particles.SetActive(true);
    }
}
