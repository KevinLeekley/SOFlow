// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using UnityEditor;
using XNode;
using XNodeEditor;
using PurpleMuffin.Internal;
using UnityEngine;

namespace PurpleMuffin.Data.Graphs
{
    /// <inheritdoc />
    [CustomNodeEditor(typeof(DialogNode))]
    public class DialogNodeEditor : TraversableNodeEditor
    {
        /// <inheritdoc />
        public override void OnBodyGUI()
        {
            PMEditorUtilities.AdjustTextContrast(GetTint());

            serializedObject.Update();

            DialogNode dialog = (DialogNode)target;

            NodePort entryPort = GetDynamicPort("Entry", true, Node.ConnectionType.Multiple);
            NodePort exitPort  = GetDynamicPort("Exit",  false);

            if(dialog.Choices.Count == 0)
                PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.SecondaryLayerColour,
                                                            () =>
                                                            {
                                                                NodeEditorGUILayout.PortField(entryPort,
                                                                                              GUILayout.MinWidth(0f));

                                                                NodeEditorGUILayout.PortField(exitPort,
                                                                                              GUILayout.MinWidth(0f));
                                                            });
            else
                PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.SecondaryLayerColour,
                                                            () =>
                                                            {
                                                                NodeEditorGUILayout.PortField(entryPort);
                                                            });

            string value = dialog.Dialog.Value;

            if(value == null)
                EditorGUILayout.LabelField("Data unassigned.");
            else
                dialog.Dialog.Value =
                    EditorGUILayout.TextArea(value, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 3f));

            PMEditorUtilities.RestoreTextContrast();

            NodeEditorGUILayout.DynamicPortList(nameof(dialog.Choices), typeof(Node), serializedObject,
                                                NodePort.IO.Output, Node.ConnectionType.Override);

            serializedObject.ApplyModifiedProperties();
        }

        /// <inheritdoc />
        public override Color GetTint()
        {
            if(IsCurrentNode()) return base.GetTint();

            return PMEditorSettings.PrimaryLayerColour;
        }
    }
}
#endif