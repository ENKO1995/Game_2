using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a hitbox that can deal damage.
/// </summary>
public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 0;
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private bool stun = true;
    [SerializeField]
    private float knockback;

    /// <summary>
    /// Get hitbox damage.
    /// </summary>
    /// <returns>damage</returns>
    public int GetDamage()
    {
        return damage;
    }

    /// <summary>
    /// Set hitbox damage.
    /// </summary>
    /// <param name="_damage">damage</param>
    public void SetDamage(int _damage)
    {
        damage = _damage;
    }

    /// <summary>
    /// Get if hitbox stuns.
    /// </summary>
    /// <returns>hitbox stuns</returns>
    public bool GetStun()
    {
        return stun;
    }

    /// <summary>
    /// Get hitbox knockback.
    /// </summary>
    /// <returns>knockback</returns>
    public float GetKnockback()
    {
        return knockback;
    }

    /// <summary>
    /// Set hitbox lifetime.
    /// </summary>
    /// <param name="_time">lifetime</param>
    public void SetLifetime(float _time)
    {
        lifetime = _time;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
