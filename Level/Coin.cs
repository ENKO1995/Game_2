using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for coin objects
/// </summary>
public class Coin : MonoBehaviour
{
    //Coin Value
    [SerializeField]
    private int coinWorth = 1;

    //Coin pickup delegate
    public static Action<int> OnCoinPickup;

    private bool pickedUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!pickedUp && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            pickedUp = true;
            Destroy(gameObject);
            if (OnCoinPickup != null)
            {
                OnCoinPickup(coinWorth);
            }
        }
    }
}
