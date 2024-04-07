using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace WeaponSystem.Scripts.Base.Core
{
    public class SlideGrabInteractable : XRGrabInteractable
    {
        public float threshold = 0.02f;
        public Transform target;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public Animator gunAnimator;

        private bool _wasReached = false;
        private float _startPosition;
        private float _endPosition;
        private float _startDistance;
        private float _reachedValue;
        private float _distance;
    
        private Vector3 initialAttachLocalPos;
        private Quaternion initialAttachLocalRot;

        // Start is called before the first frame update
        void Start()
        {
            _startDistance = Vector3.Distance(transform.position, target.position);
        
            CreateAttachPoint();
        }

        private void CreateAttachPoint()
        {
            if(!attachTransform)
            {
                GameObject grab = new GameObject("Grab Pivot");
                grab.transform.SetParent(transform, false);
                attachTransform = grab.transform;
            }

            initialAttachLocalPos = attachTransform.localPosition;
            initialAttachLocalRot = attachTransform.localRotation;
        }

        private void Update()
        {
            _distance = Vector3.Distance(transform.position, target.position);
            _reachedValue = Mathf.InverseLerp(_startDistance, threshold, _distance);
            gunAnimator.SetFloat("MoveSlider", _reachedValue);
        
            if (_distance < threshold && !_wasReached)
            {
                onEnter?.Invoke();
                _wasReached = true;
            } 
            else if (_distance >= threshold && _wasReached)
            {
                onExit?.Invoke();
                _wasReached = false;
            }
        }

        protected override void OnSelectEntered(XRBaseInteractor interactor)
        {
            if(interactor is XRDirectInteractor)
            {
                attachTransform.position = interactor.transform.position;
                attachTransform.rotation = interactor.transform.rotation;
            }
            else
            {
                attachTransform.localPosition = initialAttachLocalPos;
                attachTransform.localRotation = initialAttachLocalRot;
            }

            base.OnSelectEntered(interactor);
        }
    }
}
