using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the main controller of the store
/// </summary>
public class StoreController : MonoBehaviour
{
    //list for all store items to sell in order
    [SerializeField]
    private List<StoreItem> storeItems = new List<StoreItem>();

    //array for all store item pedestals
    [SerializeField]
    private ActionBuyItem[] storeStock;

    //delegate for when an item is bought
    public static Action<ActionBuyItem, StoreItem> OnShopPurchase;

    //amount of items that were bought for scene loading
    private static List<StoreItem> purchasedItems = new List<StoreItem>();

    /// <summary>
    /// Start, removes already bought items and stocks pedestals
    /// </summary>
    void Start()
    {
        foreach(StoreItem item in purchasedItems)
        {
            storeItems.Remove(item);
        }
        OnShopPurchase = RestockShop;
        foreach (ActionBuyItem stock in storeStock)
        {
            stock.StockItem(getNextStoreItem());
        }
    }

    /// <summary>
    /// Restocks a pedestal when an item has been bought from it.
    /// </summary>
    /// <param name="_shop">the pedestal</param>
    private void RestockShop(ActionBuyItem _shop, StoreItem _purchased)
    {
        purchasedItems.Add(_purchased);
        _shop.StockItem(getNextStoreItem());
    }

    /// <summary>
    /// Gets the next store item in line or null.
    /// </summary>
    /// <returns>next store item</returns>
    private StoreItem getNextStoreItem()
    {
        if (storeItems.Count > 0)
        {
            StoreItem item = storeItems[0];
            storeItems.RemoveAt(0);
            return item;
        }
        return null;
    }
    
}
