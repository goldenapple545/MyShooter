using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTargetReached : MonoBehaviour
{
    public float threshold = 0.02f;
    public Transform target;
    public UnityEvent onEnter;
    public UnityEvent onExit;

    private bool _wasReached = false;

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < threshold && !_wasReached)
        {
            onEnter?.Invoke();
            _wasReached = true;
        } 
        else if (distance >= threshold && _wasReached)
        {
            onExit?.Invoke();
            _wasReached = false;
        }
    }
}
