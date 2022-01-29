using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotData : MonoBehaviour
{
    [SerializeField] string _captionPrey;
    [SerializeField] string _captionHunter;

    public string GetCaption(bool isHunter)
    {
        if (isHunter)
            return _captionHunter;
        else
            return _captionPrey;
    }
}
