// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using PurpleMuffin.Internal;
using UnityEngine;
#if UNITY_EDITOR
using XNode;
using XNodeEditor;

namespace PurpleMuffin.Data.Graphs
{
    /// <inheritdoc />
    [CustomNodeEditor(typeof(TraversableNode))]
    public class TraversableNodeEditor : NodeEditor
    {
        /// <summary>
        ///     Gets the given dynamic port for this node.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="input"></param>
        /// <param name="connectionType"></param>
        /// <returns></returns>
        protected NodePort GetDynamicPort(string              port, bool input,
                                          Node.ConnectionType connectionType = Node.ConnectionType.Override)
        {
            NodePort dynamicPort = target.GetPort(port);

            if(dynamicPort == null)
            {
                if(input)
                    dynamicPort = target.AddDynamicInput(typeof(Node), connectionType,
                                                         Node.TypeConstraint.Inherited, port);
                else
                    dynamicPort = target.AddDynamicOutput(typeof(Node), connectionType,
                                                          Node.TypeConstraint.Inherited, port);
            }

            return dynamicPort;
        }

        /// <summary>
        ///     Checks if this is the currently active node within the graph.
        /// </summary>
        /// <returns></returns>
        protected bool IsCurrentNode()
        {
            return ((TraversableGraph)target.graph).CurrentNode == (TraversableNode)target;
        }

        /// <inheritdoc />
        public override void OnBodyGUI()
        {
            PMEditorUtilities.AdjustTextContrast(GetTint());

            base.OnBodyGUI();

            PMEditorUtilities.RestoreTextContrast();
        }

        /// <inheritdoc />
        public override int GetWidth()
        {
            return 300;
        }

        /// <inheritdoc />
        public override Color GetTint()
        {
            if(IsCurrentNode()) return PMEditorSettings.TriggeredNodeColour;

            return base.GetTint();
        }
    }
}
#endif