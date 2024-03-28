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
    public Animator gunAnimator;

    private bool _wasReached = false;
    private float _startPosition;
    private float _endPosition;
    private float _startDistance;

    private void Start()
    {
        _startDistance = Vector3.Distance(transform.position, target.position);
        // _startPosition = transform.position.z;
        // _endPosition = target.position.z;
        // Debug.Log(_startPosition);
        // Debug.Log(_endPosition);
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        //float distance = transform.position.z - target.position.z;
        float reachedValue = Mathf.InverseLerp(_startDistance, threshold, distance);
        gunAnimator.SetFloat("MoveSlider", reachedValue);
        
        Debug.Log(reachedValue);
        //Debug.Log(distance);

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
