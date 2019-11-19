﻿// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using PurpleMuffin.Data.Primitives;
using UnityEngine;

namespace PurpleMuffin.ManagedComponents.Managers
{
    [CreateAssetMenu(menuName = "PurpleMuffin/Managed Components/Managers/Behaviour Component Manager")]
    public class BehaviourComponentManager : ManagerBase
    {
	    /// <summary>
	    ///     Reference to the game active state.
	    /// </summary>
	    public BoolField GameActive;

	    /// <summary>
	    ///     Reference to the game paused state.
	    /// </summary>
	    public BoolField GamePaused;

	    /// <summary>
	    ///     The current game speed.
	    /// </summary>
	    public FloatField GameSpeed;

        /// <inheritdoc />
        public override void EntityUpdateTick(float deltaTime, float time)
        {
            if(!GamePaused.Value && GameActive.Value) base.EntityUpdateTick(deltaTime * GameSpeed, time);
        }
    }
}