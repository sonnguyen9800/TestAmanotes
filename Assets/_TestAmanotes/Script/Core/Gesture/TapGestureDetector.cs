using System;
using DigitalRubyShared;
using UnityEngine;
using UnityEngine.Events;

namespace FAW.Core
{
    public class TapGestureDetector : MonoBehaviour
    {
        [Tooltip("On Tap Gesture")]
        public UnityEvent OnTapGesture = null;
        private Camera _mainCamera = null;

        public TapGestureRecognizer TapGesture { get; private set; }

        public void OnEnable()
        {
            if (FingersScript.Instance != null)
            {
                FingersScript.Instance.AddGesture(TapGesture);
            }
        }

        private void OnDisable()
        {
            if (FingersScript.Instance != null)
            {
                FingersScript.Instance.RemoveGesture(TapGesture);
            }
        }

        private void Awake()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            TapGesture = new TapGestureRecognizer();
            
            TapGesture.StateUpdated += TapGestureUpdated;
            TapGesture.PlatformSpecificView = gameObject;
        }
        private void TapGestureUpdated(GestureRecognizer gesture)
        {
            switch (gesture.State)
            {
                case GestureRecognizerState.Ended:
                    OnTapGesture?.Invoke();
                    break;
            }
        }

    }
}