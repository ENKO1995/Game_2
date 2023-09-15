using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class Type_B_Enemy : MonoBehaviour
{   
    public Animator Animator;
    private static readonly int attacking = Animator.StringToHash("Attacking");
    private static readonly int walking = Animator.StringToHash("Walking");

    public NavMeshAgent Agent;
    public LayerMask WhatIsGround, WhatIsPlayer;

    public Transform Player;

    
    [SerializeField] float health;

    //Patrol
    [SerializeField] Vector3 walkPoint;
    [SerializeField] bool walkPointSet;
    [SerializeField] float walkPointRange;

    //Attack
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] bool alreadyAttacked;

    //AttackEffect
    [SerializeField] private ParticleSystem attackParticle;
    public Text EnemyUIStats;


    //Range
     [SerializeField] float sightRange, attackRange;
     [SerializeField] bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        EnemyUIStats.text = health.ToString();

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            FollowPlayer();
            Animator.SetBool(attacking, false);
        }

        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            Animator.SetBool(attacking, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox") && other.TryGetComponent(out Hitbox hit))
        {
            TakeDamage(hit.GetDamage());
            ParticleAttack();
            if (health > 0)
            {
                AttackPlayer();
            }
        }
    }

    //Random Patroling
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            Agent.SetDestination(walkPoint);
        }

        Agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Set random WalkPoint
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
            walkPointSet = true;
    }

    private void FollowPlayer()
    {
        Agent.SetDestination(Player.position);
    }

    //Attack the Player
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        Agent.SetDestination(transform.position);

        transform.LookAt(Player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void ParticleAttack()
    {
        attackParticle.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}