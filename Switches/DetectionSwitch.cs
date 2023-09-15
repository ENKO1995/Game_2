using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch that activates when the player enters a trigger
/// </summary>
public class DetectionSwitch : SwitchObject
{
    //Set if the switch turns itself off when the player leaves
    [SerializeField]
    private bool permanent = true;

    //Set if the switch turns itself off after a set time
    [SerializeField]
    private float timed = 0;

    private float currentTimer = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!activated)
            {
                SetSwitch(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (activated && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!permanent)
            {
                SetSwitch(false);
            }
            else if (timed > 0)
            {
                currentTimer = timed;
            }
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
