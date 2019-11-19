﻿// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PurpleMuffin.Data.Events
{
    [ExecuteInEditMode]
    public partial class GameEventListener
    {
        /// <summary>
        ///     The previously assigned game event.
        /// </summary>
        private GameEvent _previousGameEvent;

        private void Update()
        {
            if(Events.Count > 0 && Events[0] != _previousGameEvent)
            {
                _previousGameEvent = Events[0];

                if(_previousGameEvent != null) name = Events[0].name;
            }
        }

        /// <summary>
        ///     Sets the game object name to the event name.
        /// </summary>
        /// <param name="command"></param>
        [MenuItem("CONTEXT/GameEventListener/Inherit Event Name")]
        public static void InheritEventName(MenuCommand command)
        {
            List<GameEvent> listeners = ((GameEventListener)command.context).Events;
            GameEvent       @event    = listeners.Count > 0 ? listeners[0] : null;

            if(@event != null) command.context.name = @event.name;
        }

        /// <summary>
        ///     Sets the game object name to the event name.
        /// </summary>
        [MenuItem("GameObject/Purple Muffin/Inherit Event Name", false, 10)]
        public static void InheritEventNameHierarchy()
        {
            if(Selection.activeGameObject != null)
            {
                GameEventListener eventListener = Selection.activeGameObject.GetComponent<GameEventListener>();

                if(eventListener != null)
                {
                    List<GameEvent> listeners = eventListener.Events;
                    GameEvent       @event    = listeners.Count > 0 ? listeners[0] : null;

                    if(@event != null) Selection.activeGameObject.name = @event.name;
                }
            }
        }
    }
}
#endif