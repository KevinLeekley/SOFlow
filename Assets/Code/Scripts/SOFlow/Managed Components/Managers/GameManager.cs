// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using System.Diagnostics;
using SOFlow.Data.Primitives;
using UnityEngine;

namespace SOFlow.ManagedComponents.Managers
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        ///     The Game Manager stopwatch.
        /// </summary>
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        ///     Indicates whether the game is currently active.
        /// </summary>
        public BoolField GameActive;

        /// <summary>
        ///     The list of managers managed by the Game Manager.
        /// </summary>
        public List<ManagerBase> Managers = new List<ManagerBase>();

        /// <summary>
        ///     The manager update rate.
        /// </summary>
        public IntField ManagerUpdateRate = new IntField
                                            {
                                                Value = 1000
                                            };

        private void Awake()
        {
            _stopwatch.Start();
        }

        private void Update()
        {
            if(!GameActive) return;

            if(_stopwatch.ElapsedMilliseconds >= ManagerUpdateRate)
            {
                foreach(ManagerBase manager in Managers) manager.ManagerUpdateTick();

                _stopwatch.Restart();
            }

            float time      = Time.time;
            float deltaTime = Time.deltaTime;

            foreach(ManagerBase manager in Managers) manager.EntityUpdateTick(deltaTime, time);
        }

        /// <summary>
        ///     Initializes the managers.
        /// </summary>
        public void InitializeManagers()
        {
            foreach(ManagerBase manager in Managers) manager.InitializeManager();
        }
    }
}