using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a moving object
/// </summary>
public class MovingObject : MonoBehaviour
{
    //moving direction
    [SerializeField]
    private Vector3 movementPosition;

    //time to move once
    [SerializeField]
    private float movementSpeed = 1;

    //time to pause at destination
    [SerializeField]
    private float pauseTime;

    //smooth movement curve
    [SerializeField]
    private bool smoothMovement;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTimer;
    private float pauseTimer;

    private 

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        targetPosition = startPosition + movementPosition;
        moveTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseTimer > 0)
        {
            //pause
            pauseTimer -= Time.deltaTime;
        } else if (moveTimer > 0)
        {
            moveTimer = Mathf.Max(0, moveTimer - (Time.deltaTime / movementSpeed));
            if (smoothMovement)
            {
                //Sin curve movement
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, Mathf.Cos(moveTimer * Mathf.PI) * 0.5f + 0.5f);
            }
            else
            {
                //linear movement
                transform.localPosition = Vector3.Lerp(targetPosition, startPosition, moveTimer);
            }
        } else
        {
            //swap target and origin and repeat
            Vector3 temp = startPosition;
            startPosition = targetPosition;
            targetPosition = temp;
            pauseTimer = pauseTime;
            moveTimer = 1;
        }
    }
}
