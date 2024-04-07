using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem.Scripts.Base.Core
{
    public class CheckSlideMovement : MonoBehaviour
    {
        [SerializeField] private GunAnimator gunAnimator;
        public float threshold = 0.02f;
        public Transform target;
        public UnityEvent onOpen;
        public UnityEvent onClose;
    
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

            if (IsSlideOpen(distance))
            {
                onOpen?.Invoke();
                _wasReached = true;
            } 
            else if (IsSlideClose(distance))
            {
                onClose?.Invoke();
                _wasReached = false;
            }
        }

        private bool IsSlideOpen(float distance)
        {
            return distance < threshold && !_wasReached;
        }
    
        private bool IsSlideClose(float distance)
        {
            return _startDistance - distance < threshold && _wasReached;
        }
    }
}
