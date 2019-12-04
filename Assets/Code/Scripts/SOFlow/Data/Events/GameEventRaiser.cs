// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections;
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
        /// A cached version of the delay used before executing the event.
        /// </summary>
        private WaitForSeconds _cachedEventDelay;

        /// <summary>
        /// Keeps track of the previous delay time to determine when _cachedEventDelay needs to be updated.
        /// </summary>
        private float _previousDelayTime = -1f;

        private void Start()
        {
            if(RaiseOnStart) RaiseEvent();
        }

        /// <summary>
        ///     Raises this event after the specified period of time has passed.
        /// </summary>
        public void RaiseEvent()
        {
            StartCoroutine(nameof(RaiseEventOverTime));
        }

        private IEnumerator RaiseEventOverTime()
        {
            if(EventWaitTime > 0f)
            {
                if(Mathf.Abs(EventWaitTime - _previousDelayTime) > Mathf.Epsilon)
                {
                    _cachedEventDelay  = new WaitForSeconds(EventWaitTime);
                    _previousDelayTime = EventWaitTime;
                }

                yield return _cachedEventDelay;
            }

            Event.Invoke(null);
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