using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckSlideMovement : MonoBehaviour
{
    [SerializeField] private GunAnimator gunAnimator;
    public float threshold = 0.02f;
    public Transform target;
    public UnityEvent onEnter;
    public UnityEvent onExit;
    
    private bool _wasReached = false;
    private float _startDistance;

    private void Start()
    {
        _startDistance = Vector3.Distance(transform.position, target.position);

        if (gunAnimator == null)
        {
            Debug.Log("Slide animator not connected");
        }
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        float slideValue = Mathf.InverseLerp(_startDistance, threshold, distance);
        if (gunAnimator != null)
            gunAnimator.SetSlideValue(slideValue);

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
