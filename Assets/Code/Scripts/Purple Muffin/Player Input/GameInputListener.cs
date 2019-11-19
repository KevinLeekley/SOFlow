// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using UnityEngine;

namespace PurpleMuffin.PlayerInput
{
    public class GameInputListener : MonoBehaviour
    {
        /// <summary>
        ///     Enable to automatically check all game input on this listener.
        /// </summary>
        public bool AutoCheckGameInput = true;

        /// <summary>
        ///     The input this component is listening for.
        /// </summary>
        public List<GameInput> Input = new List<GameInput>();

        private void Update()
        {
            if(AutoCheckGameInput) CheckGameInput();
        }

        /// <summary>
        ///     Checks the current state of all game input on this listener.
        /// </summary>
        public void CheckGameInput()
        {
            foreach(GameInput input in Input) input.CheckInputState();
        }
    }
}