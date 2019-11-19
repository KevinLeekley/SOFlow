// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections;
using PurpleMuffin.ManagedComponents.Managers;
using UnityEngine;

namespace PurpleMuffin.ManagedComponents.Components
{
    public class BehaviourComponent : MonoBehaviour
    {
	    /// <summary>
	    ///     The manager for this component.
	    /// </summary>
	    public ManagerBase Manager;

        protected virtual void Awake()
        {
            StartCoroutine(nameof(RegisterToManager));
        }

        /// <summary>
        ///     Registers this component to its associated manager.
        /// </summary>
        public virtual IEnumerator RegisterToManager()
        {
            if(Manager)
            {
                yield return new WaitUntil(() => Manager.Initialized);

                Manager.RegisterEntity(this);
            }
        }

        /// <summary>
        ///     Updates this behaviour component.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="time"></param>
        public virtual void UpdateBehaviour(float deltaTime, float time)
        {
        }
    }
}