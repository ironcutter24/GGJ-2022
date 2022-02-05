using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrail : MonoBehaviour
{
    [SerializeField] float step = .4f;
    [SerializeField] bool isEnabled = true;

    Vector3 startPosition, startDirection;
    Quaternion startRotation;
    int particlesSpawned;
    public void Init(Vector3 startPosition, Quaternion startRotation, Vector3 startDirection)
    {
        this.startPosition = startPosition;
        this.startRotation = startRotation;
        this.startDirection = startDirection;
        particlesSpawned = 0;
    }

    public void Process(float distance)
    {
        if (!isEnabled)
        {
            Debug.LogWarning("DashTrail is disabled");
            return;
        }

        int particlesNeeded = (int)(distance / step);
        Vector3 spawnPosition;

        for (int i = particlesSpawned; i < particlesNeeded; i++)
        {
            spawnPosition = startPosition + startDirection * step * i;
            PlayerGhostPooler.Spawn("DashParticle", spawnPosition, startRotation, null);
            particlesSpawned++;
        }
    }
}
