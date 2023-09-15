using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// The Main Game Controller class
/// </summary>
public class GameController : MonoBehaviour
{
    //Static reference to the game controller
    public static GameController gameController;

    //reference to player upgrades and skills
    private PlayerAbilities playerAbilities;

    //coin counter
    private int coinCount = 0;

    //delegate for UI
    public static Action<int> OnCoinCountChanged;

    //saves values for completed Levels
    private static int keysTotal = 2;
    private bool[] keysCollected = new bool[keysTotal];

    //next scene to be loaded
    private string nextScene;

    //Creates gamecontroller on start
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void CreateGameController()
    {
        gameController = new GameObject().AddComponent<GameController>();
        gameController.Initialize();
    }

    /// <summary>
    /// Starts the game controller
    /// </summary>
    private void Initialize()
    {
        //
        playerAbilities = new PlayerAbilities();
        ActionBuyItem.OnItemPurchase += ApplyItemPurchase;

        //Player can always climb
        playerAbilities.EquipAbility(AbilityNames.Climb);

        PlayerController.OnPlayerDeath += LevelFailed;
        SceneManager.sceneLoaded += OnSceneLoad;
        Coin.OnCoinPickup += IncreaseCoinCount;
        ActionBuyItem.OnItemPurchase += SubtractItemPrice;

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Level failed, display text and go back to hub.
    /// </summary>
    private void LevelFailed()
    {
        UIController.OnGlobalMessage("<color=\"red\">Game Over</color>", 3);
        SceneTransition("Level_Dennis", 3.5f);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartPlayer();
    }

    /// <summary>
    /// Sets the player's abilities
    /// </summary>
    private void StartPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.SetAbilities(playerAbilities);
        }
    }

    /// <summary>
    /// Returns global coin count.
    /// </summary>
    /// <returns>coin count</returns>
    public int GetPlayerCoins()
    {
        return coinCount;
    }

    /// <summary>
    /// Applies stats and abilities to player abilities and displays text.
    /// </summary>
    /// <param name="_item">The bought item</param>
    public void ApplyItemPurchase(StoreItem _item)
    {
        playerAbilities.ApplyItem(_item.ItemType);
        switch (_item.ItemType)
        {
            case StoreItemType.Health:
                UIController.OnGlobalMessage(_item.ItemTextPopup + (100 + playerAbilities.GetStatUpgrade(UpgradeNames.Health)) + "!", 3);
                break;
            case StoreItemType.Damage:
                UIController.OnGlobalMessage(_item.ItemTextPopup + (10 + playerAbilities.GetStatUpgrade(UpgradeNames.Damage)) + "!", 3);
                break;
            default:
                UIController.OnGlobalMessage(_item.ItemTextPopup, 5);
                break;
        }
    }

    /// <summary>
    /// Check if the player has collected a certain key.
    /// </summary>
    /// <param name="_keyId">The number of the key</param>
    /// <returns>Player has collected the key</returns>
    public bool PlayerHasKey(int _keyId)
    {
        return keysCollected[_keyId];
    }

    /// <summary>
    /// Checks if the player has collected all keys in the game.
    /// </summary>
    /// <returns>Player has collected all keys</returns>
    public bool PlayerHasAllKeys()
    {
        for (int i = 0; i < keysCollected.Length; i++)
        {
            if (!keysCollected[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Saves a key as collected and displays text.
    /// </summary>
    /// <param name="_keyId">The number of the key</param>
    public void CollectKey(int _keyId)
    {
        keysCollected[_keyId] = true;
        UIController.OnGlobalMessage("<color=\"yellow\">Key Collected!</color>", 3);
        SceneTransition("Level_Dennis", 3.5f);
    }

    /// <summary>
    /// Loads a different scene in a certain amount if time.
    /// </summary>
    /// <param name="_loadScene">The scene to load</param>
    /// <param name="_inTime">The amount of time to wait</param>
    public void SceneTransition(string _loadScene, float _inTime)
    {
        nextScene = _loadScene;
        Invoke("SwitchScene", _inTime);
    }

    /// <summary>
    /// Loads the next scene.
    /// </summary>
    private void SwitchScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    /// <summary>
    /// Increases the player's coin count.
    /// </summary>
    /// <param name="_amount">Amount to add</param>
    private void IncreaseCoinCount(int _amount)
    {
        coinCount += _amount;
        if (OnCoinCountChanged != null)
        {
            OnCoinCountChanged(coinCount);
        }
    }

    /// <summary>
    /// Subtracts a store item's price from the player's coins
    /// </summary>
    /// <param name="_item"></param>
    private void SubtractItemPrice(StoreItem _item)
    {
        IncreaseCoinCount(-_item.ItemPrice);
    }
}
