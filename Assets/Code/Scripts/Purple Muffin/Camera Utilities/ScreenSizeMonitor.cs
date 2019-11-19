﻿// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace PurpleMuffin.CameraUtilities
{
    public class ScreenSizeMonitor : MonoBehaviour
    {
        /// <summary>
        ///     The previous screen size.
        /// </summary>
        private Vector2Int _previousScreenSize;

        /// <summary>
        ///     The world resolution.
        /// </summary>
        private Vector2 _worldResolution;

        /// <summary>
        ///     The game camera reference.
        /// </summary>
        public CameraReference GameCamera;

        /// <summary>
        ///     The resolution state.
        /// </summary>
        public ResolutionState ResolutionState;

        private void Update()
        {
            CalculateWorldSize(GameCamera.Camera.orthographicSize, GameCamera.Camera.pixelRect);
        }

        /// <summary>
        ///     Calculates the world size.
        /// </summary>
        /// <param name="orthographicSize"></param>
        /// <param name="cameraPixelRect"></param>
        public void CalculateWorldSize(float orthographicSize, Rect cameraPixelRect)
        {
            int screenWidth  = (int)cameraPixelRect.width;
            int screenHeight = (int)cameraPixelRect.height;

            if(screenWidth != _previousScreenSize.x && screenHeight != _previousScreenSize.y)
            {
                _previousScreenSize.x = screenWidth;
                _previousScreenSize.y = screenHeight;

                ResolutionState.CurrentScreenResolution = _previousScreenSize;
            }
        }
    }
}