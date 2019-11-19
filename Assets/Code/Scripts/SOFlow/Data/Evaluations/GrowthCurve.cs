// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.Data.Primitives;
using UnityEngine;

namespace SOFlow.Data.Evaluations
{
    public class GrowthCurve : MonoBehaviour
    {
        /// <summary>
        ///     The growth curve.
        /// </summary>
        public AnimationCurve Curve = new AnimationCurve();

        /// <summary>
        ///     The value to feed into the curve.
        /// </summary>
        public FloatField CurveValue;

        /// <summary>
        ///     The resulting value after the growth curve has been applied.
        /// </summary>
        public FloatField EvaluatedValue;

        /// <summary>
        ///     The multiplier to apply to the growth curve.
        /// </summary>
        public float GrowthMultiplier = 1f;

        /// <summary>
        ///     Enable to evaluate the growth curve every editor update.
        /// </summary>
        [HideInInspector]
        public bool LiveEvaluation;

        /// <summary>
        ///     Evaluates the curve value on the growth curve and updates the evaluated value accordingly.
        /// </summary>
        public float Evaluate()
        {
            EvaluatedValue.Value = Curve.Evaluate(CurveValue.Value) * GrowthMultiplier;

            return EvaluatedValue.Value;
        }
    }
}