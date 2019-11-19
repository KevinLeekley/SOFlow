﻿// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace PurpleMuffin.Fading
{
    public class LightFadeable : Fadable
    {
	    /// <summary>
	    ///     The light source to fade.
	    /// </summary>
	    public Light LightSource;

        /// <inheritdoc />
        protected override Color GetColour()
        {
            return LightSource.color;
        }

        /// <inheritdoc />
        public override void UpdateColour(Color colour, float percentage)
        {
            LightSource.color = colour;
        }
    }
}