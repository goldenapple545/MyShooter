using UnityEngine.XR.Interaction.Toolkit;

namespace WeaponSystem.Scripts.Base
{
    public class MagazineSocket : XRSocketInteractor
    {
        public string magazineType;

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            return base.CanSelect(interactable) && interactable.colliders[0].CompareTag(magazineType);
        }
    }
}
