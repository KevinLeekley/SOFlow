// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using UnityEngine;

namespace SOFlow.Data.Events
{
    public class GameEventReactorSet : MonoBehaviour
    {
        /// <summary>
        ///     The player events to listen for.
        /// </summary>
        public List<GameEventReactor> Events = new List<GameEventReactor>();

        private void Awake()
        {
            foreach(GameEventReactor @event in Events) @event.ActivateReactor(gameObject);
        }

        private void OnDestroy()
        {
            foreach(GameEventReactor @event in Events) @event.DeactivateReactor();
        }
    }
}