// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using PurpleMuffin.Data.Primitives;
using PurpleMuffin.Internal;
using UnityEngine;

namespace PurpleMuffin.Motion
{
    public class BasicMotion : MonoBehaviour
    {
	    /// <summary>
	    ///     The final velocity applied to the target.
	    /// </summary>
	    private Vector3 _appliedVelocity = Vector3.zero;

	    /// <summary>
	    ///     Indicates whether the motion is currently active.
	    /// </summary>
	    private bool _motionActive;

	    /// <summary>
	    ///     The motion active time.
	    /// </summary>
	    private float _motionActiveTime;

	    /// <summary>
	    ///     The motion dropoff duration.
	    /// </summary>
	    private float _motionDropoffDuration;

	    /// <summary>
	    ///     Indicates whether motion dropoff can be performed.
	    /// </summary>
	    private bool _motionDropoffReady;

	    /// <summary>
	    ///     The previous invert state.
	    /// </summary>
	    private bool _previousInvertState;

	    /// <summary>
	    ///     The previous target.
	    /// </summary>
	    private Transform _previousTarget;

	    /// <summary>
	    ///     The active duration time.
	    /// </summary>
	    public FloatField ActiveDuration;

	    /// <summary>
	    ///     The forward velocity.
	    /// </summary>
	    public AnimationCurve ForwardVelocity;

	    /// <summary>
	    ///     The horizontal velocity.
	    /// </summary>
	    public AnimationCurve HorizontalVelocity;

	    /// <summary>
	    ///     Enable to invert forward velocity.
	    /// </summary>
	    public bool InvertForwardVelocity;

	    /// <summary>
	    ///     Enable to invert horizontal velocity.
	    /// </summary>
	    public bool InvertHorizontalVelocity;

	    /// <summary>
	    ///     Enable to invert vertical velocity.
	    /// </summary>
	    public bool InvertVerticalVelocity;

	    /// <summary>
	    ///     Enable to keep motion active for a set duration after initial activation.
	    ///     Useful for creating simple jumping mechanics.
	    /// </summary>
	    [Info("Enable to keep motion active for a set duration after initial activation.\n\n" +
              "Useful for creating simple jumping mechanics.")]
        public bool StayActiveForSetDuration;

	    /// <summary>
	    ///     Enable to use forward velocity during motion.
	    /// </summary>
	    public bool UseForwardVelocity;

	    /// <summary>
	    ///     Enable to use horizontal velocity.
	    /// </summary>
	    public bool UseHorizontalVelocity;

	    /// <summary>
	    ///     Enable to use local position instead of world position.
	    ///     Local position has better performance than world position.
	    /// </summary>
	    [Info("Enable to use local position instead of world position.\n\n" +
              "Local position has better performance than world position.")]
        public bool UseLocalPosition;

	    /// <summary>
	    ///     Enable to use vertical velocity.
	    /// </summary>
	    public bool UseVerticalVelocity;

	    /// <summary>
	    ///     The velocity multiplier.
	    /// </summary>
	    public FloatField VelocityMultiplier = new FloatField
                                               {
                                                   Value = 1f
                                               };

	    /// <summary>
	    ///     The vertical velocity.
	    /// </summary>
	    public AnimationCurve VerticalVelocity;

        private void Update()
        {
            if(_motionActive)
            {
                if(!_motionDropoffReady)
                {
                    _motionDropoffReady    =  true;
                    _motionActiveTime      += Time.deltaTime;
                    _motionDropoffDuration =  _motionActiveTime + ActiveDuration;

                    return;
                }

                if(StayActiveForSetDuration)
                {
                    if(_motionActiveTime < _motionDropoffDuration)
                    {
                        _motionActiveTime += Time.deltaTime;

                        Move(_previousTarget, _previousInvertState);
                    }
                    else
                    {
                        _motionActive     = false;
                        _motionActiveTime = 0f;
                    }
                }
                else
                {
                    _motionActive     = false;
                    _motionActiveTime = 0f;
                }
            }
        }

        /// <summary>
        ///     Moves the given target using the motion configuration.
        /// </summary>
        /// <param name="target"></param>
        public void MoveTarget(Transform target)
        {
            _motionDropoffReady = false;
            Move(target, false);
        }

        /// <summary>
        ///     Moves the given target using the inverted motion configuration.
        /// </summary>
        /// <param name="target"></param>
        public void InvertMoveTarget(Transform target)
        {
            _motionDropoffReady = false;
            Move(target, true);
        }

        /// <summary>
        ///     Moves the target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="invert"></param>
        private void Move(Transform target, bool invert)
        {
            _previousTarget      = target;
            _previousInvertState = invert;

            _motionActive = true;

            if(UseHorizontalVelocity)
            {
                _appliedVelocity.x =
                    HorizontalVelocity.Evaluate(Mathf.Clamp01(_motionActiveTime)) * (invert ? -1f : 1f);

                if(InvertHorizontalVelocity) _appliedVelocity.x *= -1f;
            }
            else
            {
                _appliedVelocity.x = 0;
            }

            if(UseVerticalVelocity)
            {
                _appliedVelocity.y = VerticalVelocity.Evaluate(Mathf.Clamp01(_motionActiveTime)) * (invert ? -1f : 1f);

                if(InvertVerticalVelocity) _appliedVelocity.y *= -1f;
            }
            else
            {
                _appliedVelocity.y = 0;
            }

            if(UseForwardVelocity)
            {
                _appliedVelocity.z = ForwardVelocity.Evaluate(Mathf.Clamp01(_motionActiveTime)) * (invert ? -1f : 1f);

                if(InvertForwardVelocity) _appliedVelocity.z *= -1f;
            }
            else
            {
                _appliedVelocity.z = 0;
            }

            if(UseLocalPosition)
                target.localPosition += VelocityMultiplier * Time.deltaTime * _appliedVelocity;
            else
                target.position += VelocityMultiplier * Time.deltaTime * _appliedVelocity;
        }
    }
}