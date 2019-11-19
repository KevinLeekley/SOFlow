// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UltEvents;
using UnityEngine;

namespace PurpleMuffin.CameraUtilities
{
    [CreateAssetMenu(menuName = "PurpleMuffin/Utilities/Resolution State")]
    public class ResolutionState : ScriptableObject
    {
	    /// <summary>
	    ///     The current screen resolution.
	    /// </summary>
	    private Vector2 _currentScreenResolution;

	    /// <summary>
	    ///     The screen resolution the application was designed for.
	    /// </summary>
	    public Vector2 DesignedScreenResolution;

	    /// <summary>
	    ///     Event raised when the screen resolution changes.
	    /// </summary>
	    public UltEvent OnScreenResolutionChanged;

	    /// <summary>
	    ///     The current screen resolution.
	    /// </summary>
	    public Vector2 CurrentScreenResolution
        {
            get => _currentScreenResolution;
            set
            {
                _currentScreenResolution = value;
                OnScreenResolutionChanged.Invoke();
            }
        }
    }
}