using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch that activates when certain objects are destroyed
/// </summary>
public class ObjectDestroySwitch : SwitchObject
{
    //Set the objects to check for destruction
    [SerializeField]
    private List<GameObject> destroyTargets = new List<GameObject>();

    private void Update()
    {
        if (!activated)
        {
            //remove destroyed objects
            foreach (GameObject o in destroyTargets)
            {
                if (o == null)
                {
                    destroyTargets.Remove(o);
                }
            }
            //all objects are gone
            if (destroyTargets.Count == 0)
            {
                SetSwitch(true);
            }
        }
    }
}
