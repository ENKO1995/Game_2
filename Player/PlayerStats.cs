using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object for the player's stats
/// </summary>
[CreateAssetMenu(fileName = "New PlayerStats", menuName = "Player Stats", order = 51)]
public class PlayerStats : ScriptableObject
{
    //movement
    public float PlayerMaxSpeed;
    public float PlayerGroundAcceleration;
    public float PlayerGroundDecceleration;
    public float PlayerAirAcceleration;
    public float PlayerAirDecceleration;
    public float PlayerGravity;
    public float PlayerMaxFallSpeed;
    public float PlayerJumpHeight;

    //dash
    public float DashSpeed;
    public float DashTime;
    public float DashEndSpeed;

    //health
    public int PlayerMaxHealthBase;
    public int PlayerMaxHealth;

    //damage
    public int PlayerDamageBase;
    public int PlayerDamage;

    //walljump
    public float PlayerWallSlideSpeed;
    public float PlayerWallJumpSpeed;
    public float PlayerWallJumpHeight;

    //wallclimb
    public float PlayerWallClimbSpeed;

    //on hit
    public float OnHitStun;
    public float OnHitInvincibility;

    //stomp
    public float StompPauseTime;
    public float StompSpeed;
    public float StompStun;

    //spin
    public float SpinStartChargeTime;
    public float SpinChargeTime;
    public float SpinChargeMoveSpeed;
    public float SpinTime;
    public float SpinMoveSpeed;
    public float SpinRotateSpeed;

    //ground check
    public float ColliderHeight;
    public float ColliderRadius;

    //void out
    public int VoidOutDamage;
    public float VoidOutTime;

    /// <summary>
    /// Sets the temporary stats based on player upgrades.
    /// </summary>
    /// <param name="_playerAbilities">player stat upgrades</param>
    public void SetTempValues(PlayerAbilities _playerAbilities)
    {
        PlayerMaxHealth = PlayerMaxHealthBase + _playerAbilities.GetStatUpgrade(UpgradeNames.Health);
        PlayerDamage = PlayerDamageBase + _playerAbilities.GetStatUpgrade(UpgradeNames.Damage);
    }
}
