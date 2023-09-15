using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that needs multiple other actions to activate
/// </summary>
public class ActionCombiner : SwitchActivation
{
    [SerializeField]
    private int activationsNeeded = 2;

    [SerializeField]
    private SwitchActivation action;

    private int currentActivations = 0;

    public override void SwitchFunction(bool _activated)
    {
        int previous = currentActivations;
        currentActivations += _activated ? 1 : -1;

        if (previous < activationsNeeded && currentActivations >= activationsNeeded)
        {
            action.SwitchFunction(true);
        }

        if (previous >= activationsNeeded && currentActivations < activationsNeeded)
        {
            action.SwitchFunction(false);
        }
    }

    
}
