using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class Recoil : MonoBehaviour
{
    // Rotations
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;
    
    // Hipfire recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;
    
    // Settings
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    public void InvokeReturn(Transform target)
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, snappiness* Time.fixedDeltaTime);
        target.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void RecoilFire()
    {
        _targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
