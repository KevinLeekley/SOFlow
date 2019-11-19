// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.Data.Primitives;
using UnityEngine;

namespace SOFlow.Fading
{
    public class FloatDataFadable : Fadable
    {
	    /// <summary>
	    ///     The fade range.
	    /// </summary>
	    public Vector2 FadeRange;

	    /// <summary>
	    ///     The maximum fade range.
	    /// </summary>
	    public FloatField FadeReferenceRangeMax;

	    /// <summary>
	    ///     The minimum fade range.
	    /// </summary>
	    public FloatField FadeReferenceRangeMin;

	    /// <summary>
	    ///     The float data to fade.
	    /// </summary>
	    public FloatField FloatData;

	    /// <summary>
	    ///     Indicates whether FloatField references should be used for the fade range.
	    /// </summary>
	    public bool UseReferenceRange;

        /// <inheritdoc />
        protected override Color GetColour()
        {
            return default;
        }

        /// <inheritdoc />
        public override void UpdateColour(Color colour, float percentage)
        {
            if(UseReferenceRange)
                FloatData.Value = Mathf.Lerp(FadeReferenceRangeMin.Value, FadeReferenceRangeMax.Value, percentage);
            else
                FloatData.Value = Mathf.Lerp(FadeRange.x, FadeRange.y, percentage);
        }
    }
}