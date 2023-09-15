using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle collision with enemy hitboxes for the player
/// </summary>
public class PlayerHurtbox : MonoBehaviour
{
    //player reference
    [SerializeField]
    private PlayerController player;

    /// <summary>
    /// Collision with hitbox, calls player take damage.
    /// </summary>
    /// <param name="other">collider</param>
    private void OnTriggerStay(Collider other)
    {
        if (!player.IsInvincible() && other.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox") && other.TryGetComponent(out Hitbox hit))
        {
            player.TakeDamage(hit.GetDamage(), hit.GetStun());
        }
    }

}
