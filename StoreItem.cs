using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum for all store item types
public enum StoreItemType
{
    Climb, Sneak, StompAttack, SpinAttack, ShootAttack, DashAttack, Health, Damage
}

/// <summary>
/// Scriptable item for a store item
/// </summary>
[CreateAssetMenu]
public class StoreItem : ScriptableObject
{
    public StoreItemType ItemType;
    public int ItemPrice;
    //number of the mesh to display
    public int MeshID;

    public string ItemDescription;
    [TextArea]
    public string ItemTextPopup;
}
