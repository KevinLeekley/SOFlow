// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.Data.Primitives;
using SOFlow.Internal;
using UnityEngine;

namespace SOFlow.Motion
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
	    public FloatField ActiveDuration = new FloatField();

	    /// <summary>
	    ///     The forward velocity.
	    /// </summary>
	    public AnimationCurve ForwardVelocity = new AnimationCurve();

	    /// <summary>
	    ///     The horizontal velocity.
	    /// </summary>
	    public AnimationCurve HorizontalVelocity = new AnimationCurve();

	    /// <summary>
	    ///     Enable to invert forward velocity.
	    /// </summary>
	    public BoolField InvertForwardVelocity = new BoolField();

	    /// <summary>
	    ///     Enable to invert horizontal velocity.
	    /// </summary>
	    public BoolField InvertHorizontalVelocity = new BoolField();

	    /// <summary>
	    ///     Enable to invert vertical velocity.
	    /// </summary>
	    public BoolField InvertVerticalVelocity = new BoolField();

	    /// <summary>
	    ///     Enable to keep motion active for a set duration after initial activation.
	    ///     Useful for creating simple jumping mechanics.
	    /// </summary>
        public BoolField StayActiveForSetDuration = new BoolField();

	    /// <summary>
	    ///     Enable to use forward velocity during motion.
	    /// </summary>
	    public BoolField UseForwardVelocity = new BoolField();

	    /// <summary>
	    ///     Enable to use horizontal velocity.
	    /// </summary>
	    public BoolField UseHorizontalVelocity = new BoolField();

	    /// <summary>
	    ///     Enable to use local position instead of world position.
	    ///     Local position has better performance than world position.
	    /// </summary>
        public BoolField UseLocalPosition = new BoolField();

	    /// <summary>
	    ///     Enable to use vertical velocity.
	    /// </summary>
	    public BoolField UseVerticalVelocity = new BoolField();

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
	    public AnimationCurve VerticalVelocity = new AnimationCurve();

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

#if UNITY_EDITOR
        /// <summary>
        ///     Adds a Basic Motion to the scene.
        /// </summary>
        [UnityEditor.MenuItem("GameObject/SOFlow/Motion/Add Basic Motion", false, 10)]
        public static void AddComponentToScene()
        {
	        GameObject _gameObject = new GameObject("Basic Motion", typeof(BasicMotion));

	        if(UnityEditor.Selection.activeTransform != null)
	        {
		        _gameObject.transform.SetParent(UnityEditor.Selection.activeTransform);
	        }

	        UnityEditor.Selection.activeGameObject = _gameObject;
        }
#endif
    }
}