using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that moves an object
/// </summary>
public class ActionMoveObject : SwitchActivation
{
    //Set how the object should move
    [SerializeField]
    private Vector3 movement;

    //Set how the object should rotate
    [SerializeField]
    private Vector3 rotate;
    private Vector3 setRotation;

    //Set of how fast the object should move
    [SerializeField]
    private float movementTime = 1;

    private float movementTimer = 0;

    private bool activated = false;

    public override void SwitchFunction(bool _activated)
    {
        activated = _activated;
    }

    // Move the Object
    void Update()
    {
        float change = movementTimer;
        movementTimer = Mathf.Clamp01(movementTimer + (Time.deltaTime / movementTime) * (activated ? 1 : -1));

        transform.localPosition += movement * (movementTimer - change);
        if (rotate.magnitude > 0 && change != 0)
        {
            setRotation += rotate * (movementTimer - change);
            transform.localEulerAngles = setRotation;
        }
    }
}
