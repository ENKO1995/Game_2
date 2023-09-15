using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch that activates if the player has collected a certain key
/// </summary>
public class SwitchCheckKey : SwitchObject
{
    [SerializeField]
    private int checkKeyId;

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.gameController.PlayerHasKey(checkKeyId))
        {
            SetSwitch(true);
        }
    }

}
