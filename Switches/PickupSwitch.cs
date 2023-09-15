using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch that activated when the player picks up the object.
/// Destroys itself.
/// </summary>
public class PickupSwitch : SwitchObject
{
    [SerializeField]
    private bool showInUI = true;

    private bool pickedUp = false;

    public static Action OnSwitchPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (!pickedUp && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            pickedUp = true;
            Destroy(gameObject);
            SetSwitch(true);

            if (showInUI && OnSwitchPickup != null)
            {
                OnSwitchPickup();
            }
        }
    }

    /// <summary>
    /// returns if this crystal should be shown in the UI
    /// </summary>
    /// <returns>show in UI</returns>
    public bool ShowInUI()
    {
        return showInUI;
    }
}
