using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Type_A_Enemy : MonoBehaviour
{
	//NavMesh
	[SerializeField]
    private float lookRadius = 10f;
    Transform target;
	NavMeshAgent agent;
	
	[SerializeField] int healthEnemy;
	[SerializeField] private Animator _animator;

	

	void Start()
	{
		_animator = GetComponent<Animator>();
		target = GameObject.FindWithTag("Player").transform;
		agent = GetComponent<NavMeshAgent>();
		
	}
	void Update()
	{
		// Get the distance to the player
		float distance = Vector3.Distance(target.position, transform.position);

		// If inside the radius
		if (distance <= lookRadius)
		{
			// Move in direction of the player
			agent.SetDestination(target.position);
			if (distance <= agent.stoppingDistance)
			{
				FaceTarget();
			}
		}
	}

	// Point towards the player
	void FaceTarget()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox") && other.TryGetComponent(out Hitbox hit))
		{
			healthEnemy -= hit.GetDamage();
           

			if (healthEnemy <= 0)
			{
				//_animator.Play("Eat");
				AnimationMS();
				Destroy(gameObject);
			}
		}
	}
	
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			_animator.Play("Eat");
		}
	}
	
	void AnimationMS()
	{
		_animator.Play("Run In Place");
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}
}
