using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    [SerializeField] TrailRenderer trail;

    public void EnableTrail()
    {
        trail.enabled = true;
    }

    public void DisableTrail()
    {
        trail.enabled = false;
    }
}
