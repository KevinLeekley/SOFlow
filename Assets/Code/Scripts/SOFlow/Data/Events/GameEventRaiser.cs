// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections;
using SOFlow.Utilities;
using UnityEngine;

namespace SOFlow.Data.Events
{
    public class GameEventRaiser : MonoBehaviour
    {
        /// <summary>
        ///     The game event to raise.
        /// </summary>
        public DynamicEvent Event;

        /// <summary>
        ///     The time before the event is raised in seconds.
        /// </summary>
        public float EventWaitTime;

        /// <summary>
        ///     Indicates whether the event should be raised automatically when Start is called.
        /// </summary>
        public bool RaiseOnStart = true;

        /// <summary>
        /// Indicates whether the event should be repeated.
        /// </summary>
        public bool RepeatEvent = false;

        /// <summary>
        /// Indicates whether the event has been scheduled for cancellation.
        /// </summary>
        private bool _cancelEventScheduled = false;

        private void Start()
        {
            if(RaiseOnStart) RaiseEvent();
        }

        /// <summary>
        ///     Raises this event after the specified period of time has passed.
        /// </summary>
        public void RaiseEvent()
        {
            // Nullify any cancellation schedules as a new event has been invoked.
            _cancelEventScheduled = false;
            
            StartCoroutine(nameof(RaiseEventOverTime));
        }

        private IEnumerator RaiseEventOverTime()
        {
            if(EventWaitTime > 0f)
            {
                yield return WaitCache.Get(EventWaitTime);
            }

            if(_cancelEventScheduled)
            {
                _cancelEventScheduled = false;
                
                yield break;
            }

            Event.Invoke(null);

            if(RepeatEvent)
            {
                RaiseEvent();
            }
        }

        /// <summary>
        /// Cancels the event if it has been scheduled.
        /// </summary>
        public void CancelEvent()
        {
            _cancelEventScheduled = true;
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Adds a Game Event Raiser to the scene.
        /// </summary>
        [UnityEditor.MenuItem("GameObject/SOFlow/Events/Add Game Event Raiser", false, 10)]
        public static void AddComponentToScene()
        {
            GameObject _gameObject = new GameObject("Game Event Raiser", typeof(GameEventRaiser));

            if(UnityEditor.Selection.activeTransform != null)
            {
                _gameObject.transform.SetParent(UnityEditor.Selection.activeTransform);
            }

            UnityEditor.Selection.activeGameObject = _gameObject;
        }
#endif
    }
}