using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that opens a chest to spawn pickups
/// </summary>
public class ActionOpenChest : SwitchActivation
{
    //prefabs for physic coins
    [SerializeField]
    protected GameObject[] coinPrefabs;

    //prefabs for physic health
    [SerializeField]
    protected GameObject[] healthPrefabs;

    //prosition to spawn pickups from
    [SerializeField]
    protected Transform spawnPosition;

    //coins in the chest
    [SerializeField]
    protected int coins = 0;

    [SerializeField]
    protected int redHearts = 0;

    [SerializeField]
    protected int goldHearts = 0;

    [SerializeField]
    protected SwitchActivation openLid;

    protected List<GameObject> loot = new List<GameObject>();

    protected static float spawnDelay = 0.2f;
    protected static float minUpwardsForce = 6;
    protected static float maxUpwardsForce = 8;
    protected static float minSidewaysForce = 1.5f;
    protected static float maxSidewaysForce = 3;

    protected void Start()
    {
        InstantiateLoot();
    }

    /// <summary>
    /// Creates all pickups and deactivates them to activate when opened.
    /// </summary>
    protected void InstantiateLoot()
    {
        int currentCoins = coins;
        while (currentCoins > 10)
        {
            SpawnObject(coinPrefabs[2]);
            currentCoins -= 10;
        }

        while (currentCoins > 5)
        {
            SpawnObject(coinPrefabs[1]);
            currentCoins -= 5;
        }

        while (currentCoins > 0)
        {
            SpawnObject(coinPrefabs[0]);
            currentCoins -= 1;
        }

        for (int i = 0; i < redHearts; i++)
        {
            SpawnObject(healthPrefabs[0]);
        }

        for (int i = 0; i < goldHearts; i++)
        {
            SpawnObject(healthPrefabs[1]);
        }
    }

    public override void SwitchFunction(bool _activated)
    {
        openLid.SwitchFunction(true);
        Invoke("SpawnAllLoot", spawnDelay);
    }

    /// <summary>
    /// Activates all instantiated loot.
    /// </summary>
    protected void SpawnAllLoot()
    {
        foreach (GameObject o in loot)
        {
            o.SetActive(true);
        }
    }

    /// <summary>
    /// Creates an object as a child and deactivates it.
    /// Adds it to the loot list.
    /// </summary>
    /// <param name="_prefab">object</param>
    protected void SpawnObject(GameObject _prefab)
    {
        GameObject o = Instantiate(_prefab, spawnPosition);
        o.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * new Vector3(Random.Range(minSidewaysForce, maxSidewaysForce), Random.Range(minUpwardsForce, maxUpwardsForce), 0);
        loot.Add(o);
        o.SetActive(false);
    }

}
