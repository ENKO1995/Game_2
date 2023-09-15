using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that makes an object spin
/// </summary>
public class Spinning : MonoBehaviour
{
    [SerializeField]
    private float turningSpeed = 90;

    [SerializeField]
    private Vector3 axis = Vector3.up;

    /// <summary>
    /// Update, spins object around an axis
    /// </summary>
    void Update()
    {
        transform.Rotate(axis, turningSpeed * Time.deltaTime);
    }
}
