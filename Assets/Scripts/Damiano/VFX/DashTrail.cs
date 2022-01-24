using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrail : MonoBehaviour
{
    [SerializeField] GameObject particlePrefab;
    [SerializeField] float step = .4f;

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
        int particlesNeeded = (int)(distance / step);

        for (int i = particlesSpawned; i < particlesNeeded; i++)
        {
            Instantiate(particlePrefab, startPosition + startDirection * step * i, startRotation, null);
            particlesSpawned++;
        }
    }
}
