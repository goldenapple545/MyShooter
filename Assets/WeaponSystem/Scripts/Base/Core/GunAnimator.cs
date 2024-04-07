using UnityEngine;
using WeaponSystem.Scripts.Base.Interfaces;

namespace WeaponSystem.Scripts.Base.Core
{
    public class GunAnimator : MonoBehaviour, IGunAnimator
    {
        public Animator gunAnimator;

        private string _pressTriggerName = "PressTrigger";
        private string _fireName = "Fire";
        private string _moveSliderName = "MoveSlider";
        private void Awake()
        {
            gunAnimator = GetComponent<Animator>();
        }

        public void SetTriggerValue(float triggerValue)
        {
            gunAnimator.SetFloat(_pressTriggerName, triggerValue);
        }

        public void SetSlideValue(float slideValue)
        {
            gunAnimator.SetFloat(_moveSliderName, slideValue);
        }

        public void InvokeFire()
        {
            gunAnimator.SetTrigger(_fireName);
        }
    }
}
