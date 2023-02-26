using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool forceful = false;
    [SerializeField] private float speed = 100f;
    private void FixedUpdate()
    {
        if (forceful)
        {
            transform.position = target.position;
            return;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
