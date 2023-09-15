using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that buys an item from a store pedestal
/// </summary>
public class ActionBuyItem : SwitchActivation
{
    [SerializeField]
    private GameObject[] itemMeshes;

    [SerializeField]
    private TextBox textBox;

    [SerializeField]
    private string textSoldOut;

    [SerializeField]
    private string textCanPurchase;

    [SerializeField]
    private string textCannotPurchase;

    [SerializeField]
    private string textPrice;

    private static float promptTime = 5;
    private float promptTimer = 0;

    private StoreItem storeItem;

    public static Action<StoreItem> OnItemPurchase;

    /// <summary>
    /// Displays an item on the depestal.
    /// </summary>
    /// <param name="_item">item</param>
    public void StockItem(StoreItem _item)
    {
        storeItem = _item;
        if (_item != null)
        {
            itemMeshes[_item.MeshID].SetActive(true);
        }
    }

    /// <summary>
    /// Checks if the player has enough coins to buy an item.
    /// </summary>
    /// <param name="_item">item</param>
    /// <returns>player can buy the item</returns>
    private bool PlayerCanBuy(StoreItem _item)
    {
        return GameController.gameController.GetPlayerCoins() >= _item.ItemPrice;
    }

    /// <summary>
    /// Generates the text to display for the item.
    /// </summary>
    /// <param name="_item">item</param>
    /// <returns>item text</returns>
    private string GenerateItemText(StoreItem _item)
    {
        if (_item == null)
        {
            return textSoldOut;
        }
        string itemText = (PlayerCanBuy(_item) ? textCanPurchase : textCannotPurchase) + "\n";
        itemText += textPrice + "<color=yellow>" + _item.ItemPrice + "</color>" + "\n\n";
        itemText += _item.ItemDescription;
        return itemText;
    }

    /// <summary>
    /// Buys the item on the pedestal.
    /// Notifies store controller.
    /// </summary>
    /// <param name="_item">item</param>
    private void BuyItem(StoreItem _item)
    {
        OnItemPurchase(_item);
        itemMeshes[_item.MeshID].SetActive(false);
        StoreController.OnShopPurchase(this, _item);
    }

    public override void SwitchFunction(bool _activated)
    {
        if (promptTimer <= 0)
        {
            promptTimer = promptTime;
            textBox.displayText(GenerateItemText(storeItem));
        } else if (storeItem != null && PlayerCanBuy(storeItem))
        {
            BuyItem(storeItem);
            promptTimer = 0;
            textBox.close();
        }
    }

    private void Update()
    {
        if (promptTimer > 0)
        {
            promptTimer -= Time.deltaTime;
            if (promptTimer <= 0)
            {
                textBox.close();
            }
        }
    }
}
