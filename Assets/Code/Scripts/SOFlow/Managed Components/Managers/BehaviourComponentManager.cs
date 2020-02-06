// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.Data.Primitives;
using UnityEngine;

namespace SOFlow.ManagedComponents.Managers
{
    [CreateAssetMenu(menuName = "SOFlow/Managed Components/Managers/Behaviour Component Manager")]
    public class BehaviourComponentManager : ManagerBase
    {
	    /// <summary>
	    ///     Reference to the game active state.
	    /// </summary>
	    public BoolField GameActive = new BoolField();

	    /// <summary>
	    ///     Reference to the game paused state.
	    /// </summary>
	    public BoolField GamePaused = new BoolField();

	    /// <summary>
	    ///     The current game speed.
	    /// </summary>
	    public FloatField GameSpeed = new FloatField();

        /// <inheritdoc />
        public override void EntityUpdateTick(float deltaTime, float time)
        {
            if(!GamePaused.Value && GameActive.Value) base.EntityUpdateTick(deltaTime * GameSpeed, time);
        }
    }
}