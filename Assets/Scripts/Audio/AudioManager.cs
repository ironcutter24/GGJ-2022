using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class AudioManager : Singleton<AudioManager>
{
    public static void PlayerStateTransition() { PlayFMODEvent("event:/PlayerStateTransition"); } //
    public static void PlayerTrapDetonation() { PlayFMODEvent("event:/PlayerTrapDetonation"); } //
    public static void PlayerKnifeThrow() { PlayFMODEvent("event:/PlayerKnifeThrow"); }
    public static void PlayerKnifeHit() { PlayFMODEvent("event:/PlayerKnifeHit"); } //
    public static void PlayerDash() { PlayFMODEvent("event:/PlayerDash"); } //
    public static void PlayerHit() { PlayFMODEvent("event:/PlayerHit"); } //


    public static void SpikeMeleeAttack() { PlayFMODEvent("event:/SpikeMeleeAttack"); } //
    public static void SpikeDeath() { PlayFMODEvent("event:/SpikeDeath"); } //


    public static void PlayFMODEvent(string eventPath)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventPath);
    }
}
