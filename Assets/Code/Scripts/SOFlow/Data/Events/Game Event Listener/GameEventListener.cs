// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using System.Collections;
using System.Collections.Generic;
using SOFlow.Extensions;
using SOFlow.Utilities;
using UnityEngine;

namespace SOFlow.Data.Events
{
    public partial class GameEventListener : MonoBehaviour, IEventListener
    {
        /// <summary>
        ///     The game event to listen for.
        /// </summary>
        public List<GameEvent> Events = new List<GameEvent>();

        /// <summary>
        ///     The event listener order.
        /// </summary>
        public float EventListenerOrder;

        /// <summary>
        ///     Indicates whether this listener should be prioritized after other listeners when events are raised.
        /// </summary>
        public bool RegisterLast;

        /// <summary>
        ///     Indicates whether the event listener should register to the given event on Awake.
        /// </summary>
        public bool RegisterOnAwake;

        /// <summary>
        ///     Indicates whether this event should be debugged.
        /// </summary>
        public bool Debug;

        /// <summary>
        ///     The list of conditions for this event.
        /// </summary>
        public List<EventCondition> Conditions = new List<EventCondition>();

        /// <summary>
        ///     The response to the game event.
        /// </summary>
        public DynamicEvent Response;

        /// <summary>
        ///     The game object reference.
        /// </summary>
        private GameObject _gameObjectReference;

        /// <inheritdoc />
        public void OnEventRaised(SOFlowDynamic value, GameEvent raisedEvent)
        {
            bool conditionsMet = true;

            foreach(EventCondition condition in Conditions)
                if(!condition.Evaluate())
                {
                    conditionsMet = false;

                    break;
                }

            if(conditionsMet)
            {
                Response.Invoke(value);

                if(Debug) UnityEngine.Debug.Log($"|Game Event Listener| Responding to event : \n{raisedEvent.name}");
            }
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
            return Events;
        }

        private void Awake()
        {
            _gameObjectReference = gameObject;

            if(!Application.isPlaying) return;

            if(RegisterOnAwake)
                foreach(GameEvent @event in Events)
                {
                    if(@event == null)
                    {
                        UnityEngine.Debug
                                   .LogWarning($"[Game Event Listener Set] No event supplied for listener at : \n{transform.GetPath()}");

                        continue;
                    }

                    @event.RegisterListener(this, RegisterLast);
                }
        }

        private void OnEnable()
        {
            if(!Application.isPlaying) return;

            if(!RegisterOnAwake) StartCoroutine(RegisterEvent());
        }

        private void OnDisable()
        {
            if(!Application.isPlaying) return;

            if(!RegisterOnAwake)
            {
                StopAllCoroutines();

                foreach(GameEvent @event in Events) @event.DeregisterListener(this);
            }
        }

        private void OnDestroy()
        {
            if(!Application.isPlaying) return;

            if(RegisterOnAwake)
            {
                StopAllCoroutines();

                foreach(GameEvent @event in Events) @event.DeregisterListener(this);
            }
        }

        /// <summary>
        ///     Registers the event.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RegisterEvent()
        {
            yield return WaitCache.Get(EventListenerOrder);

            foreach(GameEvent @event in Events)
            {
                if(@event == null)
                {
                    UnityEngine.Debug
                               .LogWarning($"[Game Event Listener Set] No event supplied for listener at : \n{transform.GetPath()}");

                    continue;
                }

                @event.RegisterListener(this, RegisterLast);
            }
        }
    }
}