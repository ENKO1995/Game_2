using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a barrier that causes the player to teleport back to the last respawn point.
/// </summary>
public class VoidBarrier : MonoBehaviour
{
    //player reference
    private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    /// <summary>
    /// Collision with player causes a void out.
    /// </summary>
    /// <param name="other">collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.TriggerVoidOut();
        }
    }
}
