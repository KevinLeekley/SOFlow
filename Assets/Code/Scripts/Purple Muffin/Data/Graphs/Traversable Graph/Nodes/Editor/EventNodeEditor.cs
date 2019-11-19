// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using XNode;
using XNodeEditor;
using PurpleMuffin.Internal;
using UnityEngine;

namespace PurpleMuffin.Data.Graphs
{
    /// <inheritdoc />
    [CustomNodeEditor(typeof(EventNode))]
    public class EventNodeEditor : TraversableNodeEditor
    {
        /// <inheritdoc />
        public override void OnBodyGUI()
        {
            PMEditorUtilities.AdjustTextContrast(GetTint());

            serializedObject.Update();

            EventNode eventNode = (EventNode)target;

            NodePort entryPort = GetDynamicPort("Entry", true, Node.ConnectionType.Multiple);
            NodePort exitPort  = GetDynamicPort("Exit",  false);

            PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.TertiaryLayerColour,
                                                        () =>
                                                        {
                                                            NodeEditorGUILayout.PortField(entryPort,
                                                                                          GUILayout.MinWidth(0f));

                                                            NodeEditorGUILayout.PortField(exitPort,
                                                                                          GUILayout.MinWidth(0f));
                                                        });

            PMEditorUtilities.RestoreTextContrast();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(eventNode.Event)));

            serializedObject.ApplyModifiedProperties();
        }

        /// <inheritdoc />
        public override Color GetTint()
        {
            if(IsCurrentNode()) return base.GetTint();

            return PMEditorSettings.SecondaryLayerColour;
        }
    }
}
#endif