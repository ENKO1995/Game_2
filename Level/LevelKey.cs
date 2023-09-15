using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a key that ends the level when taken.
/// </summary>
public class LevelKey : MonoBehaviour
{
    //Number of the key
    [SerializeField]
    private int keyId;

    //key not collected
    [SerializeField]
    private GameObject keyNew;

    //key duplicate
    [SerializeField]
    private GameObject keyDuplicate;

    private void Start()
    {
        bool collected = GameController.gameController.PlayerHasKey(keyId);

        keyNew.SetActive(!collected);
        keyDuplicate.SetActive(collected);
    }

    /// <summary>
    /// Trigger key collection and destroy.
    /// </summary>
    /// <param name="other">collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameController.gameController.CollectKey(keyId);
            Destroy(gameObject);
        }
    }
}
