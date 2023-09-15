using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a moving platform the player can stand on
/// </summary>
public class ParentPlatform : MonoBehaviour
{
    /// <summary>
    /// Player lands on the platform, add to children
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(0).normal.y <= -0.9f && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.transform.SetParent(gameObject.transform, true);
        }
    }

    /// <summary>
    /// Player leaves platform, remove from children
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }

}
