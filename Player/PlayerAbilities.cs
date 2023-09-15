using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityNames
{
    Climb, Sneak, StompAttack, SpinAttack, ShootAttack, DashAttack
}

public enum UpgradeNames
{
    Health, Damage
}

/// <summary>
/// Class for player stat upgrades and abilities
/// </summary>
public class PlayerAbilities
{
    //list of active abilities
    private List<AbilityNames> activeAbilities = new List<AbilityNames>();

    //list of stat upgrades
    private int healthUpgrades = 0;
    private int damageUpgrades = 0;

    public void ApplyItem(StoreItemType _itemType)
    {
        switch(_itemType)
        {
            case StoreItemType.Climb:
                EquipAbility(AbilityNames.Climb);
                break;
            case StoreItemType.DashAttack:
                EquipAbility(AbilityNames.DashAttack);
                break;
            case StoreItemType.ShootAttack:
                EquipAbility(AbilityNames.ShootAttack);
                break;
            case StoreItemType.SpinAttack:
                EquipAbility(AbilityNames.SpinAttack);
                break;
            case StoreItemType.StompAttack:
                EquipAbility(AbilityNames.StompAttack);
                break;
            case StoreItemType.Sneak:
                EquipAbility(AbilityNames.Sneak);
                break;
            case StoreItemType.Health:
                UpgradeStats(UpgradeNames.Health, 10);
                if (PlayerController.OnPlayerStatsChange != null)
                {
                    PlayerController.OnPlayerStatsChange();
                }
                break;
            case StoreItemType.Damage:
                UpgradeStats(UpgradeNames.Damage, 5);
                if (PlayerController.OnPlayerStatsChange != null)
                {
                    PlayerController.OnPlayerStatsChange();
                }
                break;
        }
    }

    /// <summary>
    /// Increase a stat by a value
    /// </summary>
    /// <param name="_upgrade">The Stat</param>
    /// <param name="_value">The Value</param>
    public void UpgradeStats(UpgradeNames _upgrade, int _value)
    {
        switch(_upgrade)
        {
            case UpgradeNames.Health:
                healthUpgrades += _value;
                break;
            case UpgradeNames.Damage:
                damageUpgrades += _value;
                break;
        }
    }

    /// <summary>
    /// Return stat upgrades
    /// </summary>
    /// <param name="_upgrade">The Stat</param>
    /// <returns></returns>
    public int GetStatUpgrade(UpgradeNames _upgrade)
    {
        switch (_upgrade)
        {
            case UpgradeNames.Health:
                return healthUpgrades;
            case UpgradeNames.Damage:
                return damageUpgrades;
        }
        return 0;
    }
    
    /// <summary>
    /// Resets the list of active abilities
    /// </summary>
    public void ResetAbilities()
    {
        activeAbilities.Clear();
    }

    /// <summary>
    /// Adds an ability to the actives list
    /// </summary>
    /// <param name="_ability">The ability</param>
    public void EquipAbility(AbilityNames _ability)
    {
        activeAbilities.Add(_ability);
    }

    /// <summary>
    /// Removes an ability from the actives list
    /// </summary>
    /// <param name="_ability">The Ability</param>
    public void UnequipAbility(AbilityNames _ability)
    {
        activeAbilities.Remove(_ability);
    }

    /// <summary>
    /// Checks if an ability is equipped
    /// </summary>
    /// <param name="_ability">The ability</param>
    /// <returns>Ability is equipped</returns>
    public bool HasAbility(AbilityNames _ability)
    {
        return activeAbilities.Contains(_ability);
    }
}

