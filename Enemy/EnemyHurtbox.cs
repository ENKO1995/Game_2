using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox") && other.TryGetComponent(out Hitbox hit))
        {
            hit.GetDamage();
        }
    }

}
