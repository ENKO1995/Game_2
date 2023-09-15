using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

/// <summary>
/// Main class for the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    //Delegate for when the player gets hurt, argument health percentage
    public static Action<float> OnPlayerHurt;
    //Delegate for when the player dies
    public static Action OnPlayerDeath;
    //Delegate for health pickup
    public static Action<int> OnPlayerHeal;

    public static Action OnPlayerStatsChange;

    //Player Stats
    [SerializeField]
    private PlayerStats playerStats;

    //Point to create the player hitbox
    [SerializeField]
    private Transform hitboxSpawnPoint;

    //Hitboxes
    [SerializeField]
    private Hitbox attackHitbox;

    [SerializeField]
    private Hitbox swordHitbox;

    [SerializeField]
    private Hitbox bodyHitbox;

    //Animator
    [SerializeField]
    private Animator animator;

    //Invincibility flashing script
    [SerializeField]
    private RendererFlashing flashEffect;

    //Particles
    [SerializeField]
    private ParticleSystem dashParticles;

    [SerializeField]
    private ParticleSystem dashAttackParticles;

    [SerializeField]
    private ParticleSystem spinAttackChargedParticles;

    [SerializeField]
    private ParticleSystem stompAttackParticles;

    //Reference to player abilities
    private PlayerAbilities abilities;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    //Movement speed on the x/z axis
    private Vector2 hMovement = Vector2.zero;
    //Movement speed on the y axis
    private float vMovement = 0;

    //if the player is on the ground
    private bool grounded = false;

    private bool climbing = false;

    //Dash time remaining
    private float dashTimer = 0;
    //Dash direction
    private Vector2 dashDirection = Vector2.zero;
    //if the player can dash (in the air)
    private bool canDash = true;

    //timer for stomp attack
    private float stompTimer = 0;

    //timer for spin attack
    private float spinChargeTimer = 0;
    private float spinTimer = 0;

    //Time the player can't move for
    private float disableControlTimer = 0;

    //Time the player can't be damaged for
    private float invincibilityTimer = 0;

    //current player health
    private int playerHealth;

    //Camera position for movement direction
    private Transform cameraTransform;

    //current moving platform that the player is a child of
    private Transform parentPlatform;

    private int wallLayer;

    //Initializes variables and delegates
    void Start()
    {
        OnPlayerHeal += HealPlayer;
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerHealth = playerStats.PlayerMaxHealth;
        cameraTransform = Camera.main.transform;
        OnPlayerStatsChange += UpdatePlayerStats;
        SetHitboxDamage(playerStats.PlayerDamage);
        wallLayer = 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Ladder");
    }

    /// <summary>
    /// Updates the damage for all hitboxes.
    /// </summary>
    /// <param name="_damage">damage</param>
    private void SetHitboxDamage(int _damage)
    {
        swordHitbox.SetDamage(_damage);
        attackHitbox.SetDamage(_damage);
        bodyHitbox.SetDamage(_damage);
    }

    /// <summary>
    /// Updates player's stats to ability's upgrades.
    /// Fills health and updates hitboxes.
    /// </summary>
    private void UpdatePlayerStats()
    {
        playerStats.SetTempValues(abilities);
        playerHealth = playerStats.PlayerMaxHealth;
        if (OnPlayerHurt != null)
        {
            OnPlayerHurt((float)playerHealth / playerStats.PlayerMaxHealth);
        }
        SetHitboxDamage(playerStats.PlayerDamage);
    }

    /// <summary>
    /// Gets the direction the player is trying to move in and rotates it based on the camera rotation
    /// </summary>
    /// <returns>The direction the player is trying to move in based on camera rotation</returns>
    private Vector2 GetInputVector(bool _cameraDirection)
    {
        //player can't move
        if (disableControlTimer > 0)
        {
            return Vector2.zero;
        }

        //turn vector by camera direction
        if (_cameraDirection)
        {
            return Quaternion.Euler(0, 0, -cameraTransform.eulerAngles.y) * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        } else
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        }
    }

    /// <summary>
    /// Checks if the player is charging a spin attack.
    /// </summary>
    /// <returns>player is charging spin</returns>
    private bool IsCharging()
    {
        return spinChargeTimer > playerStats.SpinStartChargeTime;
    }

    /// <summary>
    /// Checks if the player is executing a spin attack.
    /// </summary>
    /// <returns>player is spin attacking</returns>
    private bool IsSpinning()
    {
        return spinTimer > 0;
    }

    /// <summary>
    /// Calculates factor for ground movement.
    /// </summary>
    /// <returns>Ground movement factor</returns>
    private float getMovementFactor()
    {
        if (grounded)
        {
            if (IsCharging())
            {
                return playerStats.SpinChargeMoveSpeed;
            }
            if (IsSpinning())
            {
                return playerStats.SpinMoveSpeed;
            }
            return 1;
        } else
        {
            return 1;
        }
    }

    /// <summary>
    /// Handles X/Z axis movement
    /// Speed decreases by player decceleration
    /// Speed increases by player acceleration if moving and slower than max speed
    /// </summary>
    private void HMovementUpdate() {
        Vector2 direction = GetInputVector(true);
        float factor = getMovementFactor();

        if (hMovement.magnitude > 0)
        {
            //decrease speed by decceleration
            hMovement *= Mathf.Max(0, hMovement.magnitude - (grounded ? playerStats.PlayerGroundDecceleration : playerStats.PlayerAirDecceleration) * factor * Time.deltaTime) / hMovement.magnitude;
        }

        //increase by acceleration
        float accel = (grounded ? playerStats.PlayerGroundAcceleration : playerStats.PlayerAirAcceleration) * factor * Time.deltaTime;
        accel = Mathf.Max(0, Mathf.Min(accel, playerStats.PlayerMaxSpeed * factor - hMovement.magnitude));
        hMovement += direction * accel;
        
        //set animator movement speed
        if (grounded)
        {
            animator.SetFloat("RunSpeed", Mathf.Clamp01(hMovement.magnitude / playerStats.PlayerMaxSpeed));
        }

    }

    /// <summary>
    /// Checks wall collision under the player and handles parenting with moving platforms.
    /// </summary>
    /// <returns>If the player is touching a wall under them</returns>
    private bool CheckGrounded()
    {
        if (vMovement > 0)
        {
            if (parentPlatform != null)
            {
                parentPlatform = null;
                transform.SetParent(parentPlatform);
            }
            return false;
        }

        RaycastHit hit = GroundedCast();

        return hit.collider != null;
    }

    /// <summary>
    /// Casts a sphere downward to see if the player is grounded
    /// </summary>
    /// <returns>the raycast hit</returns>
    private RaycastHit GroundedCast()
    {
        Ray cast = new Ray(transform.position + Vector3.up * playerStats.ColliderHeight, Vector3.down);
        RaycastHit hit;
        Physics.SphereCast(cast, playerStats.ColliderRadius, out hit, 0.05f, wallLayer);

        RaycastHit platformHit;
        Physics.SphereCast(cast, playerStats.ColliderRadius, out platformHit, 0.25f, wallLayer);

        if (platformHit.collider != null && platformHit.collider.tag == "MovingPlatform")
        {
            if (parentPlatform != platformHit.collider.transform)
            {
                parentPlatform = platformHit.collider.transform;
                transform.SetParent(parentPlatform);
            }
        }
        else if (parentPlatform != null)
        {
            parentPlatform = null;
            transform.SetParent(parentPlatform);
        }

        return hit;
    }

    /// <summary>
    /// Casts a sphere in the direction the player is moving to see if they touch a wall
    /// </summary>
    /// <returns>The raycast hit</returns>
    private RaycastHit WallslideCast()
    {
        Ray cast = new Ray(transform.position + Vector3.up * ((capsuleCollider.height - 1) * 0.5f), new Vector3(hMovement.x, 0, hMovement.y).normalized);
        RaycastHit hit;
        Physics.SphereCast(cast, /*capsuleCollider.radius **/ 0.02f, out hit, capsuleCollider.radius, wallLayer);
        return hit;
    }

    /// <summary>
    /// Casts a sphere in the direction the player is moving to see if they can climb a wall
    /// </summary>
    /// <returns></returns>
    private RaycastHit WallClimbCast()
    {
        Ray cast = new Ray(transform.position, new Vector3(Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y), 0, Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y)).normalized);
        RaycastHit hit;
        Physics.SphereCast(cast, /*capsuleCollider.radius **/ 0.02f, out hit, capsuleCollider.radius, wallLayer);
        return hit;
    }

    /// <summary>
    /// Checks if the player is sliding on a vertical wall
    /// </summary>
    /// <returns>The player is wallsliding</returns>
    private bool CheckWallSlide()
    {
        RaycastHit hit = WallslideCast();

        if (hit.collider != null && hit.normal.y == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if a cast collided with a climbable wall.
    /// </summary>
    /// <returns>Player can climb</returns>
    private bool CheckWallClimb()
    {
        RaycastHit hit = WallClimbCast();

        if (hit.collider != null && hit.normal.y == 0 && hit.collider.tag == "Climbable")
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Moves the player along a wall when climbing.
    /// </summary>
    private void WallClimbMovement()
    {
        RaycastHit hit = WallClimbCast();
        vMovement = 0;

        //moves the player into the wall so they stick to it
        hMovement = -new Vector2(hit.normal.x, hit.normal.z);
        transform.rotation = Quaternion.Euler(0, -Vector2.SignedAngle(Vector2.up, hMovement.normalized), 0);

        //moves the player along the wall's normal vector
        Vector2 movement = GetInputVector(false);
        vMovement = movement.y * playerStats.PlayerWallClimbSpeed;
        hMovement.x += movement.x * Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y) * playerStats.PlayerWallClimbSpeed;
        hMovement.y -= movement.x * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y) * playerStats.PlayerWallClimbSpeed;

        //animator climb speed
        animator.SetFloat("ClimbSpeed", movement.magnitude);
    }

    /// <summary>
    /// If the player is touching a vertical wall, jump off of it and reset dash
    /// </summary>
    private void TryWallJump()
    {
        RaycastHit hit = WallslideCast();

        if (hit.collider != null && hit.normal.y == 0)
        {
            //jump off the wall
            hMovement = new Vector2(hit.normal.x, hit.normal.z) * playerStats.PlayerWallJumpSpeed;
            vMovement = playerStats.PlayerWallJumpHeight;
            dashTimer = 0;
            dashParticles.Stop();
            dashAttackParticles.Stop();
            bodyHitbox.gameObject.SetActive(false);
            animator.SetTrigger("StartJump");
            climbing = false;
        }
    }

    /// <summary>
    /// Handles Y axis movement
    /// Accelerates downwards in the air
    /// If wallsliding, downwards acceleration and max speed is lowered
    /// </summary>
    private void VMovementUpdate()
    {
        if (grounded)
        {
            //makes player stay on the floor
            vMovement = 0;
        } else
        {
            //Check for wall slide to reduce falling speed
            float factor = (vMovement < 0 && spinTimer <= 0 && CheckWallSlide()) ? playerStats.PlayerWallSlideSpeed : 1;
            vMovement -= playerStats.PlayerGravity * Time.deltaTime * factor;
            
            if (vMovement < -playerStats.PlayerMaxFallSpeed * factor)
            {
                vMovement = -playerStats.PlayerMaxFallSpeed * factor;
            }
        }
    }

    /// <summary>
    /// Updates the angle the player is facing
    /// When spinning the player will rotate
    /// </summary>
    private void UpdatePlayerAngle()
    {
        if (spinChargeTimer < playerStats.SpinStartChargeTime)
        {
            if (spinTimer > 0) {
                transform.Rotate(Vector3.up, playerStats.SpinRotateSpeed * Time.deltaTime);
            } else if (hMovement.magnitude > 0)
            {
                transform.rotation = Quaternion.Euler(0, -Vector2.SignedAngle(Vector2.up, hMovement.normalized), 0);
            }
        }
    }

    /// <summary>
    /// Player jumps
    /// </summary>
    private void PlayerJump()
    {
        vMovement = playerStats.PlayerJumpHeight;
        dashTimer = 0;
        dashParticles.Stop();
        dashAttackParticles.Stop();
        animator.SetTrigger("StartJump");
    }

    /// <summary>
    /// Check if the player is dashing
    /// </summary>
    /// <returns>The player is dashing</returns>
    private bool IsDashing()
    {
        return dashTimer > 0;
    }

    /// <summary>
    /// Starts a dash, sets dash direction
    /// </summary>
    private void StartDash()
    {
        if (abilities.HasAbility(AbilityNames.DashAttack))
        {
            dashAttackParticles.Play();
        } else
        {
            dashParticles.Play();
        }
        canDash = false;
        dashTimer = playerStats.DashTime;

        //dash in stick direction
        dashDirection = GetInputVector(true);
        animator.SetTrigger("StartDash");

        //if no stick direction, dash in movement direction
        if (dashDirection.magnitude == 0)
        {
            dashDirection = hMovement.normalized;
        }

        //if no movement direction, dash in looking direction
        if (hMovement.magnitude == 0)
        {
            dashDirection = new Vector2(Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y), Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y)).normalized;
        }

        //dash attack activate hitbox
        if (abilities.HasAbility(AbilityNames.DashAttack))
        {
            bodyHitbox.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Counts down the dash timer and sets speed to dash direction
    /// </summary>
    private void DashUpdate()
    {
        hMovement = dashDirection * playerStats.DashSpeed;
        vMovement = 0;
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0)
        {
            //stop dash
            bodyHitbox.gameObject.SetActive(false);
            dashParticles.Stop();
            dashAttackParticles.Stop();
            if (hMovement.magnitude > playerStats.DashEndSpeed)
            {
                hMovement *= playerStats.DashEndSpeed / hMovement.magnitude;
            }
        }
    }

    /// <summary>
    /// Checks if the player is stomping
    /// </summary>
    /// <returns>Player is stomping</returns>
    private bool IsStomping()
    {
        return stompTimer > 0;
    }

    /// <summary>
    /// Update during the stomp attack
    /// </summary>
    private void StompUpdate()
    {
        //locks horizontal movement
        hMovement = Vector2.zero;

        //disables controls
        disableControlTimer = playerStats.StompStun;
        if (stompTimer == 0)
        {
            //stomp start
            animator.SetTrigger("StartStomp");
        }
        if (stompTimer < playerStats.StompPauseTime)
        {
            //stomp beginning, stall in the air
            stompTimer += Time.deltaTime;
            vMovement = 0;
        } else
        {
            //stomp falling
            bodyHitbox.gameObject.SetActive(true);
            vMovement = -playerStats.StompSpeed;
            if (grounded)
            {
                //stomp landing
                stompTimer = 0;
                stompAttackParticles.Play();
                bodyHitbox.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Update during the spin charge
    /// </summary>
    /// <param name="charging">currently charging</param>
    private void ChargeSpin(bool charging)
    {
        if (charging && spinChargeTimer < playerStats.SpinChargeTime)
        {
            //charging
            spinChargeTimer += Time.deltaTime;
            if (spinChargeTimer >= playerStats.SpinChargeTime)
            {
                //charge is full, play particle
                spinAttackChargedParticles.Play();
            }
        }

        if (!charging)
        {
            if (grounded && spinChargeTimer >= playerStats.SpinChargeTime)
            {
                //let go with full charge, start spin
                StartSpin();
            }
            spinChargeTimer = 0;
        }
    }

    /// <summary>
    /// Starts the spin attack.
    /// </summary>
    private void StartSpin()
    {
        spinAttackChargedParticles.Stop();
        spinTimer = playerStats.SpinTime;
        SwordHitboxState(true);
    }

    /// <summary>
    /// Reduces Player health by damage and stuns the player for a time
    /// Calls OnPlayerHurt and OnPlayerDeath if health is 0
    /// </summary>
    /// <param name="_damage">the damage</param>
    /// <param name="_stun">how long the player will be stunned</param>
    public void TakeDamage(int _damage, bool _stun)
    {
        //player can't be hit when invincible or spinning
        if (invincibilityTimer <= 0 && !IsSpinning())
        {
            int previous = playerHealth;
            playerHealth = Mathf.Max(0, playerHealth - _damage);
            if (_stun)
            {
                //player takes hitstun, stop actions
                disableControlTimer = playerStats.OnHitStun;
                invincibilityTimer = playerStats.OnHitInvincibility;
                dashTimer = 0;
                dashParticles.Stop();
                dashAttackParticles.Stop();
                spinAttackChargedParticles.Stop();
                spinChargeTimer = 0;
                spinTimer = 0;
                SwordHitboxState(false);
                bodyHitbox.gameObject.SetActive(false);
            }

            animator.SetTrigger("TakeDamage");

            //player hurt
            if (OnPlayerHurt != null)
            {
                OnPlayerHurt((float)playerHealth / playerStats.PlayerMaxHealth);
            }

            //player death
            if (previous > 0 && playerHealth == 0)
            {
                animator.SetBool("PlayerDead", true);
                invincibilityTimer = 0;
                if (OnPlayerDeath != null)
                {
                    OnPlayerDeath();
                }
            }
        }
    }

    /// <summary>
    /// Restores the player's health by a certain amount.
    /// </summary>
    /// <param name="_healAmount">the amount to heal</param>
    public void HealPlayer(int _healAmount)
    {
        playerHealth = Mathf.Min(playerHealth + _healAmount, playerStats.PlayerMaxHealth);

        //updates UI
        if (OnPlayerHurt != null)
        {
            OnPlayerHurt((float)playerHealth / playerStats.PlayerMaxHealth);
        }
    }

    /// <summary>
    /// Reduces Player health by damage and stuns the player for a time
    /// Also sets the player's speed
    /// Calls OnPlayerHurt and OnPlayerDeath if health is 0
    /// </summary>
    /// <param name="_damage">the damage</param>
    /// <param name="_stun">how long the player will be stunned</param>
    /// <param name="_knockback">The knockback of the attack</param>
    public void TakeDamage(int _damage, bool _stun, Vector3 _knockback)
    {
        if (invincibilityTimer <= 0)
        {
            TakeDamage(_damage, _stun);
            hMovement = new Vector2(_knockback.x, _knockback.z);
            vMovement = _knockback.y;
        }
    }

    /// <summary>
    /// Counts down the player's timers
    /// </summary>
    private void DecreaseTimers()
    {
        if (disableControlTimer > 0 && playerHealth > 0)
        {
            disableControlTimer -= Time.deltaTime;
            if (disableControlTimer <= 0 && invincibilityTimer > 0)
            {
                flashEffect.SetFlashTime(invincibilityTimer);
            }
        }

        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        if (spinTimer > 0)
        {
            spinTimer -= Time.deltaTime;
            if (spinTimer <= 0)
            {
                SwordHitboxState(false);
            }
        }
    }

    /// <summary>
    /// Sets a reference to PlayerAbilities
    /// </summary>
    /// <param name="_abilities">abilities</param>
    public void SetAbilities(PlayerAbilities _abilities)
    {
        this.abilities = _abilities;
        playerStats.SetTempValues(abilities);
    }

    /// <summary>
    /// Check if the player can be hurt
    /// </summary>
    /// <returns></returns>
    public bool IsInvincible()
    {
        return invincibilityTimer > 0;
    }

    /// <summary>
    /// Sets the state of the sword hitbox
    /// </summary>
    /// <param name="_active">state</param>
    public void SwordHitboxState(bool _active)
    {
        swordHitbox.gameObject.SetActive(_active);
    }

    /// <summary>
    /// Player falls off the map and respawns with damage.
    /// </summary>
    public void TriggerVoidOut()
    {
        RespawnPoint respawn = RespawnPoint.GetCurrentRespawn();

        if (respawn == null)
        {
            TakeDamage(playerStats.PlayerMaxHealth, false);
        } else
        {
            UIController.OnTransition(playerStats.VoidOutTime);
            Invoke("RespawnPlayer", playerStats.VoidOutTime - 0.5f);
        }
    }

    /// <summary>
    /// Respawn the player on the last respawn platform. If there is none, kill player.
    /// </summary>
    private void RespawnPlayer()
    {
        RespawnPoint respawn = RespawnPoint.GetCurrentRespawn();

        if (respawn != null)
        {
            transform.position = respawn.transform.position;
            TakeDamage(playerStats.VoidOutDamage, false);
            invincibilityTimer = playerStats.OnHitInvincibility;
            flashEffect.SetFlashTime(invincibilityTimer);
        }
    }

    //Calls update functions
    void Update()
    {
        DecreaseTimers();

        bool landing = grounded;
        grounded = CheckGrounded();
        animator.SetBool("Grounded", grounded);

        //jump landing
        if (grounded && !IsStomping() && !landing && vMovement < -5)
        {
            animator.SetTrigger("Landing");
        }

        climbing = false;

        if (IsStomping())
        {
            //stomp
            StompUpdate();
        } else if (IsDashing())
        {
            //dash
            DashUpdate();
        }
        else
        {
            if (!(grounded && Input.GetAxis("Vertical") < 0) && spinTimer <= 0 && abilities.HasAbility(AbilityNames.Climb) && CheckWallClimb())
            {
                //wall climb
                WallClimbMovement();
                climbing = true;
            }
            else
            {
                //normal movement
                HMovementUpdate();
                VMovementUpdate();
            }
        }

        if (disableControlTimer <= 0 && canDash && !climbing && Input.GetButtonDown("Dash"))
        {
            StartDash();
        }

        if (disableControlTimer <= 0 && abilities.HasAbility(AbilityNames.SpinAttack))
        {
            //charge if grounded and holding attack
            ChargeSpin(grounded && Input.GetButton("Attack"));
            if (landing && !grounded)
            {
                spinAttackChargedParticles.Stop();
            }
        }

        if (grounded)
        {
            //reset air dash
            canDash = true;
            if (disableControlTimer <= 0 && spinTimer <= 0 && Input.GetButton("Jump"))
            {
                PlayerJump();
            }
        } else if (disableControlTimer <= 0 && spinTimer <= 0 && Input.GetButton("Jump")) {
            TryWallJump();
        }

        if (disableControlTimer <= 0 && spinTimer <= 0 && Input.GetButtonDown("Attack"))
        {
            if (!grounded && !IsStomping() && abilities.HasAbility(AbilityNames.StompAttack))
            {
                //start stomp
                StompUpdate();
            }
            else
            {
                animator.SetTrigger("StartAttack");
            }
        }

        animator.SetBool("Dashing", IsDashing());
        animator.SetBool("Climbing", climbing);
        animator.SetBool("Charging", IsCharging());
        animator.SetBool("Spinning", IsSpinning());
        if (!climbing)
        {
            UpdatePlayerAngle();
            //set speed to 1 so animator can exit climbing animation
            animator.SetFloat("ClimbSpeed", 1);
        }

        if (grounded)
        {
            RaycastHit floor;
            Physics.Raycast(transform.position, Vector3.down, out floor, 0.1f, wallLayer);

            if (floor.collider != null)
            {
                Vector3 slopeMovement = Vector3.Cross(floor.normal, new Vector3(hMovement.x, 0, hMovement.y));
                slopeMovement = Vector3.Cross(floor.normal, slopeMovement);
                rb.velocity = -slopeMovement + new Vector3(0, vMovement, 0);
            } else
            {
                rb.velocity = new Vector3(hMovement.x, vMovement, hMovement.y);
            }
        }
        else
        {
            rb.velocity = new Vector3(hMovement.x, vMovement, hMovement.y);
        }
    }
    
}
