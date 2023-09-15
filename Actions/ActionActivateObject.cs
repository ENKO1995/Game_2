using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that activates or deactivates a gameobject
/// </summary>
public class ActionActivateObject : SwitchActivation
{
    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private bool startActive = false;

    public override void SwitchFunction(bool _activated)
    {
        targetObject.SetActive(_activated);
    }

    private void Awake()
    {
        targetObject.SetActive(startActive);
    }

}
