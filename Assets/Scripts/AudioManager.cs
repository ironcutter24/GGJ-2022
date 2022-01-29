using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Player")]
    [SerializeField] AudioClip playerStateTransition;
    [SerializeField] AudioClip playerTrapDetonation;
    [SerializeField] AudioClip playerKnifeThrow;
    [SerializeField] AudioClip playerKnifeHit;
    [SerializeField] AudioClip playerDash;
    [SerializeField] AudioClip playerHit;

    [Header("Spike enemy")]
    [SerializeField] AudioClip spikeMeleeAttack;
    [SerializeField] AudioClip spikeDeath;

    #region Properties

    public AudioClip PlayerStateTransition { get { return playerStateTransition; } }
    public AudioClip PlayerTrapDetonation { get { return playerTrapDetonation; } }
    public AudioClip PlayerKnifeThrow { get { return playerKnifeThrow; } }
    public AudioClip PlayerKnifeHit { get { return playerKnifeHit; } }
    public AudioClip PlayerDash { get { return playerDash; } }
    public AudioClip PlayerHit { get { return playerHit; } }


    public AudioClip SpikeMeleeAttack { get { return spikeMeleeAttack; } }
    public AudioClip SpikeDeath { get { return spikeDeath; } }

    #endregion

    public void PlayClip(AudioClip audioClip)
    {

    }
}
