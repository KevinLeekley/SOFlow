// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using SOFlow.Data.Primitives;
using UnityEngine;

namespace SOFlow.ManagedComponents.Components
{
    public class SimpleMotionComponent : BehaviourComponent
    {
	    /// <summary>
	    ///     The currently applied motion velocity, affected by the speed modifier.
	    /// </summary>
	    private Vector3 _appliedMotionVelocity;

	    /// <summary>
	    ///     The previous speed modifier.
	    /// </summary>
	    private float _previousSpeedModifier;

	    /// <summary>
	    ///     The motion target.
	    /// </summary>
	    public Transform MotionTarget;

	    /// <summary>
	    ///     The motion velocity.
	    /// </summary>
	    public Vector3Field MotionVelocity;

	    /// <summary>
	    ///     The motion speed modifier.
	    /// </summary>
	    public FloatField SpeedModifier;

        /// <inheritdoc />
        public override void UpdateBehaviour(float deltaTime, float time)
        {
            base.UpdateBehaviour(deltaTime, time);

            UpdateAppliedVelocity();

            MotionTarget.localPosition += _appliedMotionVelocity * deltaTime;
        }

        /// <summary>
        ///     Updates the applied motion velocity.
        /// </summary>
        private void UpdateAppliedVelocity()
        {
            if(Math.Abs(SpeedModifier - _previousSpeedModifier) > Mathf.Epsilon)
            {
                _previousSpeedModifier = SpeedModifier;
                _appliedMotionVelocity = MotionVelocity.Value * _previousSpeedModifier;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Adds a Simple Motion Component to the scene.
        /// </summary>
        [UnityEditor.MenuItem("GameObject/SOFlow/Managed Components/Components/Add Simple Motion Component", false, 10)]
        public static void AddComponentToScene()
        {
	        GameObject _gameObject = new GameObject("Simple Motion Component", typeof(SimpleMotionComponent));

	        if(UnityEditor.Selection.activeTransform != null)
	        {
		        _gameObject.transform.SetParent(UnityEditor.Selection.activeTransform);
	        }

	        UnityEditor.Selection.activeGameObject = _gameObject;
        }
#endif
    }
}