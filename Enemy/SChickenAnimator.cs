using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SChickenAnimator : MonoBehaviour
{
    public int healthEnemy;
    
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void AnimationMS()
    {
            _animator.Play("Run In Place");
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox") && other.TryGetComponent(out Hitbox hit))
        {
           healthEnemy -= hit.GetDamage();
           

            if (healthEnemy <= 0)
            {
                //_animator.Play("Eat");

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

    //// private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        _animator.Play("Eat");
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        _animator.Play("Eat");
    //    }
    //}

}
