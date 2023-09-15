using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that opens the final chest.
/// Can only be opened once.
/// </summary>
public class ActionOpenFinalChest : ActionOpenChest
{
    //save if already opened
    private static bool opened = false;

    protected new void Start()
    {
        if (!opened)
        {
            InstantiateLoot();
        } else
        {
            openLid.SwitchFunction(true);
        }
    }

    public override void SwitchFunction(bool _activated)
    {
        if (!opened)
        {
            if (GameController.gameController.PlayerHasAllKeys())
            {
                //player has all keys
                opened = true;
                openLid.SwitchFunction(true);
                Invoke("SpawnAllLoot", spawnDelay);
                UIController.OnGlobalMessage("<color=\"yellow\">Congratulations!\nYou've completed Friday 0120.\n\nThe game is over but you can keep playing if you want to.</color>", 10);
            } else
            {
                //player is missing keys
                UIController.OnGlobalMessage("<color=\"red\">The chest is locked.\n\nYou need two keys.</color>", 10);
            }
        }
    }
}
