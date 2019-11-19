// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using XNode;
using XNodeEditor;
using PurpleMuffin.Internal;
using UnityEngine;

namespace PurpleMuffin.Data.Graphs
{
    /// <inheritdoc />
    [CustomNodeEditor(typeof(BranchNode))]
    public class BranchNodeEditor : TraversableNodeEditor
    {
        /// <inheritdoc />
        public override void OnBodyGUI()
        {
            PMEditorUtilities.AdjustTextContrast(GetTint());

            serializedObject.Update();

            BranchNode branch = (BranchNode)target;

            NodePort entryPort   = GetDynamicPort("Entry",   true, Node.ConnectionType.Multiple);
            NodePort defaultPort = GetDynamicPort("Default", false);

            PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.TertiaryLayerColour,
                                                        () =>
                                                        {
                                                            NodeEditorGUILayout.PortField(entryPort);
                                                        });

            PMEditorUtilities.RestoreTextContrast();

            NodeEditorGUILayout.DynamicPortList(nameof(branch.Conditions), typeof(Node), serializedObject,
                                                NodePort.IO.Output, Node.ConnectionType.Override);

            PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.TertiaryLayerColour,
                                                        () =>
                                                        {
                                                            NodeEditorGUILayout.PortField(defaultPort);
                                                        });

            serializedObject.ApplyModifiedProperties();
        }

        /// <inheritdoc />
        public override Color GetTint()
        {
            if(IsCurrentNode()) return base.GetTint();

            return PMEditorSettings.StandardNodeColour;
        }
    }
}
#endif