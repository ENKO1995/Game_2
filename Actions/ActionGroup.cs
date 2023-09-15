using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that can activate multiple other actions
/// </summary>
public class ActionGroup : SwitchActivation
{
    [SerializeField]
    private SwitchActivation[] switchActivations = new SwitchActivation[0];

    public override void SwitchFunction(bool _activated)
    {
        foreach(SwitchActivation activate in switchActivations)
        {
            activate.SwitchFunction(_activated);
        }
    }

    
}
