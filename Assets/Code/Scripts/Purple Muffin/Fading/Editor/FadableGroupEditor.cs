// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using PurpleMuffin.Internal;
using UnityEditor;

namespace PurpleMuffin.Fading
{
    [CustomEditor(typeof(FadableGroup))]
    public class FadableGroupEditor : PMCustomEditor
    {
	    /// <summary>
	    ///     The FadableGroup target.
	    /// </summary>
	    private FadableGroup _target;

        private void OnEnable()
        {
            _target = (FadableGroup)target;
        }

        /// <inheritdoc />
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawTertiaryLayer(() =>
                                                {
                                                    if(PMEditorUtilities.DrawColourButton("Capture Child Fadables",
                                                                                          PMEditorSettings
                                                                                             .AcceptContextColour))
                                                        _target.CaptureChildFadables();
                                                });

            PMEditorUtilities.DrawLayeredProperties(serializedObject);
        }
    }
}
#endif