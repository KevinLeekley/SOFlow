// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using PurpleMuffin.Utilities;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using PurpleMuffin.Data.Events.Editor;

#endif

namespace PurpleMuffin.Data.Events
{
    [CreateAssetMenu(menuName = "PurpleMuffin/Events/Game Event")]
    public partial class GameEvent : ScriptableObject
    {
        /// <summary>
        ///     The dynamic parameter for this game event.
        /// </summary>
        public PMDynamic EventParameter = new PMDynamic();

        /// <summary>
        ///     All listeners registered to this game event.
        /// </summary>
        [HideInInspector]
        public List<IEventListener> Listeners = new List<IEventListener>();

        /// <summary>
        ///     Notifies all registered listeners to invoke their events.
        /// </summary>
        public void Raise()
        {
#if UNITY_EDITOR
            GameEventLog log = new GameEventLog(new List<IEventListener>(), DateTime.Now);
            EventStack.Add(log);
#endif

            for(int i = Listeners.Count - 1; i >= 0; i--)
            {
#if UNITY_EDITOR
                log.Listener.Add(Listeners[i]);
#endif
                try
                {
                    Listeners[i].OnEventRaised(EventParameter, this);

#if UNITY_EDITOR
                    log.IsError.Add(false);
#endif
                }
                catch(Exception e)
                {
                    Exception baseException = e.GetBaseException();

#if UNITY_EDITOR
                    log.IsError.Add(true);
                    log.ErrorMessages.Add(baseException.Message);
                    log.StackTraces.Add(baseException.StackTrace);

                    List<GameEventErrorData> errorData  = new List<GameEventErrorData>();
                    StackTrace               stackTrace = new StackTrace(baseException, true);

                    for(int j = 0; j < stackTrace.FrameCount; j++)
                    {
                        StackFrame stackFrame = stackTrace.GetFrame(j);
                        int        lineNumber = stackFrame.GetFileLineNumber();

                        if(lineNumber > 0)
                            errorData.Add(new GameEventErrorData(stackFrame.GetMethod().Name, stackFrame.GetFileName(),
                                                                 stackFrame.GetFileLineNumber()));
                    }

                    log.ErrorData.Add(errorData);
#endif
                    Debug.LogError($"[Game Event Error] {DateTime.Now:T} | {i} | {name} | {(Listeners[i].GetGameObject() == null ? "No Game Object" : Listeners[i].GetGameObject().name)} | {Listeners[i].GetObjectType().Name}");
                    Debug.LogException(baseException);
                }
                finally
                {
#if UNITY_EDITOR
                    GameEventLogWindow.LogEvent(this);
#endif
                }
            }
        }

        /// <summary>
        ///     Notifies all registered listeners to invoke their events.
        /// </summary>
        /// <param name="eventParameter"></param>
        public void Raise(dynamic eventParameter)
        {
            if(eventParameter == null) EventParameter = null;

            {
                EventParameter = new PMDynamic
                                 {
                                     Value = eventParameter
                                 };
            }

            Raise();
        }

        /// <summary>
        ///     Sets the event parameter to the specified value.
        ///     Usable with Unity.Object types only. Usable within the inspector.
        /// </summary>
        /// <param name="value"></param>
        public void SetEventParameterUnity(Object value)
        {
            EventParameter.Value = value;
        }

        /// <summary>
        ///     Sets the event parameter to the specified value.
        ///     Usable with all data types. Unusable within the inspector.
        /// </summary>
        /// <param name="value"></param>
        public void SetEventParameter(object value)
        {
            EventParameter.Value = value;
        }

        /// <summary>
        ///     Registers a listener to this game event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="registerLast"></param>
        public void RegisterListener(IEventListener listener, bool registerLast = false)
        {
            if(registerLast)
                Listeners.Insert(0, listener);
            else
                Listeners.Add(listener);
        }

        /// <summary>
        ///     Deregisters a listener from this game event.
        /// </summary>
        /// <param name="listener"></param>
        public void DeregisterListener(IEventListener listener)
        {
            Listeners.Remove(listener);
        }
    }
}