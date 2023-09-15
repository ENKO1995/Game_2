using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a health pickup that heals the player
/// </summary>
public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    private int restoreHealth = 25;

    private bool pickedUp = false;

    /// <summary>
    /// Heal player and destroy.
    /// </summary>
    /// <param name="other">collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!pickedUp && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.OnPlayerHeal(restoreHealth);
            pickedUp = true;
            Destroy(gameObject);
        }
    }
}
