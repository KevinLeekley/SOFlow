// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.Data.Primitives;
using UltEvents;
using UnityEngine;

namespace SOFlow.CameraUtilities
{
    [CreateAssetMenu(menuName = "SOFlow/Utilities/Resolution State")]
    public class ResolutionState : ScriptableObject
    {
	    /// <summary>
	    ///     The screen resolution the application was designed for.
	    /// </summary>
	    public Vector2Field DesignedScreenResolution = new Vector2Field();

	    /// <summary>
	    ///     Event raised when the screen resolution changes.
	    /// </summary>
	    public UltEvent OnScreenResolutionChanged = new UltEvent();

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
	    
	    /// <summary>
	    ///     The current screen resolution.
	    /// </summary>
	    private Vector2 _currentScreenResolution = new Vector2();
    }
}