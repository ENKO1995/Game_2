using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private bool reached = false;

    public static Action OnLevelComplete;

    private void OnTriggerEnter(Collider other)
    {
        if (!reached && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            reached = true;
            OnLevelComplete();
        }
    }

}
