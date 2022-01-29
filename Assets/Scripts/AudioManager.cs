using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> playerRangedAttack = new List<AudioClip>();
    [SerializeField] List<AudioClip> playerHit = new List<AudioClip>();

    [SerializeField] List<AudioClip> spikeEnemyHit = new List<AudioClip>();
    [SerializeField] List<AudioClip> spikeEnemyMeleeAttack = new List<AudioClip>();
}
