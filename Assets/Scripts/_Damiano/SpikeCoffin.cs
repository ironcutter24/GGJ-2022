using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCoffin : MonoBehaviour
{
    private static List<SpikeCoffin> _inScene = new List<SpikeCoffin>();
    public static List<SpikeCoffin> InScene { get { return _inScene; } }

    private void Awake()
    {
        _inScene.Add(this);
    }

    private void OnDestroy()
    {
        _inScene.Clear();
    }
}
