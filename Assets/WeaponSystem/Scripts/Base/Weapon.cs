using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class Weapon: XRGrabInteractable, IWeaponActions
{
    [Header("Weapon Parts")]
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform shellLocation;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private GameObject casePrefab;
    [SerializeField] private XRBaseInteractor magazineSocket;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip insertMagazineSound;
    [SerializeField] private AudioClip pullOutMagazineSound;
    [SerializeField] private AudioClip noAmmoSound;
    [SerializeField] private AudioClip slideOpenSound;
    [SerializeField] private AudioClip slideCloseSound;
    
    [Header("Weapon Settings")]
    [SerializeField] private int shotPower = 5000;
    [SerializeField] private int caseEjectPower = 100;
    [SerializeField] private bool autoFire = false;
    [SerializeField] private int fireRate = 10;
    
    
    public long ID;
    
    private IMagazine _magazine;
    private float _valueToFire = 0.9f;
    private bool _canEjectCase = true;
    private bool _isSlideCock = false;
    private IGunAnimator _gunAnimator;
    private bool _isOneClickMade = true;
    private Recoil _recoilSystem;

    private void Start()
    {
        _gunAnimator = gameObject.GetComponent<IGunAnimator>();
        _recoilSystem = gameObject.GetComponent<Recoil>();
        
        magazineSocket.onSelectEntered.AddListener(AddMagazine);
        magazineSocket.onSelectExited.AddListener(RemoveMagazine);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected) 
                if (firstInteractorSelecting is XRDirectInteractor interactor)
                {
                    InteractionState activateState = interactor.xrController.activateInteractionState;
                    CheckPressingTrigger(activateState.value);
                    
                    // rewrite
                    if (_recoilSystem)
                        _recoilSystem.InvokeReturn(interactor.transform);
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
            InvokeFireAnimation();
            SpawnMuzzleFlash();
            PlaySound(shootSound);
            CreateBullet();
            EjectCase();
            
            // rewrite
            if (_recoilSystem)
                _recoilSystem.RecoilFire();
            
            _magazine.SubstractOneBullet();
        }
        else
        {
            PlaySound(noAmmoSound);
        }
    }

    IEnumerator AutoFire()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(1f / fireRate);
            
        }
    }

    private void InvokeFireAnimation()
    {
        _gunAnimator.InvokeFire();
    }

    private void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
        Destroy(bullet, 5f);
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
            shell.GetComponent<Rigidbody>().AddForce(shellLocation.up * caseEjectPower);
            Destroy(shell, 5f);
        }
    }

    private void UpdateTriggerValueAnimation(float triggerValue)
    {
        if (_gunAnimator != null)
            _gunAnimator.SetTriggerValue(triggerValue);
    }
    

    public void CheckPressingTrigger(float triggerValue)
    {
        UpdateTriggerValueAnimation(triggerValue);
        if (IsTriggerPressed(triggerValue) && _isOneClickMade)
        {
            if (autoFire)
            {
                StartCoroutine(AutoFire()); 
            }
            else
            {
                Shoot();
            }
            _isOneClickMade = false;
        }
        if (IsTriggerReleased(triggerValue) && !_isOneClickMade)
        {
            StopAllCoroutines();
            _isOneClickMade = true;
        }
    }

    private bool IsTriggerPressed(float triggerValue)
    {
        return triggerValue >= _valueToFire;
    }
    
    private bool IsTriggerReleased(float triggerValue)
    {
        return triggerValue < _valueToFire;
    }

    public void AddMagazine(XRBaseInteractable interactable)
    {
        _magazine = interactable.GetComponent<IMagazine>();
        PlaySound(insertMagazineSound);
        
        _isSlideCock = false; // If add new magazine, need to reload slide
    }
    
    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        _magazine = null;
        PlaySound(pullOutMagazineSound);
    }
    
    public void OnSlideOpen()
    {
        _isSlideCock = true;
        PlaySound(slideOpenSound);
    }

    public void OnSlideClose()
    {
        PlaySound(slideCloseSound);
    }
}
