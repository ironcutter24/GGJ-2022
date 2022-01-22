using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }
}
