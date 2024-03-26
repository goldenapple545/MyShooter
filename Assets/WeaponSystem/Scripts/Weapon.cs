using System;
using UnityEngine;
using UnityEngine.Serialization;
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
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip insertMagazineSound;
    [SerializeField] private AudioClip pullOutMagazineSound;
    [SerializeField] private AudioClip noAmmoSound;
    [SerializeField] private AudioClip slideOpenSound;
    [SerializeField] private AudioClip slideCloseSound;
    
    public long ID;
    public XRBaseInteractor magazineSocket;
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

    private IMagazine _magazine;
    private string _pressTriggerName = "PressTrigger";
    private string _fireName = "Fire";
    private float _valueToFire = 0.9f;
    private int _shotPower = 5000;
    private int _caseEjectPower = 100;
    private bool _canEjectCase = true;
    private bool _isSlideCock = false;
    private Animator _gunAnimator;
    private bool _isOneClickMade = true;

    private void Start()
    {
        _gunAnimator = gameObject.GetComponent<Animator>();
        
        magazineSocket.onSelectEnter.AddListener(AddMagazine);
        magazineSocket.onSelectExit.AddListener(RemoveMagazine);
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

    public bool IsReadyToFire()
    {
        return _magazine != null && _magazine.GetNumberOfBullets() > 0 && _isSlideCock;
    }

    public void Shoot()
    {
        if (IsReadyToFire())
        {
            _gunAnimator.SetTrigger(_fireName);
            SpawnMuzzleFlash();
            PlaySound(shootSound);
            _magazine.SubstractOneBullet();
            
            GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * _shotPower);
        }
        else
        {
            PlaySound(noAmmoSound);
        }
    }

    private void PlaySound(AudioClip audioClip)
    {
        if (audioSource)
            audioSource.PlayOneShot(audioClip);
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
        
        if (triggerValue >= _valueToFire && _isOneClickMade)
        {
            Shoot();
            _isOneClickMade = false;
        }

        if (triggerValue < _valueToFire && !_isOneClickMade) // Check release trigger
        {
            _isOneClickMade = true;
        }
    }

    public void AddMagazine(XRBaseInteractable interactable)
    {
        _magazine = interactable.GetComponent<IMagazine>();
        PlaySound(insertMagazineSound);
        _isSlideCock = false;
    }
    
    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        _magazine = null;
        PlaySound(pullOutMagazineSound);
    }
    
    public void SlideOpen()
    {
        _isSlideCock = true;
        PlaySound(slideOpenSound);
    }

    public void SlideClose()
    {
        PlaySound(slideCloseSound);
    }
}
