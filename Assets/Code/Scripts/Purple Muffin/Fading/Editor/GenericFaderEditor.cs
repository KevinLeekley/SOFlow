// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR

using PurpleMuffin.Internal;
using UnityEditor;

namespace PurpleMuffin.Fading
{
    [CustomEditor(typeof(GenericFader))]
    public class GenericFaderEditor : PMCustomEditor
    {
        /// <summary>
        ///     The GenericFader target.
        /// </summary>
        private GenericFader _target;

        private void OnEnable()
        {
            _target = (GenericFader)target;
        }

        /// <inheritdoc />
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawPrimaryLayer(DrawFaderInspector);
        }

        /// <summary>
        ///     Draws the fader inspector.
        /// </summary>
        private void DrawFaderInspector()
        {
            PMEditorUtilities.DrawSecondaryLayer(() =>
                                                 {
                                                     PMEditorUtilities
                                                        .DrawListComponentProperty(serializedObject,
                                                                                   serializedObject
                                                                                      .FindProperty("FadeTargets"),
                                                                                   PMEditorSettings
                                                                                      .TertiaryLayerColour);
                                                 });

            serializedObject.DrawProperty("UnfadedColour");
            serializedObject.DrawProperty("FadedColour");
            serializedObject.DrawProperty("FadeCurve");

            if(!_target.OnlyFade) serializedObject.DrawProperty("UnfadeCurve");

            serializedObject.DrawProperty("OnlyFade");
            serializedObject.DrawProperty("FadeTime");

            if(!_target.OnlyFade)
            {
                serializedObject.DrawProperty("UnfadeTime");
                serializedObject.DrawProperty("WaitBetweenFades");
            }

            serializedObject.DrawProperty("OnFadeStart");

            if(!_target.OnlyFade) serializedObject.DrawProperty("OnFadeWait");

            serializedObject.DrawProperty("OnFadeComplete");
        }
    }
}
#endif