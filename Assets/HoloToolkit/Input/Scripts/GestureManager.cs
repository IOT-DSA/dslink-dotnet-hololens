// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using Debug = System.Diagnostics.Debug;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// GestureManager creates a gesture recognizer and signs up for a tap gesture.
    /// When a tap gesture is detected, GestureManager uses GazeManager to find the game object.
    /// GestureManager then sends a message to that game object.
    /// </summary>
    [RequireComponent(typeof(GazeManager))]
    public partial class GestureManager : Singleton<GestureManager>
    {
        /// <summary>
        /// Key to press in the editor to select the currently gazed hologram
        /// </summary>
        public KeyCode EditorSelectKey = KeyCode.Space;

        /// <summary>
        /// To select even when a hologram is not being gazed at,
        /// set the override focused object.
        /// If its null, then the gazed at object will be selected.
        /// </summary>
        public GameObject OverrideFocusedObject
        {
            get; set;
        }
        
        public GestureRecognizer GestureRecognizer { get; private set; }
        public GestureRecognizer ManipulationRecognizer { get; private set; }
        public GameObject FocusedObject { get; private set; }
        public GestureRecognizer ActiveRecognizer { get; private set; }
        public bool Manipulating { get; private set; }

        private Vector3 manipulationPreviousPosition;

        private void Start()
        {
            // Create a new GestureRecognizer. Sign up for tapped events.
            GestureRecognizer = new GestureRecognizer();
            GestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
            ActiveRecognizer = GestureRecognizer;

            GestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;
            GestureRecognizer.HoldStartedEvent += GestureRecognizer_OnHoldStartedEvent;
            GestureRecognizer.HoldCompletedEvent += GestureRecognizer_OnHoldCompletedEvent;

            ManipulationRecognizer = new GestureRecognizer();
            ManipulationRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);
            ManipulationRecognizer.ManipulationStartedEvent += GestureRecognizerOnManipulationStartedEvent;
            ManipulationRecognizer.ManipulationUpdatedEvent += GestureRecognizerOnManipulationUpdatedEvent;
            ManipulationRecognizer.ManipulationCompletedEvent += GestureRecognizerOnManipulationCompletedEvent;
            ManipulationRecognizer.ManipulationCanceledEvent += GestureRecognizerOnManipulationCanceledEvent;

            // Start looking for gestures.
            GestureRecognizer.StartCapturingGestures();
        }

        public void Transition(GestureRecognizer newRecognizer)
        {
            if (newRecognizer == null)
            {
                return;
            }

            if (ActiveRecognizer != null)
            {
                if (ActiveRecognizer == newRecognizer)
                {
                    return;
                }

                ActiveRecognizer.CancelGestures();
                ActiveRecognizer.StopCapturingGestures();
            }

            ActiveRecognizer = newRecognizer;
            ActiveRecognizer.StartCapturingGestures();
        }

        private void OnTap()
        {
            FocusedObject?.SendMessage("OnSelect");
        }

        private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            Debug.WriteLine("Tapped event");
            OnTap();
        }

        private void GestureRecognizer_OnHoldStartedEvent(InteractionSourceKind source, Ray headRay)
        {
            Debug.WriteLine("Hold started event");
            FocusedObject?.SendMessage("OnHoldStart");
        }

        private void GestureRecognizer_OnHoldCompletedEvent(InteractionSourceKind source, Ray headRay)
        {
            Debug.WriteLine("Hold stopped event");
            FocusedObject?.SendMessage("OnHoldStop");
        }

        private void GestureRecognizerOnManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            Debug.WriteLine("Manipulation started");
            if (FocusedObject != null)
            {
                Manipulating = true;
                manipulationPreviousPosition = cumulativeDelta;
            }
        }

        private void GestureRecognizerOnManipulationUpdatedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            Debug.WriteLine("Manipulation updated");
            if (FocusedObject != null)
            {
                Manipulating = true;
                Vector3 moveVector = cumulativeDelta - manipulationPreviousPosition;
                manipulationPreviousPosition = cumulativeDelta;
                FocusedObject.transform.position += moveVector;
            }
        }

        private void GestureRecognizerOnManipulationCompletedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            Debug.WriteLine("Manipulation completed");
            Manipulating = false;
        }

        private void GestureRecognizerOnManipulationCanceledEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            Debug.WriteLine("Manipulation canceled");
            Manipulating = false;
        }

        private void LateUpdate()
        {
            GameObject oldFocusedObject = FocusedObject;

            if (GazeManager.Instance.Hit &&
                OverrideFocusedObject == null &&
                GazeManager.Instance.HitInfo.collider != null)
            {
                // If gaze hits a hologram, set the focused object to that game object.
                // Also if the caller has not decided to override the focused object.
                FocusedObject = GazeManager.Instance.HitInfo.collider.gameObject;
            }
            else
            {
                // If our gaze doesn't hit a hologram, set the focused object to null or override focused object.
                FocusedObject = OverrideFocusedObject;
            }

            if (FocusedObject != oldFocusedObject)
            {
                // If the currently focused object doesn't match the old focused object, cancel the current gesture.
                // Start looking for new gestures.  This is to prevent applying gestures from one hologram to another.
                GestureRecognizer.CancelGestures();
                GestureRecognizer.StartCapturingGestures();
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(EditorSelectKey))
            {
                OnTap();
            }
#endif
        }

        void OnDestroy()
        {
            GestureRecognizer.StopCapturingGestures();
            GestureRecognizer.TappedEvent -= GestureRecognizer_TappedEvent;
            GestureRecognizer.HoldStartedEvent -= GestureRecognizer_OnHoldStartedEvent;
            GestureRecognizer.HoldCompletedEvent -= GestureRecognizer_OnHoldCompletedEvent;
            GestureRecognizer.ManipulationStartedEvent -= GestureRecognizerOnManipulationStartedEvent;
            GestureRecognizer.ManipulationUpdatedEvent -= GestureRecognizerOnManipulationUpdatedEvent;
            GestureRecognizer.ManipulationCompletedEvent -= GestureRecognizerOnManipulationCompletedEvent;
            GestureRecognizer.ManipulationCanceledEvent -= GestureRecognizerOnManipulationCanceledEvent;
        }
    }
}
