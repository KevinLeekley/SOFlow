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
        /// Indicates whether the Game Manager should initialize automatically when Awake is called.
        /// </summary>
        public BoolField AutoInitializeOnAwake = new BoolField
                                                 {
                                                     Value = true
                                                 };
    
        /// <summary>
        ///     The Game Manager stopwatch.
        /// </summary>
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        ///     Indicates whether the game is currently active.
        /// </summary>
        public BoolField GameActive = new BoolField();

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
            if(AutoInitializeOnAwake)
            {
                InitializeManagers();
            }
            
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

#if UNITY_EDITOR
        /// <summary>
        ///     Adds a Game Manager to the scene.
        /// </summary>
        [UnityEditor.MenuItem("GameObject/SOFlow/Managed Components/Managers/Add Game Manager", false, 10)]
        public static void AddComponentToScene()
        {
            GameObject _gameObject = new GameObject("Game Manager", typeof(GameManager));

            if(UnityEditor.Selection.activeTransform != null)
            {
                _gameObject.transform.SetParent(UnityEditor.Selection.activeTransform);
            }

            UnityEditor.Selection.activeGameObject = _gameObject;
        }
#endif
    }
}