using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class fir switches that can call actions
/// </summary>
public abstract class SwitchObject : MonoBehaviour
{
    //if the switch is activated
    protected bool activated = false;

    //the action for the switch to perform
    [SerializeField]
    protected SwitchActivation action;

    /// <summary>
    /// Sets the switch state and calls the switch action
    /// </summary>
    /// <param name="_active">switch state</param>
    protected virtual void SetSwitch(bool _active)
    {
        activated = _active;
        action.SwitchFunction(activated);
    }
}
