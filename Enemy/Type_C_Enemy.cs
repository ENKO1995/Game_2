using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type_C_Enemy : MonoBehaviour
{
    //Destination points
    public Transform[] DestinationPoints;
    public int speed;

    private int desPointsIndex;
    private float distance;

    private void Start()
    {
        desPointsIndex = 0;
        transform.LookAt(DestinationPoints[desPointsIndex].position);
    }
    private void Update()
    {

        distance = Vector3.Distance(transform.position, DestinationPoints[desPointsIndex].position);
        if (distance < 1f)
        {
            IndexUp();
        }
        Patrol();
    }
    void Patrol()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    //Chase next WalkPoint
    void IndexUp()
    {
        desPointsIndex++;
        if (desPointsIndex >= DestinationPoints.Length)
        {
            desPointsIndex = 0;
        }
        transform.LookAt(DestinationPoints[desPointsIndex].position);
    }



}