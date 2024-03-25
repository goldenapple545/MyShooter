using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class Weapon: XRGrabInteractable, IWeaponActions
{
    [Header("Weapon Settings")]
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform shellLocation;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private GameObject casePrefab;
    [SerializeField] private AudioSource shootAudioSource;
    
    public long ID;
    public int ShotPower
    {
        get { return _shotPower; }
        set
        {
            if (value > 0) _shotPower = value;
        }
    }
    public float ValueToFire {
        get { return _valueToFire; }
        set
        {
            if (value > 0 && value < 1) _valueToFire = value;
        }
    }

    private string _pressTriggerName = "PressTrigger";
    private string _fireName = "Fire";
    private float _valueToFire = 0.9f;
    private int _shotPower = 5000;
    private int _caseEjectPower = 100;
    private bool _canEjectCase = true;
    private bool _isReadyToFire;
    private Animator _gunAnimator;
    private bool _isNextShotReady = true;

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
                if (firstInteractorSelecting is XRBaseControllerInteractor interactor)
                {
                    InteractionState activateState = interactor.xrController.activateInteractionState;
                    CheckPressingTrigger(activateState.value);
                }
        }
    } 

    public void Shoot()
    {
        if (_isReadyToFire)
        {
            SpawnMuzzleFlash();
            PlaySound(shootAudioSource);
            GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * _shotPower);
        }
    }

    private void PlaySound(AudioSource audioSource)
    {
        if (audioSource)
            audioSource.Play();
    }
    
    private void SpawnMuzzleFlash()
    {
        if (muzzleFlashPrefab)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, barrelLocation.position,  barrelLocation.rotation);
            Destroy(flash, 0.1f);
        }
    }

    private void EjectCase()
    {
        if (casePrefab)
        {
            GameObject shell = Instantiate(casePrefab, shellLocation.position,  shellLocation.rotation);
            shell.GetComponent<Rigidbody>().AddForce(shellLocation.up * _caseEjectPower);
            Destroy(shell, 5f);
        }
    }
    
    // Rewrite this, split function responsability
    public void CheckPressingTrigger(float triggerValue)
    {
        
        _gunAnimator.SetFloat(_pressTriggerName, triggerValue);
        
        if (triggerValue >= _valueToFire && _isNextShotReady)
        {
            _gunAnimator.SetTrigger(_fireName);
            Shoot();
            _isNextShotReady = false;
        }

        if (triggerValue < _valueToFire && !_isNextShotReady) // Check release trigger
        {
            _isNextShotReady = true;
        }
    }

    public bool Reload(int ammoCount)
    {
        throw new NotImplementedException();
    }
    
}
