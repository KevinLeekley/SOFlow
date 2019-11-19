// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEditor;
#if UNITY_EDITOR
using PurpleMuffin.Internal;
using UnityEngine;

namespace PurpleMuffin.Data.Evaluations.Editor
{
    [CustomEditor(typeof(Evaluator))]
    public class EvaluatorEditor : PMCustomEditor
    {
        /// <summary>
        ///     The result text style.
        /// </summary>
        private GUIStyle _resultStyle;

        /// <summary>
        ///     The Evaluator target.
        /// </summary>
        private Evaluator _target;

        /// <summary>
        ///     The evaluation test result.
        /// </summary>
        private bool _testResult;

        private void OnEnable()
        {
            _target = (Evaluator)target;
        }

        /// <inheritdoc />
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   PMEditorUtilities
                                                      .DrawListComponentProperty(serializedObject,
                                                                                 serializedObject
                                                                                    .FindProperty("Comparisons"),
                                                                                 PMEditorSettings.SecondaryLayerColour);

                                                   serializedObject.DrawProperty("Any");
                                               });

            PMEditorUtilities.DrawPrimaryLayer(DrawEvaluationTest);
        }

        /// <summary>
        ///     Draws evaluation testing tools.
        /// </summary>
        private void DrawEvaluationTest()
        {
            Color originalGUIColor = GUI.backgroundColor;

            PMEditorUtilities.DrawColourLayer(_testResult
                                                  ? PMEditorSettings.AcceptContextColour
                                                  : PMEditorSettings.DeclineContextColour,
                                              () =>
                                              {
                                                  if(PMEditorUtilities.DrawColourButton("Test Comparison"))
                                                      _testResult = _target.Evaluate();

                                                  GUI.backgroundColor =
                                                      _testResult
                                                          ? PMEditorSettings.AcceptContextColour
                                                          : PMEditorSettings.DeclineContextColour;

                                                  GUILayout.Label(_testResult.ToString(), PMStyles.BoldCenterLabel);
                                              });

            GUI.backgroundColor = originalGUIColor;
        }
    }
}
#endif