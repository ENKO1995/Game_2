using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTargetBullet : MonoBehaviour
{
    [SerializeField] private float force;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
        Destroy(gameObject, 2.0f);
    }
}
