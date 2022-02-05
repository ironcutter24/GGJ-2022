using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoolSceneController : MonoBehaviour
{
    [SerializeField] ExitDoor exitDoor;

    private void Start()
    {
        exitDoor.StartTarotAnimation();
    }
}
