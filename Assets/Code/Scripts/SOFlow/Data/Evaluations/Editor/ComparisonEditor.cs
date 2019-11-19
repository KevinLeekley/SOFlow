// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using SOFlow.Internal;
using UnityEngine;
using UnityEditor;

namespace SOFlow.Data.Evaluations.Editor
{
    [CustomEditor(typeof(Comparison))]
    public class ComparisonEditor : SOFlowCustomEditor
    {
        /// <summary>
        ///     The minimum width for each field.
        /// </summary>
        private const float _minimumFieldWidth = 150f;

        /// <summary>
        ///     The width of each text character.
        /// </summary>
        private const float _characterWidth = 14f;

        // The comparison type properties.
        private SerializedProperty _comparisonType;
        private SerializedProperty _dataOperator;

        // The fields to evaluate.
        private SerializedProperty _firstField;
        private SerializedProperty _OnComparisonFail;
        private SerializedProperty _OnComparisonSuccess;
        private SerializedProperty _primitiveOperator;
        private SerializedProperty _respondInEditor;

        /// <summary>
        ///     The result text style.
        /// </summary>
        private GUIStyle _resultStyle;

        private SerializedProperty _secondField;

        /// <summary>
        ///     The Comparison target.
        /// </summary>
        private Comparison _target;

        /// <summary>
        ///     The comparison test result.
        /// </summary>
        private bool _testResult;

        private SerializedProperty _thirdField;

        // The event properties.
        private SerializedProperty _trigger;

        private void OnEnable()
        {
            _target = (Comparison)target;
        }

        /// <inheritdoc />
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            SOFlowEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   GUILayout.Space(5f);
                                                   DrawComparisonFields();

                                                   GUILayout.Space(20f);
                                                   DrawComparisonTest();

