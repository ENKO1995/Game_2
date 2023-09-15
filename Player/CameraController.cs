using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CameraController : MonoBehaviour
{
    public GameObject FollowTransform;

    private float rotationPower = 0.1f;
    private Vector2 _Look;
    private Vector2 _Move;

    public void OnLook(InputValue value)
    {
        if (_Move.x == 0 && _Move.y == 0)
        {
            _Look = value.Get<Vector2>();

            FollowTransform.transform.rotation *= Quaternion.AngleAxis(_Look.x * rotationPower, Vector3.up);
            FollowTransform.transform.rotation *= Quaternion.AngleAxis(_Look.y * rotationPower, Vector3.right);
        }
    }
}
