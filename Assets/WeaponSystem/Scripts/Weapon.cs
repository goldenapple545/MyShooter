using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class Weapon: XRGrabInteractable, IWeaponActions
{
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private GameObject bulletPrefab;
    public long ID;
    public int ShotPower
    {
        get { return _shotPower; }
        set
        {
            if (value > 0) _shotPower = value;
        }
    }

    private int _shotPower = 5000;
    private bool _isReadyToFire;
    private Animator _gunAnimator;

    private void Start()
    {
        _gunAnimator = gameObject.GetComponent<Animator>();
        
        _isReadyToFire = true;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected) 
                PullTheTrigger();
        }
    } 

    public void Shoot()
    {
        if (_isReadyToFire)
        {
            GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * _shotPower);
        }
    }
    
    public void PullTheTrigger()
    {
        if (firstInteractorSelecting is XRBaseControllerInteractor interactor)
        {
            InteractionState activateState = interactor.xrController.activateInteractionState;
            _gunAnimator.SetFloat("PullTrigger", activateState.value);
            Shoot();
        }
    }

    public bool Reload(int ammoCount)
    {
        throw new NotImplementedException();
    }
    
}