                                                   GUILayout.Space(20f);
                                                   DrawEventData();
                                               });
        }

        /// <summary>
        ///     Draws the comparison fields.
        /// </summary>
        private void DrawComparisonFields()
        {
            SOFlowEditorUtilities.DrawSecondaryLayer(DrawTypes);
        }

        /// <summary>
        ///     Draws the operation and comparison types.
        /// </summary>
        private void DrawTypes()
        {
            _comparisonType    = serializedObject.FindProperty("ComparisonType");
            _primitiveOperator = serializedObject.FindProperty("PrimitiveOperator");
            _trigger           = serializedObject.FindProperty("Trigger");
            _dataOperator      = serializedObject.FindProperty("DataOperator");

            EditorGUILayout.PropertyField(_comparisonType);

            if((Comparison.ComparisonTypes)_comparisonType.enumValueIndex == Comparison.ComparisonTypes.String ||
               (Comparison.ComparisonTypes)_comparisonType.enumValueIndex == Comparison.ComparisonTypes.Bool   ||
               (Comparison.ComparisonTypes)_comparisonType.enumValueIndex == Comparison.ComparisonTypes.Data)
            {
                if(_primitiveOperator.enumValueIndex != (int)Comparison.PrimitiveOperators.Equal &&
                   _primitiveOperator.enumValueIndex != (int)Comparison.PrimitiveOperators.NotEqual)
                    _primitiveOperator.enumValueIndex = (int)Comparison.PrimitiveOperators.Equal;

                _primitiveOperator.enumValueIndex = EditorGUILayout.Popup("Operator",
                                                                          _primitiveOperator.enumValueIndex,
                                                                          new[]
                                                                          {
                                                                              "Equal", "Not Equal"
                                                                          });
            }
            else
            {
                EditorGUILayout.PropertyField(_primitiveOperator);
            }

            EditorGUILayout.PropertyField(_trigger);

            if((Comparison.ComparisonTypes)_comparisonType.enumValueIndex == Comparison.ComparisonTypes.Data)
                EditorGUILayout.PropertyField(_dataOperator);

            GUILayout.Space(20f);
            DrawComparisons();
        }

        /// <summary>
        ///     Draws the comparisons.
        /// </summary>
        private void DrawComparisons()
        {
            RetrieveFields();

            switch(_target.PrimitiveOperator)
            {
                case Comparison.PrimitiveOperators.Equal:
                    DrawSimpleOperation("=");

                    break;
                case Comparison.PrimitiveOperators.NotEqual:
                    DrawSimpleOperation("≠");

                    break;
                case Comparison.PrimitiveOperators.LessThan:
                    DrawSimpleOperation("<");

                    break;
                case Comparison.PrimitiveOperators.GreaterThan:
                    DrawSimpleOperation(">");

                    break;
                case Comparison.PrimitiveOperators.LessThanEqual:
                    DrawSimpleOperation("≤");

                    break;
                case Comparison.PrimitiveOperators.GreaterThanEqual:
                    DrawSimpleOperation("≥");

                    break;
                case Comparison.PrimitiveOperators.Between:
                    DrawComplexOperation("≥", "≤");

                    break;
                case Comparison.PrimitiveOperators.Outside:
                    DrawComplexOperation("<", ">");

                    break;
            }
        }

        /// <summary>
        ///     Draws comparison testing tools.
        /// </summary>
        private void DrawComparisonTest()
        {
            Color originalGUIColor = GUI.backgroundColor;

            SOFlowEditorUtilities.DrawColourLayer(_testResult
                                                  ? SOFlowEditorSettings.AcceptContextColour
                                                  : SOFlowEditorSettings.DeclineContextColour,
                                              () =>
                                              {
                                                  if(SOFlowEditorUtilities.DrawColourButton("Test Comparison"))
                                                      _testResult = _target.Evaluate();

                                                  GUI.backgroundColor =
                                                      _testResult
                                                          ? SOFlowEditorSettings.AcceptContextColour
                                                          : SOFlowEditorSettings.DeclineContextColour;

                                                  GUILayout.Label(_testResult.ToString(), SOFlowStyles.BoldCenterLabel);
                                              });

            GUI.backgroundColor = originalGUIColor;
        }

        /// <summary>
        ///     Retrieves the comparison fields for evaluation.
        /// </summary>
        private void RetrieveFields()
        {
            switch(_target.ComparisonType)
            {
                case Comparison.ComparisonTypes.Int:
                    _firstField  = serializedObject.FindProperty("FirstInt");
                    _secondField = serializedObject.FindProperty("SecondInt");
                    _thirdField  = serializedObject.FindProperty("ThirdInt");

                    break;
                case Comparison.ComparisonTypes.Float:
                    _firstField  = serializedObject.FindProperty("FirstFloat");
                    _secondField = serializedObject.FindProperty("SecondFloat");
                    _thirdField  = serializedObject.FindProperty("ThirdFloat");

                    break;
                case Comparison.ComparisonTypes.String:
                    _firstField  = serializedObject.FindProperty("FirstString");
                    _secondField = serializedObject.FindProperty("SecondString");

                    break;
                case Comparison.ComparisonTypes.Bool:
                    _firstField  = serializedObject.FindProperty("FirstBool");
                    _secondField = serializedObject.FindProperty("SecondBool");

                    break;
                case Comparison.ComparisonTypes.Data:
                    _firstField  = serializedObject.FindProperty("FirstData");
                    _secondField = serializedObject.FindProperty("SecondData");

                    break;
            }
        }

        /// <summary>
        ///     Draws a simple comparison operation.
        /// </summary>
        /// <param name="logicalOperator"></param>
        private void DrawSimpleOperation(string logicalOperator)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(_firstField, GUIContent.none, true,
                                          GUILayout.MinWidth(_minimumFieldWidth / 2f));

            EditorGUILayout.LabelField(logicalOperator, GUILayout.MaxWidth(_characterWidth));

            EditorGUILayout.PropertyField(_secondField, GUIContent.none, true,
                                          GUILayout.MinWidth(_minimumFieldWidth / 2f));

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Draws a complex comparison operation.
        /// </summary>
        /// <param name="firstLogicalOperator"></param>
        /// <param name="secondLogicalOperator"></param>
        private void DrawComplexOperation(string firstLogicalOperator, string secondLogicalOperator)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(_secondField, GUIContent.none, true,
                                          GUILayout.MinWidth(_minimumFieldWidth / 2f));

            EditorGUILayout.LabelField(firstLogicalOperator, GUILayout.MaxWidth(_characterWidth));

            EditorGUILayout.PropertyField(_firstField, GUIContent.none, true,
                                          GUILayout.MinWidth(_minimumFieldWidth / 2f));

            EditorGUILayout.LabelField(secondLogicalOperator, GUILayout.MaxWidth(_characterWidth));

            EditorGUILayout.PropertyField(_thirdField, GUIContent.none, true,
                                          GUILayout.MinWidth(_minimumFieldWidth / 2f));

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Draws the event data.
        /// </summary>
        private void DrawEventData()
        {
            _OnComparisonSuccess = serializedObject.FindProperty("OnComparisonSuccess");
            _OnComparisonFail    = serializedObject.FindProperty("OnComparisonFail");
            _respondInEditor     = serializedObject.FindProperty("RespondInEditor");

            SOFlowEditorUtilities.DrawSecondaryLayer(() =>
                                                 {
                                                     EditorGUILayout.PropertyField(_respondInEditor);
                                                     EditorGUILayout.PropertyField(_OnComparisonSuccess);
                                                     EditorGUILayout.PropertyField(_OnComparisonFail);
                                                 });
        }
    }
}
#endif