﻿// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace PurpleMuffin.CameraUtilities
{
    [CreateAssetMenu(menuName = "PurpleMuffin/Utilities/Camera Reference")]
    public class CameraReference : ScriptableObject
    {
        /// <summary>
        ///     The camera reference.
        /// </summary>
        public Camera Camera;

        /// <summary>
        ///     Sets the camera reference.
        /// </summary>
        /// <param name="camera"></param>
        public void SetCameraReference(Camera camera)
        {
            Camera = camera;
        }
    }
}