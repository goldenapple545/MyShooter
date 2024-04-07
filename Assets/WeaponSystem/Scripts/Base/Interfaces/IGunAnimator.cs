namespace WeaponSystem.Scripts.Base.Interfaces
{
    public interface IGunAnimator
    {
        void SetTriggerValue(float triggerValue);
        void SetSlideValue(float slideValue);
        void InvokeFire();
    }
}
