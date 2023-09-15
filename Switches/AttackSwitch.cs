using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch that activates when the player attacks it
/// </summary>
public class AttackSwitch : SwitchObject
{
    //Set if you can turn the switch off again by hitting it again
    [SerializeField]
    private bool permanent = true;

    //Set if the switch turns itself off again after a set time
    [SerializeField]
    private float timed = 0;

    private float currentTimer = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox") && other.TryGetComponent(out Hitbox hit))
        {
            IsHit();
        }
    }

    /// <summary>
    /// Turns the switch on/off
    /// </summary>
    private void IsHit()
    {
        if (!activated)
        {
            SetSwitch(true);
            if (timed > 0)
            {
                currentTimer = timed;
            }
        } else if (!permanent)
        {
            SetSwitch(false);
        }
    }

    /// <summary>
    /// Counts down the timer if it's active
    /// </summary>
    private void Update()
    {
        if (activated && currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0)
            {
                SetSwitch(false);
            }
        }
    }

}
