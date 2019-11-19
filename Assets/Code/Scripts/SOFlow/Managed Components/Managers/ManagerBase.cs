// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using SOFlow.ManagedComponents.Components;
using UnityEngine;

namespace SOFlow.ManagedComponents.Managers
{
    public abstract class ManagerBase : ScriptableObject
    {
        /// <summary>
        ///     The list of basic entities registered to this manager.
        /// </summary>
        public List<BehaviourComponent> Entities = new List<BehaviourComponent>();

        /// <summary>
        ///     Indicates whether this manager has been initialized.
        /// </summary>
        public bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        ///     Initializes the manager.
        /// </summary>
        public virtual void InitializeManager()
        {
            Entities.Clear();

            Initialized = true;
        }

        /// <summary>
        ///     The manager update tick.
        /// </summary>
        public virtual void ManagerUpdateTick()
        {
        }

        /// <summary>
        ///     The entity update tick.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="time"></param>
        public virtual void EntityUpdateTick(float deltaTime, float time)
        {
            foreach(BehaviourComponent entity in Entities) entity.UpdateBehaviour(deltaTime, time);
        }

        /// <summary>
        ///     Registers the given entity to this manager.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void RegisterEntity(BehaviourComponent entity)
        {
            Entities.Add(entity);
        }

        private void OnEnable()
        {
            Initialized = false;
        }
    }
}