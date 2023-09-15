using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that calls for a scene change
/// </summary>
public class ActionSceneSwitch : SwitchActivation
{
    [SerializeField]
    private string sceneName;

    public override void SwitchFunction(bool _activated)
    {
        GameController.gameController.SceneTransition(sceneName, 0);
    }

}
