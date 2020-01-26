// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections;
using SOFlow.ManagedComponents.Managers;
using UnityAsync;
using UnityEngine;
using WaitUntil = UnityEngine.WaitUntil;

namespace SOFlow.ManagedComponents.Components
{
    public class BehaviourComponent : MonoBehaviour
    {
	    /// <summary>
	    ///     The manager for this component.
	    /// </summary>
	    public ManagerBase Manager;

        protected virtual void Awake()
        {
            RegisterToManager();
        }

        /// <summary>
        ///     Registers this component to its associated manager.
        /// </summary>
        public async void RegisterToManager()
        {
            if(Manager)
            {
                await Await.Until(() => Manager.Initialized);

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