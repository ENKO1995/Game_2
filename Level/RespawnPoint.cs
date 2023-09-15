using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a respawn point when the player falls off
/// </summary>
public class RespawnPoint : MonoBehaviour
{
    //Activated ring game object
    [SerializeField]
    private GameObject ringOn;

    //Deactivated ring game object
    [SerializeField]
    private GameObject ringOff;

    //the currently active respawn point
    private static RespawnPoint activeRespawn;

    //if this respawn point is active
    private bool active = false;

    /// <summary>
    /// Activates ring gameobjects.
    /// </summary>
    /// <param name="_state">ring is active</param>
    private void SetRingState(bool _state)
    {
        ringOn.SetActive(_state);
        ringOff.SetActive(!_state);
    }

    /// <summary>
    /// Deactivates last respawn point if there is one and activates this one.
    /// </summary>
    private void Activate()
    {
        active = true;

        if (activeRespawn != null)
        {
            activeRespawn.Deactivate();
        }

        activeRespawn = this;

        SetRingState(true);
    }

    /// <summary>
    /// Deactivates respawn point.
    /// </summary>
    private void Deactivate()
    {
        active = false;
        SetRingState(false);
    }

    /// <summary>
    /// Activates if the player gets close
    /// </summary>
    /// <param name="other">collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!active && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Activate();
        }
    }

    /// <summary>
    /// Static function, returns current respawn point.
    /// </summary>
    /// <returns>current respawn point</returns>
    public static RespawnPoint GetCurrentRespawn()
    {
        return activeRespawn;
    }
}
