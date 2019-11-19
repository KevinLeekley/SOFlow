// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace PurpleMuffin.Data.Events
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
        public DynamicEvent OnCollisionEnterEvent;

        /// <summary>
        ///     The OnCollisionExit game event.
        /// </summary>
        public DynamicEvent OnCollisionExitEvent;

        /// <summary>
        ///     The OnCollisionStay game event.
        /// </summary>
        public DynamicEvent OnCollisionStayEvent;

        /// <summary>
        ///     The OnTriggerEnter game event.
        /// </summary>
        public DynamicEvent OnTriggerEnterEvent;

        /// <summary>
        ///     The OnTriggerExit game event.
        /// </summary>
        public DynamicEvent OnTriggerExitEvent;

        /// <summary>
        ///     The OnTriggerStay game event.
        /// </summary>
        public DynamicEvent OnTriggerStayEvent;

        private void OnTriggerEnter(Collider other)
        {
            if(ListenForTriggerEnter) OnTriggerEnterEvent.Invoke(null);
        }

        private void OnTriggerStay(Collider other)
        {
            if(ListenForTriggerStay) OnTriggerStayEvent.Invoke(null);
        }

        private void OnTriggerExit(Collider other)
        {
            if(ListenForTriggerExit) OnTriggerExitEvent.Invoke(null);
        }

        private void OnCollisionEnter(Collision other)
        {
            if(ListenForCollisionEnter) OnCollisionEnterEvent.Invoke(null);
        }

        private void OnCollisionStay(Collision other)
        {
            if(ListenForCollisionStay) OnCollisionStayEvent.Invoke(null);
        }

        private void OnCollisionExit(Collision other)
        {
            if(ListenForCollisionExit) OnCollisionExitEvent.Invoke(null);
        }
    }
}