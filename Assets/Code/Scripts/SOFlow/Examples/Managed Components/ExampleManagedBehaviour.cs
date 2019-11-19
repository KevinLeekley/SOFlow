// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.ManagedComponents.Components;
using UnityEngine;

namespace SOFlow.Examples
{
	/// <summary>
	///     A simple managed Behaviour Component that moves randomly within
	///     given radius. This component demonstrates how Behaviour Components
	///     receive update events from their associated Managers.
	///     Bonus Feature : Behaviour Components can be set up to act in
	///     reverse. This allows for easy implementation of
	///     time-affected mechanics.
	/// </summary>
	public class ExampleManagedBehaviour : BehaviourComponent
    {
	    /// <summary>
	    ///     The current percentage the behaviour has moved towards its target location.
	    /// </summary>
	    private float _currentMovePercentage;

	    /// <summary>
	    ///     The next location the behaviour is moving towards.
	    /// </summary>
	    private Vector3 _nextMoveLocation;

	    /// <summary>
	    ///     The previous location the behaviour was.
	    /// </summary>
	    private Vector3 _previousLocation;

	    /// <summary>
	    ///     Indicates whether the behaviour is ready to select is next location.
	    /// </summary>
	    private bool _readyToSelectMoveLocation = true;

	    /// <summary>
	    ///     The area in which the behaviour will move..
	    /// </summary>
	    public float MoveRadius = 4f;

	    /// <summary>
	    ///     The speed at which the behaviour will move.
	    /// </summary>
	    public float MoveSpeed = 1.5f;

        /// <inheritdoc />
        public override void UpdateBehaviour(float deltaTime, float time)
        {
            base.UpdateBehaviour(deltaTime, time);

            if(_readyToSelectMoveLocation)
            {
                // Compensate for negative game speeds. This allows for the behaviour to act in reverse.
                if(deltaTime < 0f)
                {
                    _nextMoveLocation      = transform.position;
                    _previousLocation      = Random.insideUnitCircle * MoveRadius;
                    _currentMovePercentage = 1f;
                }
                else
                {
                    _nextMoveLocation      = Random.insideUnitCircle * MoveRadius;
                    _previousLocation      = transform.position;
                    _currentMovePercentage = 0f;
                }

                _readyToSelectMoveLocation = false;
            }
            else
            {
                _currentMovePercentage += deltaTime * MoveSpeed;

                transform.position =
                    Vector3.Slerp(_previousLocation, _nextMoveLocation, Mathf.Clamp01(_currentMovePercentage));

                // For game speed variations, ensure we select the next location when we reach either end of the percentage threshold.
                if(_currentMovePercentage > 1f || _currentMovePercentage < 0f) _readyToSelectMoveLocation = true;
            }
        }
    }
}