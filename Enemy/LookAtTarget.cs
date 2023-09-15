using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LayerMask objectSelectionMask;
    [SerializeField] private float distanceRay;

    [SerializeField] private GameObject soundFire;
    [SerializeField] private Transform mTransform;
    private Transform rayTransform;

    float nextSpawnTime;
    float nextDeltaTime = 0.5f;

    RaycastHit hit;

    private void Awake()
    {
        rayTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        mTransform = GameObject.FindWithTag("Player").transform;
        nextSpawnTime = Time.time + nextDeltaTime;
    }

    private void Update()
    {
        Ray ray = new Ray(rayTransform.position, rayTransform.forward);

        Debug.DrawRay(rayTransform.position, rayTransform.forward * distanceRay);

        if (Time.time > nextSpawnTime)
        {
            if (Physics.Raycast(ray, out RaycastHit raycastInfo, distanceRay, objectSelectionMask))
            {
                Instantiate(bulletPrefab, soundFire.transform.position, soundFire.transform.rotation);
            }
            nextSpawnTime += nextDeltaTime;
        }
        transform.LookAt(mTransform.position);
    }
}