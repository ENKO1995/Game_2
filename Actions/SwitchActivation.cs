using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for a function that can be called by a switch
/// </summary>
public abstract class SwitchActivation : MonoBehaviour
{
    /// <summary>
    /// activates or deactivates the action
    /// </summary>
    /// <param name="_activated">state</param>
    public abstract void SwitchFunction(bool _activated);
}
