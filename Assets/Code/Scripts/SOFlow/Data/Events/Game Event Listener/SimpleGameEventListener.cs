// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using System.Collections.Generic;
using SOFlow.Utilities;
using UnityEngine;

namespace SOFlow.Data.Events
{
    public class SimpleGameEventListener : MonoBehaviour, IEventListener
    {
        /// <summary>
        /// The game event to listen for.
        /// </summary>
        public GameEvent Event;

        /// <summary>
        /// The response to the game event.
        /// </summary>
        public DynamicEvent Response;

        /// <summary>
        ///     The game object reference.
        /// </summary>
        private GameObject _gameObjectReference;
        
        /// <summary>
        /// The event list cache.
        /// </summary>
        private readonly List<GameEvent> _eventListCache = new List<GameEvent>();

        /// <inheritdoc />
        public void OnEventRaised(SOFlowDynamic value, GameEvent raisedEvent)
        {
            Response.Invoke(value);
        }

        /// <inheritdoc />
        public GameObject GetGameObject()
        {
            return _gameObjectReference;
        }

        /// <inheritdoc />
        public Type GetObjectType()
        {
            return GetType();
        }

        /// <inheritdoc />
        public List<GameEvent> GetEvents()
        {
            return _eventListCache;
        }

        private void Awake()
        {
            _gameObjectReference = gameObject;
            _eventListCache.Add(Event);
        }

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.DeregisterListener(this);
        }
    }
}