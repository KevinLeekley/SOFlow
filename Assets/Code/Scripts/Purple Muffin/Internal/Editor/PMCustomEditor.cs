// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;
#if UNITY_EDITOR
using PurpleMuffin.Utilities;
using UnityEditor;

namespace PurpleMuffin.Internal
{
    public class PMCustomEditor : Editor, ICustomEditor
    {
        /// <summary>
        ///     Indicates whether this instance is a scriptable object.
        /// </summary>
        private bool _isScriptableObject = true;

        /// <summary>
        ///     The cached scriptable object instance.
        /// </summary>
        private ScriptableObject _scriptableObjectTarget;

        public override void OnInspectorGUI()
        {
            OnCustomInspectorGUI();
        }

        /// <summary>
        ///     Handles drawing the custom inspector GUI.
        /// </summary>
        protected virtual void OnCustomInspectorGUI()
        {
            if(PMEditorSettings.DrawDefaultInspectors)
            {
                DrawDefaultInspector();
            }
            else
            {
                Color originalTextColour = GUI.contentColor;
                GUI.contentColor = PMEditorSettings.TextColour;

                DrawCustomInspector();
                DrawScriptableObjectFileHandlingFields();

                GUI.contentColor = originalTextColour;

                if(GUI.changed) serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        ///     Draws the custom inspector.
        /// </summary>
        protected virtual void DrawCustomInspector()
        {
        }

        /// <summary>
        ///     Draws file handling fields for scriptable object instances.
        /// </summary>
        protected virtual void DrawScriptableObjectFileHandlingFields()
        {
            if(_isScriptableObject && !_scriptableObjectTarget)
                try
                {
                    _scriptableObjectTarget = (ScriptableObject)target;
                    _isScriptableObject     = true;
                }
                catch
                {
                    _isScriptableObject = false;
                }

            if(_isScriptableObject && _scriptableObjectTarget)
            {
                string objectClassName = _scriptableObjectTarget.GetType().Name;

                PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.TertiaryLayerColour,
                                                            () =>
                                                            {
                                                                if(PMEditorUtilities
                                                                   .DrawColourButton($"Save {objectClassName} Data",
                                                                                     PMEditorSettings
                                                                                        .AcceptContextColour))
                                                                    _scriptableObjectTarget
                                                                       .SaveJSON($"Save {objectClassName} Data",
                                                                                 $"New {objectClassName} Data");

                                                                if(PMEditorUtilities
                                                                   .DrawColourButton($"Load {objectClassName} Data",
                                                                                     PMEditorSettings
                                                                                        .AcceptContextColour))
                                                                    _scriptableObjectTarget
                                                                       .LoadJSON($"Load {objectClassName} Data",
                                                                                 () =>
                                                                                 {
                                                                                     AssetDatabase.SaveAssets();

                                                                                     // Reselect the target instance in order to refresh the inspector and display the loaded values.
                                                                                     Selection
                                                                                        .SetActiveObjectWithContext(target,
                                                                                                                    _scriptableObjectTarget);
                                                                                 });
                                                            });
            }
        }
    }
}
#endif