// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UltEvents;
using UnityEngine;

namespace SOFlow.Data.Events
{
    public class PhysicsGameEventReactor : MonoBehaviour
    {
        /// <summary>
        ///     Enable to listen for OnCollisionEnter events.
        /// </summary>
        public bool ListenForCollisionEnter;

        /// <summary>
        ///     Enable to listen for OnCollisionExit events.
        /// </summary>
        public bool ListenForCollisionExit;

        /// <summary>
        ///     Enable to listen for OnCollisionStay events.
        /// </summary>
        public bool ListenForCollisionStay;

        /// <summary>
        ///     Enable to listen for OnTriggerEnter events.
        /// </summary>
        public bool ListenForTriggerEnter;

        /// <summary>
        ///     Enable to listen for OnTriggerExit events.
        /// </summary>
        public bool ListenForTriggerExit;

        /// <summary>
        ///     Enable to listen for OnTriggerStay events.
        /// </summary>
        public bool ListenForTriggerStay;

        /// <summary>
        ///     The OnCollisionEnter game event.
        /// </summary>
        public UltEvent OnCollisionEnterEvent;

        /// <summary>
        ///     The OnCollisionExit game event.
        /// </summary>
        public UltEvent OnCollisionExitEvent;

        /// <summary>
        ///     The OnCollisionStay game event.
        /// </summary>
        public UltEvent OnCollisionStayEvent;

        /// <summary>
        ///     The OnTriggerEnter game event.
        /// </summary>
        public UltEvent OnTriggerEnterEvent;

        /// <summary>
        ///     The OnTriggerExit game event.
        /// </summary>
        public UltEvent OnTriggerExitEvent;

        /// <summary>
        ///     The OnTriggerStay game event.
        /// </summary>
        public UltEvent OnTriggerStayEvent;

        private void OnTriggerEnter(Collider other)
        {
            if(ListenForTriggerEnter) OnTriggerEnterEvent.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if(ListenForTriggerStay) OnTriggerStayEvent.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if(ListenForTriggerExit) OnTriggerExitEvent.Invoke();
        }

        private void OnCollisionEnter(Collision other)
        {
            if(ListenForCollisionEnter) OnCollisionEnterEvent.Invoke();
        }

        private void OnCollisionStay(Collision other)
        {
            if(ListenForCollisionStay) OnCollisionStayEvent.Invoke();
        }

        private void OnCollisionExit(Collision other)
        {
            if(ListenForCollisionExit) OnCollisionExitEvent.Invoke();
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Adds a Physics Game Event Reactor to the scene.
        /// </summary>
        [UnityEditor.MenuItem("GameObject/SOFlow/Events/Add Physics Game Event Reactor", false, 10)]
        public static void AddComponentToScene()
        {
            if(UnityEditor.Selection.activeGameObject?.GetComponent<Collider>() != null)
            {
                UnityEditor.Selection.activeGameObject.AddComponent<PhysicsGameEventReactor>();

                return;
            }

            GameObject _gameObject = new GameObject("Physics Game Event Reactor", typeof(PhysicsGameEventReactor));

            if(UnityEditor.Selection.activeTransform != null)
            {
                _gameObject.transform.SetParent(UnityEditor.Selection.activeTransform);
            }

            UnityEditor.Selection.activeGameObject = _gameObject;
        }
#endif
    }
}